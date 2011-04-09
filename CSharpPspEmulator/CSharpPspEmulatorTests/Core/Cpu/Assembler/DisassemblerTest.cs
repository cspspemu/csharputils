using CSharpPspEmulator.Core.Cpu.Assembler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CSharpPspEmulator.Core.Cpu;

namespace CSharpPspEmulatorTest.Core.Cpu.Assembler
{
    [TestClass]
    public class DisassemblerTest
    {
        [TestMethod]
        public void AddTest()
        {
            var Disassembler = new Disassembler();
            Assert.AreEqual("add r0, r0, r0", Disassembler.Disassemble(new Disassembler.State(0x00000020)));
            Assert.AreEqual("addu r0, r0, r0", Disassembler.Disassemble(new Disassembler.State(0x00000021)));
            Assert.AreEqual("addi r0, r0, -2", Disassembler.Disassemble(new Disassembler.State(0x2000FFFE)));
            Assert.AreEqual("addiu r0, r0, 65534", Disassembler.Disassemble(new Disassembler.State(0x2400FFFE)));
        }

        [TestMethod]
        public void MemonicTest()
        {
            var Disassembler = new Disassembler();
            Assert.AreEqual("add zr, zr, zr", Disassembler.Disassemble(new Disassembler.State(0x00000020, 0, true)));
        }
    }
}
