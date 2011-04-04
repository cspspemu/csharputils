using CSharpUtils.VirtualFileSystem.Local;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using CSharpUtils.VirtualFileSystem;

namespace CSharpUtilsTests
{
	[TestClass]
	public class LocalFileSystemTest
	{
		LocalFileSystem LocalFileSystem;

		[TestInitialize]
		public void InitializeTest()
		{
			LocalFileSystem = new LocalFileSystem(Paths.ProjectTestInputPath);
			LocalFileSystem.Mount("/Mounted", new LocalFileSystem(Paths.ProjectTestInputMountedPath));
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
		public void FindFilesTest()
		{
			foreach (var Item in LocalFileSystem.FindFiles("/Mounted"))
			{
				//Console.WriteLine(Item);
			}
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
			var FileSystem1 = new FileSystem();
			var FileSystem2 = new FileSystem();
			FileSystem1.Mount("/Mounted1", FileSystem2);
			FileSystem2.Mount("/Mounted2/Mounted/test", LocalFileSystem);
			FileSystem1.GetFileTime("/Mounted1/Mounted2/Mounted/test/Mounted/../../test/Mounted/FileInMountedFileSystem.txt");
		}
	}
}
