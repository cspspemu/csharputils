using System.IO;
using CSharpUtils.Fastcgi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpUtils.Tests
{
	[TestClass]
	public class FastcgiPacketReaderTest
	{
		[TestMethod]
		public void ReadVariableIntSingleByteTest()
		{
			var Stream = new MemoryStream();
			Stream.WriteByte(0x7F);
			Stream.Position = 0;
			Assert.AreEqual(0x7F, FastcgiPacketReader.ReadVariableInt(Stream));
		}

		[TestMethod]
		public void ReadVariableIntMultipleByteTest()
		{
			var Stream = new MemoryStream();
			Stream.WriteByte(0x80);
			Stream.WriteByte(0x00);
			Stream.WriteByte(0x00);
			Stream.WriteByte(0x01);
			Stream.Position = 0;
			Assert.AreEqual(1, FastcgiPacketReader.ReadVariableInt(Stream));
		}
		
		[TestMethod]
		public void ReadVariableIntMultipleByte2Test()
		{
			var Stream = new MemoryStream();
			Stream.WriteByte(0x12 | 0x80);
			Stream.WriteByte(0x34);
			Stream.WriteByte(0x56);
			Stream.WriteByte(0x78);
			Stream.Position = 0;

			Assert.AreEqual(0x12345678, FastcgiPacketReader.ReadVariableInt(Stream));
		}
	}
}
