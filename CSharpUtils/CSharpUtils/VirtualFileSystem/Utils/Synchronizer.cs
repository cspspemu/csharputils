using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

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

		FileSystem SourceFileSystem, DestinationFileSystem;
		String SourcePath, DestinationPath;
		SynchronizationMode _SynchronizationMode;
		ReferenceMode _ReferenceMode;

		protected Synchronizer(FileSystem SourceFileSystem, String SourcePath, FileSystem DestinationFileSystem, String DestinationPath, SynchronizationMode _SynchronizationMode, ReferenceMode _ReferenceMode)
		{
			this.SourceFileSystem = SourceFileSystem;
			this.DestinationFileSystem = DestinationFileSystem;
			this.SourcePath = SourcePath;
			this.DestinationPath = DestinationPath;
			this._SynchronizationMode = _SynchronizationMode;
			this._ReferenceMode = _ReferenceMode;
		}

		static public void Synchronize(FileSystem SourceFileSystem, String SourcePath, FileSystem DestinationFileSystem, String DestinationPath, SynchronizationMode _SynchronizationMode, ReferenceMode _ReferenceMode)
		{
			var Synchronizer = new Synchronizer(SourceFileSystem, SourcePath, DestinationFileSystem, DestinationPath, _SynchronizationMode, _ReferenceMode);
			Synchronizer.SynchronizeFolder("/");
		}

		protected void SynchronizeFolder(String Path)
		{
			var SourceFiles = FileSystemEntry.LinkedListToDictionary(this.SourceFileSystem.FindFiles(SourcePath + "/" + Path));
			var DestinationFiles = FileSystemEntry.LinkedListToDictionary(this.DestinationFileSystem.FindFiles(DestinationPath + "/" + Path));
			var FilesToUpdate = new LinkedList<FileSystemEntry>();
			var FoldersToExplore = new LinkedList<String>();

			// Folders to Explore.
			foreach (var PairSourceFile in SourceFiles)
			{
				var SourceFile = PairSourceFile.Value;
				var SourceFileName = PairSourceFile.Key;

				if (SourceFile.Type == FileSystemEntry.EntryType.Directory)
				{
					FoldersToExplore.AddLast(Path + "/" + SourceFileName);
				}
			}

			// Add New Files.
			if (_SynchronizationMode.HasFlag(SynchronizationMode.CopyNewFiles))
			{
				foreach (var PairSourceFile in SourceFiles)
				{
					var SourceFile = PairSourceFile.Value;
					var SourceFileName = PairSourceFile.Key;

					// New file (No contained in the Destination).
					if (!DestinationFiles.ContainsKey(SourceFileName))
					{
						FilesToUpdate.AddLast(SourceFile);
					}
				}
			}

			// Update Old Files.
			if (_SynchronizationMode.HasFlag(SynchronizationMode.UpdateOldFiles))
			{
				throw(new NotImplementedException());
			}

			// Delete Old Files.
			if (_SynchronizationMode.HasFlag(SynchronizationMode.DeleteOldFiles))
			{
				throw (new NotImplementedException());
			}

			foreach (var FileToUpdate in FilesToUpdate)
			{
				String FileToUpdatePathFileName = Path + "/" + FileToUpdate.Name;
				if (FileToUpdate.Type == FileSystemEntry.EntryType.Directory)
				{
					Console.WriteLine("Directory: " + FileToUpdatePathFileName);
					CreateFolder(FileToUpdatePathFileName);
				}
				else
				{
					CopyFile(FileToUpdatePathFileName);
				}
			}

			foreach (var FolderName in FoldersToExplore)
			{
				SynchronizeFolder(FolderName);
			}
		}

		protected void CreateFolder(String PathFileName)
		{
			DestinationFileSystem.CreateDirectory(DestinationPath + "/" + PathFileName);
		}

		protected void CopyFile(String PathFileName)
		{
			Stream SourceStream = SourceFileSystem.OpenFile(SourcePath + "/" + PathFileName, FileMode.Open);
			Stream DestinationStream = DestinationFileSystem.OpenFile(DestinationPath + "/" + PathFileName, FileMode.Create);
			{
				SourceStream.CopyTo(DestinationStream);
			}
			DestinationStream.Close();
			SourceStream.Close();
		}
	}
}
