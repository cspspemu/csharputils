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
			string SourcePath = "/";
			FileSystem DestinationFileSystem = new LocalFileSystem(Config.ProjectTestOutputPath);
			string DestinationPath = "/";
			Synchronizer.SynchronizationMode _SynchronizationMode = Synchronizer.SynchronizationMode.CopyNewAndUpdateOldFiles;
			Synchronizer.ReferenceMode _ReferenceMode = new Synchronizer.ReferenceMode();
			Synchronizer.Synchronize(SourceFileSystem, SourcePath, DestinationFileSystem, DestinationPath, _SynchronizationMode, _ReferenceMode);
			DestinationFileSystem.GetFileTime("ExistentFolder/2/AnotherFile.txt");
		}
	}
}
