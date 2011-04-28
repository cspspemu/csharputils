using CSharpUtils.VirtualFileSystem.Local;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using CSharpUtils.VirtualFileSystem;
using System.Linq;

namespace CSharpUtilsTests
{
	[TestClass]
	public class LocalFileSystemTest
	{
		LocalFileSystem LocalFileSystem;

		[TestInitialize]
		public void InitializeTest()
		{
			LocalFileSystem = new LocalFileSystem(Config.ProjectTestInputPath);
			LocalFileSystem.Mount("/Mounted", new LocalFileSystem(Config.ProjectTestInputMountedPath));
			LocalFileSystem.Mount("/NewMounted", new LocalFileSystem(Config.ProjectTestInputMountedPath), "/DirectoryOnMountedFileSystem");
		}
		[TestMethod]
		public void GetFileTimeExistsTest()
		{
			var FileTime = LocalFileSystem.GetFileTime("ExistentFolder");
			Assert.IsTrue(FileTime.CreationTime   >= DateTime.Parse("01/01/2011"));
			Assert.IsTrue(FileTime.LastAccessTime >= DateTime.Parse("01/01/2011"));
			Assert.IsTrue(FileTime.LastWriteTime  >= DateTime.Parse("01/01/2011"));
		}

		[ExpectedException(typeof(FileNotFoundException))]
		[TestMethod]
		public void GetFileTimeNotExistsTest()
		{
			LocalFileSystem.GetFileTime("ThisFilesDoesNotExists");
		}

		[TestMethod]
		public void MountedTest()
		{
			var FileTime = LocalFileSystem.GetFileTime("/Mounted/FileInMountedFileSystem.txt");
		}

		[TestMethod]
		public void Mounted2Test()
		{
			var FileTime = LocalFileSystem.GetFileTime("/NewMounted/3.txt");
		}

		[TestMethod]
		public void FindFilesTest()
		{
			Assert.IsTrue(LocalFileSystem.FindFiles("/Mounted").ContainsFileName("DirectoryOnMountedFileSystem"));
		}

		[TestMethod]
		public void FindFilesWildcardTest()
		{
			var FoundFiles = LocalFileSystem.FindFiles("/Mounted/DirectoryOnMountedFileSystem", "*.dat");
			Assert.IsTrue(FoundFiles.ContainsFileName("3.dat"));
			Assert.IsTrue(FoundFiles.ContainsFileName("4.dat"));
			Assert.IsFalse(FoundFiles.ContainsFileName("1.txt"));
		}

		[TestMethod]
		public void CopyFileTest()
		{
			var FoundFiles = LocalFileSystem.FindFiles("/Mounted/DirectoryOnMountedFileSystem", "*.dat");

			var SourcePath = "/Mounted/DirectoryOnMountedFileSystem/1.txt";
			var DestinationPath = "/Mounted/DirectoryOnMountedFileSystem/10.bin";

			try
			{
				LocalFileSystem.DeleteFile(DestinationPath);
			}
			catch
			{
			}

			LocalFileSystem.CopyFile(SourcePath, DestinationPath);

			Assert.AreEqual(
				LocalFileSystem.GetFileInfo(SourcePath).Size,
				LocalFileSystem.GetFileInfo(DestinationPath).Size
			);

			LocalFileSystem.DeleteFile(DestinationPath);
		}

		[TestMethod]
		public void MountedOpenFileTest()
		{
			var Stream = LocalFileSystem.OpenFile("/Mounted/FileInMountedFileSystem.txt", FileMode.Open);
			var StreamReader = new StreamReader(Stream);
			Assert.AreEqual("Hello World", StreamReader.ReadToEnd());
			StreamReader.Close();
			Stream.Close();
		}

		[TestMethod]
		public void MountedRecursiveTest()
		{
			var FileSystem1 = new ImplFileSystem();
			var FileSystem2 = new ImplFileSystem();
			FileSystem1.Mount("/Mounted1", FileSystem2);
			FileSystem2.Mount("/Mounted2/Mounted/test", LocalFileSystem);
			FileSystem1.GetFileTime("/Mounted1/Mounted2/Mounted/test/Mounted/../../test/Mounted/FileInMountedFileSystem.txt");
		}
	}
}
