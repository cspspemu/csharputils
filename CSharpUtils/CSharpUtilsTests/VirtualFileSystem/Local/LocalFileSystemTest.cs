using CSharpUtils.VirtualFileSystem.Local;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
			LocalFileSystem = new LocalFileSystem(@"C:\");
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
			var Root = LocalFileSystem.Root;
			foreach (var Item in Root)
			{
				Console.WriteLine(Item);
			}
		}
	}
}
