using CSharpPspEmulator.Core.Cpu.Interpreter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CSharpPspEmulator.Core.Cpu;
using CSharpPspEmulator.Core;
using CSharpPspEmulator.Core.Cpu.Assembler;
using System.Linq;
using CSharpPspEmulator.Hle;

namespace CSharpPspEmulatorTests
{
    [TestClass]
    public class InterpretedCpuTest
    {
        static SystemHle SystemHle;
        static Assembler Assembler;
        static RegistersCpu Registers;
        static Memory Memory;
        static InterpretedCpu Cpu;
        static CpuState CpuState;

        [ClassInitialize]
        static public void Initialize(TestContext context)
        {
            SystemHle = new SystemHle();
            Assembler = new Assembler();
            Registers = new RegistersCpu();
            Memory = new Memory();
            Cpu = new InterpretedCpu();
            CpuState = new CpuState(SystemHle, Registers, Cpu, Memory);
        }

        protected void ExecuteUntilDebugBreak(params String[] Lines)
        {
            CpuState.Reset();

            uint Address = 0x08000000;
            Lines = Lines.Concat(new String[] { "dbreak" }).ToArray();
            foreach (var Line in Lines)
            {
                foreach (var InstructionData in Assembler.Assemble(Line))
                {
                    Memory.WriteUnsigned32(Address, InstructionData.Value);
                    Address += 4;
                }
            }
            CpuState.PC = 0x08000000;
            CpuState.Execute();
        }

        [TestMethod]
        public void ADDTest()
        {
            ExecuteUntilDebugBreak(new String[] {
                // Register 0 can't be writted. Will always be 0.
                "addi r0, r1, 1000",
                // r1 = 0 + 1000 = 1000
                "addi r1, r0, 1000",
                // r1 = 0 + 1002 = 1002
                "addi r1, r0, 1002",
                // r2 = 1002 + 1000 = 2002
                "addi r2, r1, 1000",
                // r3 = 2002 + 1002 = 3004
                "add  r3, r2, r1",
            });

            //Console.WriteLine(Disassembler.Disassemble(new Disassembler.State(Memory.ReadUnsigned32(0x08000000 + 4 * 3))));

            Assert.AreEqual<uint>(0, Registers[0]);
            Assert.AreEqual<uint>(1002, Registers[1]);
            Assert.AreEqual<uint>(2002, Registers[2]);
            Assert.AreEqual<uint>(3004, Registers[3]);
        }
    }
}
