using CSharpUtils.VirtualFileSystem.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CSharpUtils.VirtualFileSystem;
using CSharpUtils.VirtualFileSystem.Local;

namespace CSharpUtilsTests
{
    
    
    /// <summary>
    ///This is a test class for SynchronizerTest and is intended
    ///to contain all SynchronizerTest Unit Tests
    ///</summary>
	[TestClass()]
	public class SynchronizerTest
	{
		[TestMethod]
		public void SynchronizeTest()
		{
			FileSystem SourceFileSystem = new LocalFileSystem(Paths.ProjectTestInputPath);
			string SourcePath = "/";
			FileSystem DestinationFileSystem = new LocalFileSystem(Paths.ProjectTestOutputPath);
			string DestinationPath = "/";
			Synchronizer.SynchronizationMode _SynchronizationMode = Synchronizer.SynchronizationMode.CopyNewFiles;
			Synchronizer.ReferenceMode _ReferenceMode = new Synchronizer.ReferenceMode();
			Synchronizer.Synchronize(SourceFileSystem, SourcePath, DestinationFileSystem, DestinationPath, _SynchronizationMode, _ReferenceMode);
			DestinationFileSystem.GetFileTime("ExistentFolder/2/AnotherFile.txt");
		}
	}
}
