using CSharpUtils.VirtualFileSystem.Local;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using CSharpUtils.VirtualFileSystem;

namespace CSharpUtilsTests
{
    /// <summary>
    ///This is a test class for LocalFileSystemTest and is intended
    ///to contain all LocalFileSystemTest Unit Tests
    ///</summary>
	[TestClass()]
	public class LocalFileSystemTest
	{
		LocalFileSystem LocalFileSystem;

		// 
		//You can use the following additional attributes as you write your tests:
		//
		//Use ClassInitialize to run code before running the first test in the class
		//[ClassInitialize()]
		//public static void MyClassInitialize(TestContext testContext)
		//{
		//}
		//
		//Use ClassCleanup to run code after all tests in a class have run
		//[ClassCleanup()]
		//public static void MyClassCleanup()
		//{
		//}
		//
		//Use TestInitialize to run code before running each test
		[TestInitialize()]
		public void MyTestInitialize()
		{
			LocalFileSystem = new LocalFileSystem(Paths.ProjectTestInputPath);
			LocalFileSystem.Mount("/Mounted", new LocalFileSystem(Paths.ProjectTestInputMountedPath));
		}
		//
		//Use TestCleanup to run code after each test has run
		//[TestCleanup()]
		//public void MyTestCleanup()
		//{
		//}
		//

		[TestMethod]
		public void TestGetFileTimeExists()
		{
			var FileTime = LocalFileSystem.GetFileTime("ExistentFolder");
			Assert.IsTrue(FileTime.CreationTime   >= DateTime.Parse("01/01/2011"));
			Assert.IsTrue(FileTime.LastAccessTime >= DateTime.Parse("01/01/2011"));
			Assert.IsTrue(FileTime.LastWriteTime  >= DateTime.Parse("01/01/2011"));
		}

		[ExpectedException(typeof(FileNotFoundException))]
		[TestMethod]
		public void TestGetFileTimeNotExists()
		{
			LocalFileSystem.GetFileTime("ThisFilesDoesNotExists");
		}

		[TestMethod]
		public void TestMounted()
		{
			var FileTime = LocalFileSystem.GetFileTime("/Mounted/FileInMountedFileSystem.txt");
		}

		[TestMethod]
		public void TestFindFiles()
		{
			foreach (var Item in LocalFileSystem.FindFiles("/Mounted"))
			{
				//Console.WriteLine(Item);
			}
		}

		[TestMethod]
		public void TestMountedOpenFile()
		{
			var Stream = LocalFileSystem.OpenFile("/Mounted/FileInMountedFileSystem.txt", FileMode.Open);
			var StreamReader = new StreamReader(Stream);
			Assert.AreEqual("Hello World", StreamReader.ReadToEnd());
			StreamReader.Close();
			Stream.Close();
		}

		[TestMethod]
		public void TestMountedRecursive()
		{
			var FileSystem1 = new FileSystem();
			var FileSystem2 = new FileSystem();
			FileSystem1.Mount("/Mounted1", FileSystem2);
			FileSystem2.Mount("/Mounted2/Mounted/test", LocalFileSystem);
			FileSystem1.GetFileTime("/Mounted1/Mounted2/Mounted/test/Mounted/../../test/Mounted/FileInMountedFileSystem.txt");
		}
	}
}
