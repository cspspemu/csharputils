using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;

namespace CSharpUtils.VirtualFileSystem
{
	// http://msdn.microsoft.com/en-us/library/system.io.isolatedstorage.isolatedstoragefile.aspx
	// http://vfs.codeplex.com/
	public abstract partial class FileSystem : IDisposable
	{
		//public FileSystemFileStream CreateFile(String FileName, uint DesiredAccess, uint ShareMode, uint CreationDisposition, uint FlagsAndAttributes) {

		// http://docs.python.org/whatsnew/2.6.html#pep-343-the-with-statement
		// http://msdn.microsoft.com/en-us/library/yh598w02(v=VS.100).aspx
		protected abstract FileSystemFileStream ImplOpenFile(String FileName, FileMode FileMode);
		public FileSystemFileStream OpenFile(String FileName, FileMode FileMode)
		{
			FileSystem NewFileSystem; String NewFileName; Access(FileName, out NewFileSystem, out NewFileName);
			return NewFileSystem.ImplOpenFile(NewFileName, FileMode);
		}

		protected abstract void ImplWriteFile(FileSystemFileStream FileStream, byte[] Buffer, int Offset, int Count);
		public void WriteFile(FileSystemFileStream FileStream, byte[] Buffer, int Offset, int Count)
		{
			ImplWriteFile(FileStream, Buffer, Offset, Count);
		}

		protected abstract int ImplReadFile(FileSystemFileStream FileStream, byte[] Buffer, int Offset, int Count);
		public int ReadFile(FileSystemFileStream FileStream, byte[] Buffer, int Offset, int Count)
		{
			return ImplReadFile(FileStream, Buffer, Offset, Count);
		}

		protected abstract void ImplCloseFile(FileSystemFileStream FileStream);
		public void CloseFile(FileSystemFileStream FileStream)
		{
			ImplCloseFile(FileStream);
		}

		protected abstract void ImplSetFileTime(String Path, FileSystemEntry.FileTime FileTime);
		public void SetFileTime(String Path, FileSystemEntry.FileTime FileTime)
		{
			FileSystem NewFileSystem; String NewPath; Access(Path, out NewFileSystem, out NewPath);
			NewFileSystem.ImplSetFileTime(NewPath, FileTime);
		}

		protected abstract FileSystemEntry ImplGetFileInfo(String Path);
		public FileSystemEntry GetFileInfo(String Path)
		{
			FileSystem NewFileSystem; String NewPath; Access(Path, out NewFileSystem, out NewPath);
			return NewFileSystem.ImplGetFileInfo(NewPath);
		}

		public FileSystemEntry.FileTime GetFileTime(String Path)
		{
			return this.GetFileInfo(Path).Time;
		}

		protected abstract void ImplDeleteFile(String Path);
		public void DeleteFile(String Path)
		{
			FileSystem NewFileSystem; String NewPath; Access(Path, out NewFileSystem, out NewPath);
			NewFileSystem.ImplDeleteFile(NewPath);
		}

		protected abstract void ImplDeleteDirectory(String Path);
		public void DeleteDirectory(String Path)
		{
			FileSystem NewFileSystem; String NewPath; Access(Path, out NewFileSystem, out NewPath);
			NewFileSystem.ImplDeleteDirectory(NewPath);
		}

		protected abstract void ImplCreateDirectory(String Path, int Mode = 0777);
		public void CreateDirectory(String Path, int Mode = 0777, bool ThrowErrorIfNotExists = true)
		{
			FileSystem NewFileSystem; String NewPath; Access(Path, out NewFileSystem, out NewPath);
			if (ThrowErrorIfNotExists)
			{
				NewFileSystem.ImplCreateDirectory(NewPath, Mode);
			}
			else
			{
				try
				{
					NewFileSystem.ImplCreateDirectory(NewPath, Mode);
				}
				catch
				{
				}
			}
		}

		protected abstract void ImplMoveFile(String ExistingFileName, String NewFileName, bool ReplaceExisiting);
		public void MoveFile(String ExistingFileName, String NewFileName, bool ReplaceExisiting)
		{
			FileSystem NewFileSystem1; String NewPath1;
			FileSystem NewFileSystem2; String NewPath2;

			Access(ExistingFileName, out NewFileSystem1, out NewPath1);
			Access(NewFileName, out NewFileSystem2, out NewPath2);

			if (NewFileSystem1 != NewFileSystem2)
			{
				throw(new Exception("Can't MoveFile in different file systems"));
			}

			NewFileSystem1.ImplMoveFile(NewPath1, NewPath2, ReplaceExisiting);
		}

		public IEnumerable<FileSystemEntry> FindMountedFiles(String NewPath)
		{
			foreach (var MountedFileSystemPath in MountedFileSystems.Keys)
			{
				if (MountedFileSystemPath.StartsWith(NewPath))
				{
					var Components = MountedFileSystemPath.Substring(NewPath.Length).TrimStart('/').Split('/');
					FileSystemEntry FileSystemEntry = new FileSystemEntry(this, Components[0]);
					FileSystemEntry.Type = VirtualFileSystem.FileSystemEntry.EntryType.Directory;
					yield return FileSystemEntry;
				}
			}
		}

		protected virtual FileSystemEntry FilterFileSystemEntry(FileSystemEntry FileSystemEntry)
		{
			return FileSystemEntry;
		}

		protected abstract IEnumerable<FileSystemEntry> ImplFindFiles(String Path);
		public IEnumerable<FileSystemEntry> FindFiles(String Path)
		{
			FileSystem NewFileSystem; String NewPath; Access(Path, out NewFileSystem, out NewPath);

			return NewFileSystem.FindMountedFiles(NewPath)
				.Concat(NewFileSystem.ImplFindFiles(NewPath))
				.DistinctByKey(FileSystemEntry => FileSystemEntry.Name)
				.Select(FileSystemEntry => FilterFileSystemEntry(FileSystemEntry))
			;
		}

		public IEnumerable<FileSystemEntry> FindFiles(String Path, Regex Regex)
		{
			return FindFiles(Path)
				.Where(Entry => Regex.IsMatch(Entry.Name))
			;
		}

		public IEnumerable<FileSystemEntry> FindFiles(String Path, Wildcard Wildcard)
		{
			return FindFiles(Path, (Regex)Wildcard);
		}
	}
}
