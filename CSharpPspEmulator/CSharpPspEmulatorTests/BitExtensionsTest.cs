using CSharpPspEmulator.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CSharpPspEmulatorTests
{
    [TestClass]
    public class BitExtensionsTest
    {
        [TestMethod]
        public void InsertTest()
        {
            uint ValueIn = 0x07000011;
            uint ValueOut = ValueIn.Insert(16, 8, 0xFFFF);
            Assert.AreEqual(0x07FF0011, ValueOut);
        }

        [TestMethod]
        public void ExtractUnsignedTest()
        {
            uint ValueIn = 0x07315411;
            uint ValueOut = ValueIn.ExtractUnsigned(8, 16);
            Assert.AreEqual(0x3154, ValueOut);
        }
    }
}
