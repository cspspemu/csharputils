using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSharpUtils;
using System.IO;

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
			Assert.AreEqual(0x12345678, Test.Uint);
			Assert.AreEqual("Hello", Test.String);
		}
	}
}
