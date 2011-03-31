using CSharpUtils.VirtualFileSystem.Ftp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.IO;

namespace CSharpUtilsTests
{
    
    
    /// <summary>
    ///This is a test class for FtpFileSystemTest and is intended
    ///to contain all FtpFileSystemTest Unit Tests
    ///</summary>
	[TestClass()]
	public class FtpFileSystemTest
	{
		static FtpFileSystem FtpFileSystem;

		[ClassInitialize]
		public static void Initialize(TestContext testContext)
		{
			FtpFileSystem = new FtpFileSystem();
			FtpFileSystem.Connect("192.168.1.36", 21, "ubuntu", "ubuntu");
		}

		[ClassCleanup]
		public static void Cleanup()
		{
			FtpFileSystem.Shutdown();
		}

		[TestMethod]
		public void GetFileTimeTest()
		{
			Assert.IsTrue(FtpFileSystem.GetFileTime(".bashrc").LastWriteTime >= DateTime.Parse("01/01/2008"));
		}

		[TestMethod]
		public void FindFilesTest()
		{
			Assert.AreEqual(
				1,
				FtpFileSystem.FindFiles("/").Where(
					(FileSystemEntry) => (FileSystemEntry.Name == "Desktop")
				).Count()
			);
		}

		[TestMethod]
		public void OpenFileTest()
		{
			foreach (var Item in FtpFileSystem.FindFiles("/Desktop/temp")) {
				Console.WriteLine(Item);
			}
			var Text = new StreamReader(FtpFileSystem.OpenFile("/Desktop/temp/test.c", FileMode.Open)).ReadToEnd();

			Console.WriteLine(Text);
		}

		[TestMethod]
		public void DeleteFileTest()
		{
			String FileName = "/Desktop/1.txt";
			FtpFileSystem.OpenFile(FileName, FileMode.Create).Close();
			try
			{
				FtpFileSystem.GetFileTime(FileName);
			}
			catch
			{
				Assert.Fail();
			}
			FtpFileSystem.DeleteFile(FileName);
			try
			{
				FtpFileSystem.GetFileTime(FileName);
				Assert.Fail();
			}
			catch
			{
			}
		}

		[TestMethod]
		public void WriteNewFileTest()
		{
			DateTime StartTime = DateTime.Now - TimeSpan.Parse("10");
			var StreamWriter = new StreamWriter(FtpFileSystem.OpenFile("/Desktop/demo.txt", FileMode.Create));
			StreamWriter.WriteLine("Hello World!");
			StreamWriter.Close();
			Assert.IsTrue(FtpFileSystem.GetFileTime("/Desktop/demo.txt").LastWriteTime >= StartTime);
			FtpFileSystem.DeleteFile("/Desktop/demo.txt");
		}
	}
}
