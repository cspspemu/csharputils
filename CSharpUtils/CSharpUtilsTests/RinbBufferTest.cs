using System.IO;
using System.Linq;
using CSharpUtils.Streams;
using NUnit.Framework;
using CSharpUtils;
using System;
using System.Collections;

namespace CSharpUtilsTests
{
    [TestFixture]
    public class RingBufferTest
    {
		RingBuffer<byte> RingBuffer;

        [SetUp]
        public void InitializeTest()
        {
			RingBuffer = new RingBuffer<byte>(32);
        }

        [Test]
        public void TestInitialState()
        {
			Assert.AreEqual(32, RingBuffer.Capacity);
			Assert.AreEqual(32, RingBuffer.WriteAvailable);
			Assert.AreEqual(0, RingBuffer.ReadAvailable);
        }
	
		[Test]
		[ExpectedException(typeof(OverflowException))]
		public void ReadEmpty()
		{
			RingBuffer.Read();
		}

		[Test]
		public void WriteReadSingle()
		{
			Assert.AreEqual(32, RingBuffer.Capacity);
			Assert.AreEqual(32, RingBuffer.WriteAvailable);
			Assert.AreEqual(0, RingBuffer.ReadAvailable);

			RingBuffer.Write(1);
	
			Assert.AreEqual(32, RingBuffer.Capacity);
			Assert.AreEqual(31, RingBuffer.WriteAvailable);
			Assert.AreEqual(1, RingBuffer.ReadAvailable);

			Assert.AreEqual(1, RingBuffer.Read());
		}

		[Test]
		public void WriteFull()
		{
			foreach (var n in Enumerable.Range(0, RingBuffer.Capacity)) RingBuffer.Write(0);
		}

		[Test]
		[ExpectedException(typeof(OverflowException))]
		public void WriteFullPlus1()
		{
			foreach (var n in Enumerable.Range(0, RingBuffer.Capacity + 1)) RingBuffer.Write(0);
		}
	}
}
