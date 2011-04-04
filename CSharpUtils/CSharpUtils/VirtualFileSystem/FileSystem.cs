using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;

namespace CSharpUtils.VirtualFileSystem
{
	// http://msdn.microsoft.com/en-us/library/system.io.isolatedstorage.isolatedstoragefile.aspx
	// http://vfs.codeplex.com/
	public class FileSystem
	{
		protected SortedDictionary<String, FileSystem> MountedFileSystems = new SortedDictionary<string, FileSystem>();

		String CurrentWorkingPath = "";
		virtual protected bool CaseInsensitiveFileSystem { get { return false; } }

		~FileSystem()
		{
			Shutdown();
		}

		virtual public void Shutdown()
		{
		}

		static public String AbsoluteNormalizePath(String Path, String CurrentWorkingPath = "")
		{
			var Components = new LinkedList<String>();

			// Normalize slashes.
			Path = Path.Replace('\\', '/');

			// Relative Path
			if (Path.StartsWith("/"))
			{
				Path = CurrentWorkingPath + "/" + Path;
			}

			// Normalize Components
			foreach (var Component in Path.Split('/'))
			{
				switch (Component)
				{
					case "": case ".": break;
					case "..": Components.RemoveLast(); break;
					default: Components.AddLast(Component); break;
				}
			}

			return String.Join("/", Components);
		}

		private void Access(String Path, out FileSystem NewFileSystem, out String NewPath)
		{
			String ComparePath;

			// Normalize Components
			Path = AbsoluteNormalizePath(Path, CurrentWorkingPath);

			ComparePath = Path;
			if (CaseInsensitiveFileSystem)
			{
				ComparePath = ComparePath.ToLower();
			}

			// Check MountedFileSystems.
			foreach (var Item in MountedFileSystems)
			{
				String CheckMountedPath = Item.Key;

				if (CaseInsensitiveFileSystem)
				{
					CheckMountedPath = CheckMountedPath.ToLower();
				}

				if (
					ComparePath.StartsWith(CheckMountedPath) &&
					(
						(CheckMountedPath.Length == ComparePath.Length) ||
						(ComparePath.Substring(CheckMountedPath.Length, 1) == "/")
					)
				) {
					// Use Mounted File System.
					Item.Value.Access(ComparePath.Substring(CheckMountedPath.Length), out NewFileSystem, out NewPath);
					return;
				}
			}
			NewFileSystem = this;
			NewPath = Path;
		}

		public void Mount(String Path, FileSystem FileSystemToMount)
		{
			MountedFileSystems[AbsoluteNormalizePath(Path, CurrentWorkingPath)] = FileSystemToMount;
		}

		public void UnMount(String Path)
		{
			MountedFileSystems.Remove(Path);
		}

		#region Public Methods
		//public FileSystemFileStream CreateFile(String FileName, uint DesiredAccess, uint ShareMode, uint CreationDisposition, uint FlagsAndAttributes) {
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

		public FileSystemEntry.FileTime GetFileTime(String Path)
		{
			FileSystem NewFileSystem; String NewPath; Access(Path, out NewFileSystem, out NewPath);
			return NewFileSystem.ImplGetFileTime(NewPath);
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

		public void FindFilesAddMounted(String NewPath, LinkedList<FileSystemEntry> List)
		{
			foreach (var MountedFileSystemPath in MountedFileSystems.Keys)
			{
				if (MountedFileSystemPath.StartsWith(NewPath))
				{
					var Components = MountedFileSystemPath.Substring(NewPath.Length).TrimStart('/').Split('/');
					/*
					 * ::MountedFolder/sftp
					::MountedFolder
					::
					::System.String[]
					*/
					Console.WriteLine("::" + MountedFileSystemPath);
					Console.WriteLine("::" + NewPath);
					Console.WriteLine("::" + Components[0]);
					Console.WriteLine("::" + String.Join("##", Components));
					FileSystemEntry FileSystemEntry = new FileSystemEntry(this, Components[0]);
					FileSystemEntry.Type = VirtualFileSystem.FileSystemEntry.EntryType.Directory;
					List.AddLast(FileSystemEntry);
				}
			}
			/*
			foreach (var MountedFileSystemPath in MountedFileSystems.Keys)
			{
				String ParentName = "";
				String FileName = "";
				int index = MountedFileSystemPath.LastIndexOf("/");
				if (index == -1)
				{
					ParentName = "";
					FileName = MountedFileSystemPath;
				}
				else
				{
					ParentName = MountedFileSystemPath.Substring(0, index);
					FileName = MountedFileSystemPath.Substring(index + 1);
				}
				Console.WriteLine(":::(1)" + MountedFileSystemPath);
				Console.WriteLine(":::(2)" + NewPath);
				Console.WriteLine(":::(3)" + ParentName);
				Console.WriteLine(":::(3)" + FileName);
				if (NewPath == ParentName)
				{
					FileSystemEntry FileSystemEntry = new FileSystemEntry(this, FileName);
					FileSystemEntry.Type = VirtualFileSystem.FileSystemEntry.EntryType.Directory;
					List.AddLast(FileSystemEntry);
				}
				//MountedFileSystem.
			}
			*/
		}

		public LinkedList<FileSystemEntry> FindFiles(String Path)
		{
			FileSystem NewFileSystem; String NewPath; Access(Path, out NewFileSystem, out NewPath);
			LinkedList<FileSystemEntry> List = new LinkedList<FileSystemEntry>();
			try
			{
				NewFileSystem.FindFilesAddMounted(NewPath, List);
				NewFileSystem.ImplFindFiles(NewPath, List);
			}
			catch (Exception e)
			{
				if (List.Count == 0) throw (e);
			}
			return new LinkedList<FileSystemEntry>(List.Distinct(new FileSystemEntryNameComparer()));
		}
		
		#endregion

		#region Implementations

		virtual protected void ImplSetFileTime(String Path, FileSystemEntry.FileTime FileTime)
		{
			throw (new NotImplementedException());
		}

		virtual protected FileSystemEntry.FileTime ImplGetFileTime(String Path)
		{
			throw (new NotImplementedException());
		}

		virtual protected void ImplDeleteFile(String Path)
		{
			throw (new NotImplementedException());
		}

		virtual protected void ImplDeleteDirectory(String Path)
		{
			throw (new NotImplementedException());
		}

		virtual protected void ImplMoveFile(String ExistingFileName, String NewFileName, bool ReplaceExisiting)
		{
			throw (new NotImplementedException());
		}

		virtual protected void ImplFindFiles(String Path, LinkedList<FileSystemEntry> List)
		{
			throw (new NotImplementedException());
		}

		//virtual protected FileSystemFileStream ImplCreateFile(String FileName, uint DesiredAccess, uint ShareMode, uint CreationDisposition, uint FlagsAndAttributes)
		virtual protected FileSystemFileStream ImplOpenFile(String FileName, FileMode FileMode)
		{
			throw (new NotImplementedException());
		}

		virtual protected void ImplWriteFile(FileSystemFileStream FileStream, byte[] Buffer, int Offset, int Count)
		{
			throw (new NotImplementedException());
		}

		virtual protected int ImplReadFile(FileSystemFileStream FileStream, byte[] Buffer, int Offset, int Count)
		{
			throw (new NotImplementedException());
		}

		virtual protected void ImplCloseFile(FileSystemFileStream FileStream)
		{
			//throw (new NotImplementedException());
		}

		virtual protected void ImplCreateDirectory(String Path, int Mode = 0777)
		{
			throw (new NotImplementedException());
		}

		#endregion
	}
}
