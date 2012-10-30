using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using CSharpUtils;
using System.Diagnostics;

namespace CSharpUtils.VirtualFileSystem
{
	// http://msdn.microsoft.com/en-us/library/system.io.isolatedstorage.isolatedstoragefile.aspx
	// http://vfs.codeplex.com/
	abstract public partial class FileSystem : IDisposable
	{
		//public FileSystemFileStream CreateFile(String FileName, uint DesiredAccess, uint ShareMode, uint CreationDisposition, uint FlagsAndAttributes) {

		// http://docs.python.org/whatsnew/2.6.html#pep-343-the-with-statement
		// http://msdn.microsoft.com/en-us/library/yh598w02(v=VS.100).aspx
		abstract protected FileSystemFileStream ImplOpenFile(String FileName, FileMode FileMode);
		
		[DebuggerHidden]
		public FileSystemFileStream OpenFile(String FileName, FileMode FileMode)
		{
			FileSystem NewFileSystem; String NewFileName; Access(FileName, out NewFileSystem, out NewFileName);
			return NewFileSystem.ImplOpenFile(NewFileName, FileMode);
		}

		[DebuggerHidden]
		public FileSystemFileStream OpenFileRW(String FileName)
		{
			return OpenFile(FileName, FileMode.Open);
		}

		[DebuggerHidden]
		public FileSystemFileStream OpenFileRead(String FileName)
		{
			return OpenFile(FileName, FileMode.Open);
		}

#if false
		abstract protected void ImplWriteFile(FileSystemFileStream FileStream, byte[] Buffer, int Offset, int Count);
		public void WriteFile(FileSystemFileStream FileStream, byte[] Buffer, int Offset, int Count)
		{
			ImplWriteFile(FileStream, Buffer, Offset, Count);
		}

		abstract protected int ImplReadFile(FileSystemFileStream FileStream, byte[] Buffer, int Offset, int Count);
		public int ReadFile(FileSystemFileStream FileStream, byte[] Buffer, int Offset, int Count)
		{
			return ImplReadFile(FileStream, Buffer, Offset, Count);
		}

		abstract protected void ImplCloseFile(FileSystemFileStream FileStream);
		public void CloseFile(FileSystemFileStream FileStream)
		{
			ImplCloseFile(FileStream);
		}
#endif

		public bool Exists(string FileName)
		{
			try
			{
				var Info = GetFileInfo(FileName);
				return true;
			}
			catch
			{
				return false;
			}
		}

		public void WriteAllBytes(string FileName, byte[] Buffer)
		{
			using (var File = OpenFile(FileName, FileMode.Truncate))
			{
				File.Write(Buffer, 0, Buffer.Length);
			}
		}

		public byte[] ReadAllBytes(string FileName)
		{
			using (var File = OpenFile(FileName, FileMode.Open))
			{
				var Bytes = new byte[File.Length];
				File.Read(Bytes, 0, Bytes.Length);
				return Bytes;
			}

		}

		abstract protected void ImplSetFileTime(String Path, FileSystemEntry.FileTime FileTime);
		public void SetFileTime(String Path, FileSystemEntry.FileTime FileTime)
		{
			FileSystem NewFileSystem; String NewPath; Access(Path, out NewFileSystem, out NewPath);
			NewFileSystem.ImplSetFileTime(NewPath, FileTime);
		}

		abstract protected FileSystemEntry ImplGetFileInfo(String Path);
		public FileSystemEntry GetFileInfo(String Path)
		{
			FileSystem NewFileSystem; String NewPath; Access(Path, out NewFileSystem, out NewPath);
			return NewFileSystem.ImplGetFileInfo(NewPath);
		}

		public FileSystemEntry.FileTime GetFileTime(String Path)
		{
			return this.GetFileInfo(Path).Time;
		}

		abstract protected void ImplDeleteFile(String Path);
		public void DeleteFile(String Path)
		{
			FileSystem NewFileSystem; String NewPath; Access(Path, out NewFileSystem, out NewPath);
			NewFileSystem.ImplDeleteFile(NewPath);
		}

		abstract protected void ImplDeleteDirectory(String Path);
		public void DeleteDirectory(String Path)
		{
			FileSystem NewFileSystem; String NewPath; Access(Path, out NewFileSystem, out NewPath);
			NewFileSystem.ImplDeleteDirectory(NewPath);
		}

		abstract protected void ImplCreateDirectory(String Path, int Mode = 0777);
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

		abstract protected void ImplMoveFile(String ExistingFileName, String NewFileName, bool ReplaceExisiting);
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

		virtual protected FileSystemEntry FilterFileSystemEntry(FileSystemEntry FileSystemEntry)
		{
			return FileSystemEntry;
		}

		abstract protected IEnumerable<FileSystemEntry> ImplFindFiles(String Path);
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
