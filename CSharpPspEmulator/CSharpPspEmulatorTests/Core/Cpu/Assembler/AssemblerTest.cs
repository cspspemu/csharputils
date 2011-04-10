using CSharpPspEmulator.Core.Cpu.Assembler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CSharpPspEmulator.Core.Cpu;
using System.Collections.Generic;

namespace CSharpPspEmulatorTests
{
    [TestClass]
    public class AssemblerTest
    {
        [TestMethod]
        public void AssembleRegisterPlusImmTest()
        {
            Assembler Assembler = new Assembler();
            var InstructionDataList = Assembler.Assemble("addi r1, r0, 1000");
            Assert.AreEqual(1, InstructionDataList.Count);
            Assert.AreEqual(0x200103E8, InstructionDataList[0].Value);
        }

        [TestMethod]
        public void AssembleRegisterPlusImmSignedTest()
        {
            Assembler Assembler = new Assembler();
            var InstructionDataList = Assembler.Assemble("addi r1, r0, -1000");
            Console.WriteLine(InstructionDataList[0]);
            Assert.AreEqual(1, InstructionDataList.Count);
            Assert.AreEqual(0x200103E8, InstructionDataList[0].Value);
        }
    }
}
