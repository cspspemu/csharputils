using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpUtils.VirtualFileSystem.Utils
{
	public class Synchronizer
	{
		public enum SynchronizationMode
		{
			CopyNewFiles = 1,
			UpdateOldFiles = 2,
			DeleteOldFiles = 4,
			CopyNewAndUpdateOldFiles = CopyNewFiles | UpdateOldFiles,
		}

		public enum ReferenceMode
		{
			Size = 1,
			LastWriteTime = 2,
			SizeAndLastWriteTime = Size | LastWriteTime,
		}

		FileSystem FileSystemSource;
		FileSystem FileSystemDestination;
		SynchronizationMode _SynchronizationMode;
		ReferenceMode _ReferenceMode;

		protected Synchronizer(FileSystem FileSystemSource, FileSystem FileSystemDestination, SynchronizationMode _SynchronizationMode, ReferenceMode _ReferenceMode)
		{
			this.FileSystemSource = FileSystemSource;
			this.FileSystemDestination = FileSystemDestination;
			this._SynchronizationMode = _SynchronizationMode;
			this._ReferenceMode = _ReferenceMode;
		}

		static public void Synchronize(FileSystem FileSystemSource, FileSystem FileSystemDestination, SynchronizationMode _SynchronizationMode = SynchronizationMode.CopyNewAndUpdateOldFiles, ReferenceMode _ReferenceMode = ReferenceMode.Size)
		{
			var Synchronizer = new Synchronizer(FileSystemSource, FileSystemDestination, _SynchronizationMode, _ReferenceMode);
			Synchronizer.SynchronizeFolder("/");
		}

		public void SynchronizeFolder(String Path)
		{
			var SourceFiles = this.FileSystemSource.FindFiles(Path);
			var DestinationFiles = this.FileSystemDestination.FindFiles(Path);
		}
	}
}
