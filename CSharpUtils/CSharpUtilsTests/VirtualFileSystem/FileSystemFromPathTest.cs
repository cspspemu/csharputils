using CSharpUtils.VirtualFileSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CSharpUtils.VirtualFileSystem.Local;
using System.Linq;
using CSharpUtils;

namespace CSharpUtilsTests.VirtualFileSystem
{
	[TestClass]
	public class FileSystemFromPathTest
	{
		[TestMethod]
		public void FileSystemFromPathConstructorTest()
		{
			var LocalFileSystem = new LocalFileSystem(Config.ProjectTestInputMountedPath);
			var LocalFileSystemAccessed = LocalFileSystem.FileSystemFromPath("DirectoryOnMountedFileSystem", false);
			Assert.IsTrue(LocalFileSystemAccessed.ExistsFile("1.txt"));
			Assert.IsFalse(LocalFileSystemAccessed.ExistsFile("../FileInMountedFileSystem.txt"));
		}
	}
}
