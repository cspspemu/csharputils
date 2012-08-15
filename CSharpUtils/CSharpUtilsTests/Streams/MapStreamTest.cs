using System;
using CSharpUtils.Streams;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpUtilsTests.Streams
{
	[TestClass]
	public class MapStreamTest
	{
		[TestMethod]
		public void TestRead()
		{
			var Stream1 = new ZeroStream(5, 0x11);
			var Stream2 = new ZeroStream(3, 0x22);
			var MapStream = new MapStream();
			byte[] Readed1, Readed2, Readed3;

			MapStream.Map(3, Stream1);
			MapStream.Map(3 + 5, Stream2);

			MapStream.Position = 4;

			Readed1 = MapStream.ReadBytesUpTo(3);
			CollectionAssert.AreEqual(new byte[] { 0x11, 0x11, 0x11 }, Readed1);

			Readed2 = MapStream.ReadBytesUpTo(3);
			CollectionAssert.AreEqual(new byte[] { 0x11, 0x22, 0x22 }, Readed2);

			Readed3 = MapStream.ReadBytesUpTo(1);
			CollectionAssert.AreEqual(new byte[] { 0x22 }, Readed3);
		}

		[TestMethod]
		public void TestReadUnmapped0()
		{
			var MapStream = new MapStream();
			MapStream.Read(new byte[1], 0, 0);
		}

		[TestMethod]
		public void TestReadUnmapped1()
		{
			try
			{
				var MapStream = new MapStream();
				MapStream.Read(new byte[1], 0, 1);
				Assert.Fail();
			}
			catch (InvalidOperationException)
			{
			}
		}
	}
}
