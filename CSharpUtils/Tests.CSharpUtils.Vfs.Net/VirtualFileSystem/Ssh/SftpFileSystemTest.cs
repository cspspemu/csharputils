using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSharpUtils.VirtualFileSystem.Ssh;
using System.Net;
using System.IO;
using CSharpUtils;

namespace CSharpUtilsTests.VirtualFileSystem.Ssh
{
	[TestClass]
	public class SftpFileSystemTest
	{
        // @TODO: These should be Harness tests.
        /*

		protected static SftpFileSystem SftpFileSystem;

		[ClassInitialize]
		public static void Initialize(TestContext testContext)
		{
			SftpFileSystem = new SftpFileSystem(Config.RemoteIp, 22, "ubuntu", "ubuntu", 1000);
		}

		[ClassCleanup]
		public static void Cleanup()
		{
			SftpFileSystem.Shutdown();
		}

		[TestMethod]
		public void FindFilesTest()
		{
			var FilesQuery =
				from File in SftpFileSystem.FindFiles("/home/ubuntu")
				where File.Name == "Desktop"
				select File
			;

			Assert.AreEqual(
				1,
				FilesQuery.Count()
			);
		}

		[TestMethod]
		public void GetFileTimeTest()
		{
			var time = SftpFileSystem.GetFileTime("/home/ubuntu/this is a test.txt");
			Console.WriteLine(time);
		}

		[TestMethod]
		public void DownloadFileTest()
		{
			Assert.AreEqual("Hello World\n", SftpFileSystem.OpenFile("/home/ubuntu/this is a test.txt", FileMode.Open).ReadAllContentsAsString(Encoding.UTF8));
		}

		[TestMethod]
		public void ModifyFileTest()
		{
			//var fs = new FileStream(@"C:\projects\csharputils\temp.bin", FileMode.Create);
			//fs.Close();

			Stream Stream = SftpFileSystem.OpenFile("/home/ubuntu/myfile.txt", FileMode.Create);
			var StreamWriter = new StreamWriter(Stream);
			StreamWriter.Write("This is a string writed");
			StreamWriter.Close();
			//Stream.Close();

			Assert.AreEqual("This is a string writed", SftpFileSystem.OpenFile("/home/ubuntu/myfile.txt", FileMode.Open).ReadAllContentsAsString(Encoding.UTF8));
		}
        */
	}
}
