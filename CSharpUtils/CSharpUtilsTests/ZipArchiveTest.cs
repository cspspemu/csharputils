using CSharpUtils.Compression.Zip;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using CSharpUtils;
using System.Text;

namespace CSharpUtilsTests
{
	[TestClass]
	public class ZipArchiveTest
	{
		[TestMethod]
		public void LoadTest()
		{
			var ZipArchive = new ZipArchive();
			ZipArchive.Load(File.OpenRead(Config.ProjectTestInputPath + @"\TestInputMounted.zip"));
			var Text = ZipArchive.Files["CompressedFile.txt"].OpenRead().ReadAllContentsAsString(Encoding.UTF8);
			Assert.AreEqual(Text.Substr(-13), "compressible.");
		}
	}
}
