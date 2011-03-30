using CSharpUtils.VirtualFileSystem.Local;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CSharpUtils.VirtualFileSystem;
using System.IO;

namespace CSharpUtilsTests
{
    
    
    /// <summary>
    ///This is a test class for LocalFileSystemTest and is intended
    ///to contain all LocalFileSystemTest Unit Tests
    ///</summary>
	[TestClass()]
	public class LocalFileSystemTest
	{
		String TestInputDirectory;
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
			TestInputDirectory = Directory.GetCurrentDirectory() + @"\..\..\..\TestInput";
			LocalFileSystem = new LocalFileSystem(TestInputDirectory);
		}
		//
		//Use TestCleanup to run code after each test has run
		//[TestCleanup()]
		//public void MyTestCleanup()
		//{
		//}
		//


		[TestMethod()]
		public void RootTest()
		{
			var Root = LocalFileSystem.Root;
			Assert.AreEqual(Root.Name, "");
		}

		[TestMethod()]
		public void ListTest()
		{
			foreach (var Item in LocalFileSystem.Root)
			{
				Console.WriteLine(Item);
			}
		}

		[TestMethod()]
		public void OpenFile()
		{
			var sw = new StreamWriter(LocalFileSystem.Root.Open("test.txt", System.IO.FileMode.Create));
			sw.WriteLine("Hello World from C#!");
			sw.Close();
			Assert.IsTrue(File.Exists(TestInputDirectory + "/test.txt"));
		}
	}
}
