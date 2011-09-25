using CSharpUtils.Compression.Lz;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CSharpUtilsTests
{
    [TestClass()]
    public class LzBufferTest
    {
        [TestMethod()]
        public void FindMaxSequenceTest()
        {
            LzBuffer LzBuffer = new LzBuffer(3);
            LzBuffer.AddBytes(new byte[] {
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18,
                7, 8, 9, 10, 12, 13,
                7, 8, 9, 10, 11, 16
            });

            var Result = LzBuffer.FindMaxSequence(
                LzBuffer.Size - 6,
                LzBuffer.Size - 6,
                0x1000,
                3,
                16,
                true
            );

            Assert.AreEqual("LzBuffer.FindSequenceResult(Offset=7, Size=5)", Result.ToString());
        }
    }
}
