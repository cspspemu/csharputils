using CSharpUtils.VirtualFileSystem;
using CSharpUtils.VirtualFileSystem.Local;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
