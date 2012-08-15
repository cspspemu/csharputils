using CSharpUtils.VirtualFileSystem.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CSharpUtils.VirtualFileSystem;
using CSharpUtils.VirtualFileSystem.Local;

namespace CSharpUtilsTests
{
	[TestClass]
	public class SynchronizerTest
	{
		[TestInitialize]
		public void TestInitialize()
		{
			RestoreFileSystem();
		}

		[TestCleanup]
		public void TestCleanup()
		{
			RestoreFileSystem();
		}

		void RestoreFileSystem()
		{

		}

		[TestMethod]
		public void SynchronizeTest()
		{
			FileSystem SourceFileSystem = new LocalFileSystem(Config.ProjectTestInputPath);
			FileSystem DestinationFileSystem = new LocalFileSystem(Config.ProjectTestOutputPath);
			Synchronizer.Synchronize(
				SourceFileSystem, ".",
				DestinationFileSystem, ".",
				Synchronizer.SynchronizationMode.CopyNewAndUpdateOldFiles,
				Synchronizer.ReferenceMode.SizeAndLastWriteTime
			);
			DestinationFileSystem.GetFileTime("ExistentFolder/2/AnotherFile.txt");
		}
	}
}
