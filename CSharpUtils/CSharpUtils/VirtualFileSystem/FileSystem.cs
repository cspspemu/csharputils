using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

namespace CSharpUtils.VirtualFileSystem
{
	// http://msdn.microsoft.com/en-us/library/system.io.isolatedstorage.isolatedstoragefile.aspx
	// http://vfs.codeplex.com/
	abstract public partial class FileSystem : IDisposable
	{
		#region Public Methods
		//public FileSystemFileStream CreateFile(String FileName, uint DesiredAccess, uint ShareMode, uint CreationDisposition, uint FlagsAndAttributes) {

		// http://docs.python.org/whatsnew/2.6.html#pep-343-the-with-statement
		// http://msdn.microsoft.com/en-us/library/yh598w02(v=VS.100).aspx
		public FileSystemFileStream OpenFile(String FileName, FileMode FileMode)
		{
			FileSystem NewFileSystem; String NewFileName; Access(FileName, out NewFileSystem, out NewFileName);
			return NewFileSystem.ImplOpenFile(NewFileName, FileMode);
		}

		public void WriteFile(FileSystemFileStream FileStream, byte[] Buffer, int Offset, int Count)
		{
			ImplWriteFile(FileStream, Buffer, Offset, Count);
		}

		public int ReadFile(FileSystemFileStream FileStream, byte[] Buffer, int Offset, int Count)
		{
			return ImplReadFile(FileStream, Buffer, Offset, Count);
		}

		public void CloseFile(FileSystemFileStream FileStream)
		{
			ImplCloseFile(FileStream);
		}

		public void SetFileTime(String Path, FileSystemEntry.FileTime FileTime)
		{
			FileSystem NewFileSystem; String NewPath; Access(Path, out NewFileSystem, out NewPath);
			NewFileSystem.ImplSetFileTime(NewPath, FileTime);
		}

		public FileSystemEntry GetFileInfo(String Path)
		{
			FileSystem NewFileSystem; String NewPath; Access(Path, out NewFileSystem, out NewPath);
			return NewFileSystem.ImplGetFileInfo(NewPath);
		}

		public FileSystemEntry.FileTime GetFileTime(String Path)
		{
			return this.GetFileInfo(Path).Time;
		}

		public void DeleteFile(String Path)
		{
			FileSystem NewFileSystem; String NewPath; Access(Path, out NewFileSystem, out NewPath);
			NewFileSystem.ImplDeleteFile(NewPath);
		}

		public void DeleteDirectory(String Path)
		{
			FileSystem NewFileSystem; String NewPath; Access(Path, out NewFileSystem, out NewPath);
			NewFileSystem.ImplDeleteDirectory(NewPath);
		}

		public void CreateDirectory(String Path, int Mode = 0777)
		{
			FileSystem NewFileSystem; String NewPath; Access(Path, out NewFileSystem, out NewPath);
			NewFileSystem.ImplCreateDirectory(NewPath, Mode);
		}


		public void MoveFile(String ExistingFileName, String NewFileName, bool ReplaceExisiting) {
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

		public IEnumerable<FileSystemEntry> FindFiles(String Path)
		{
			FileSystem NewFileSystem; String NewPath; Access(Path, out NewFileSystem, out NewPath);

			return NewFileSystem.FindMountedFiles(NewPath)
				.Concat(NewFileSystem.ImplFindFiles(NewPath))
				.DistinctByKey(FileSystemEntry => FileSystemEntry.Name)
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

		#endregion

		#region Implementations

		abstract internal void ImplSetFileTime(String Path, FileSystemEntry.FileTime FileTime);
		abstract internal FileSystemEntry ImplGetFileInfo(String Path);
		abstract internal void ImplDeleteFile(String Path);
		abstract internal void ImplDeleteDirectory(String Path);
		abstract internal void ImplMoveFile(String ExistingFileName, String NewFileName, bool ReplaceExisiting);
		abstract internal IEnumerable<FileSystemEntry> ImplFindFiles(String Path);
		abstract internal FileSystemFileStream ImplOpenFile(String FileName, FileMode FileMode);
		abstract internal void ImplWriteFile(FileSystemFileStream FileStream, byte[] Buffer, int Offset, int Count);
		abstract internal int ImplReadFile(FileSystemFileStream FileStream, byte[] Buffer, int Offset, int Count);
		abstract internal void ImplCloseFile(FileSystemFileStream FileStream);
		abstract internal void ImplCreateDirectory(String Path, int Mode = 0777);

		#endregion
	}

	static public class FileSystemUtils
	{
		static public bool ContainsFileName(this IEnumerable<FileSystemEntry> FileSystemEntry, String FileName)
		{
			foreach (var Item in FileSystemEntry)
			{
				if (Item.Name == FileName) return true;
			}
			return false;
		}

		static public bool ExistsFile(this FileSystem FileSystem, String Path)
		{
			try
			{
				FileSystem.GetFileInfo(Path);
				return true;
			}
			catch
			{
				return false;
			}
		}

		static public void CopyFile(this FileSystem FileSystem, String SourcePath, String DestinationPath)
		{
			using (var SourceStream = FileSystem.OpenFile(SourcePath, FileMode.Open))
			{
				using (var DestinationStream = FileSystem.OpenFile(DestinationPath, FileMode.Create))
				{
					SourceStream.CopyToFast(DestinationStream);
				}
			}
		}

		static public void WriteFile(this FileSystem FileSystem, String Path, byte[] Data)
		{
			using (var FileStream = FileSystem.OpenFile(Path, FileMode.Create))
			{
				FileStream.Write(Data, 0, Data.Length);
			}
		}

		static public byte[] ReadFile(this FileSystem FileSystem, String Path)
		{
			using (var FileStream = FileSystem.OpenFile(Path, FileMode.Open))
			{
				return FileStream.ReadAll();
			}
		}
	}
}
