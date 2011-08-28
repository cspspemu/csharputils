using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;
using CSharpUtils.Extensions;
using CSharpUtils.Compression;

namespace CSharpUtilsTest
{
	[TestClass]
	public class HuffmanTest
	{
		[TestMethod]
		public void HuffmanCompressUncompressTest()
		{
			var ThisEncoding = Encoding.UTF8;

			var DataString1 = "Hola. Esto es una prueba para ver si funciona la compresión Huffman.";
			var DataBytes1 = ThisEncoding.GetBytes(DataString1);
			var DataStream = new MemoryStream(DataBytes1);
			var UsageTable = Huffman.CalculateUsageTable(DataBytes1);
			var EncodingTable = Huffman.BuildTable(UsageTable);
			var DataBytes2 = Huffman.Uncompress(Huffman.Compress(DataStream, EncodingTable), (uint)DataBytes1.Length, EncodingTable).ReadAll();
			var DataString2 = ThisEncoding.GetString(DataBytes2);

			Assert.AreEqual(DataString1, DataString2);
		}
	}
}
