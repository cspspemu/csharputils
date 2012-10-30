using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpUtilsTests.Extensions
{
	[TestClass]
	public class StreamExtensionsTest
	{
		struct Test
		{
			public uint Uint;
			public string String;
		}

		[TestMethod]
		public void TestReadManagedStruct()
		{
			var MemoryStream = new MemoryStream();
			var BinaryWriter = new BinaryWriter(MemoryStream);
			BinaryWriter.Write((uint)0x12345678);
			BinaryWriter.Write((byte)'H');
			BinaryWriter.Write((byte)'e');
			BinaryWriter.Write((byte)'l');
			BinaryWriter.Write((byte)'l');
			BinaryWriter.Write((byte)'o');
			BinaryWriter.Write((byte)0);
			MemoryStream.Position = 0;
			var Test = MemoryStream.ReadManagedStruct<Test>();
			Assert.AreEqual(0x12345678U, Test.Uint);
			Assert.AreEqual("Hello", Test.String);
		}

		struct TestShorts
		{
			public short A, B, C;

			public override string ToString()
			{
				return String.Format("TestShorts(0x{0:X4}, 0x{1:X4}, 0x{2:X4})", A, B, C);
			}
		}

		[TestMethod]
		public void ReadStructVectorTest()
		{
			var Data = new byte[] {
				0x01, 0x00, 0x02, 0x00, 0x03, 0x00,
				0x01, 0x01, 0x02, 0x01, 0x03, 0x01,
				0x01, 0x02, 0x02, 0x02, 0x03, 0x02,
			};
			var TestShorts = (new MemoryStream(Data)).ReadStructVector<TestShorts>(3);
			Assert.AreEqual("TestShorts(0x0001, 0x0002, 0x0003)", TestShorts[0].ToString());
			Assert.AreEqual("TestShorts(0x0101, 0x0102, 0x0103)", TestShorts[1].ToString());
			Assert.AreEqual("TestShorts(0x0201, 0x0202, 0x0203)", TestShorts[2].ToString());
		}

		[TestMethod]
		public void WriteStructVectorTest()
		{
			var Data = new byte[] {
				0x01, 0x00, 0x02, 0x00, 0x03, 0x00,
				0x01, 0x01, 0x02, 0x01, 0x03, 0x01,
				0x01, 0x02, 0x02, 0x02, 0x03, 0x02,
			};
			var Shorts = new[] {
				new TestShorts() { A = 0x0001, B = 0x0002, C = 0x0003 },
				new TestShorts() { A = 0x0101, B = 0x0102, C = 0x0103 },
				new TestShorts() { A = 0x0201, B = 0x0202, C = 0x0203 },
			};
			var MemoryStream = new MemoryStream();
			MemoryStream.WriteStructVector(Shorts);
			Assert.AreEqual(
				Data.ToHexString(),
				MemoryStream.ToArray().ToHexString()
			);
		}
	}
}
