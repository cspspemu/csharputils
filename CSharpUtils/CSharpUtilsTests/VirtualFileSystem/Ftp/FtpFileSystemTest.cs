using CSharpUtils.VirtualFileSystem.Ftp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

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
	}
}
