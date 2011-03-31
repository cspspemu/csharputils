using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CSharpUtils.VirtualFileSystem.Local
{
	class LocalFileSystem : FileSystem
	{
		String RootPath;

		override protected bool CaseInsensitiveFileSystem { get { return true; } }

		public LocalFileSystem(String RootPath)
		{
			this.RootPath = AbsoluteNormalizePath(RootPath);
		}

		protected String RealPath(String Path)
		{
			return (RootPath + "/" + Path).Replace('/', '\\');
		}

		override protected FileSystemEntry.FileTime ImplGetFileTime(String Path)
		{
			String CachedRealPath = RealPath(Path);
			FileSystemEntry.FileTime FileTime;
			FileSystemInfo FileInfo = new FileInfo(CachedRealPath);
			if (!FileInfo.Exists)
			{
				if (!Directory.Exists(CachedRealPath))
				{
					throw (new FileNotFoundException("File '" + CachedRealPath + "' doesn't exists", Path));
				}
			}
			FileTime.CreationTime   = FileInfo.CreationTime;
			FileTime.LastAccessTime = FileInfo.LastAccessTime;
			FileTime.LastWriteTime  = FileInfo.LastWriteTime;
			return FileTime;
		}

		override protected LinkedList<FileSystemEntry> ImplFindFiles(String Path)
		{
			var Items = new LinkedList<FileSystemEntry>();
			String CachedRealPath = RealPath(Path);

			DirectoryInfo DirectoryInfo = new DirectoryInfo(CachedRealPath);

			foreach (var Item in DirectoryInfo.EnumerateFileSystemInfos())
			{
				if (!Item.FullName.StartsWith(CachedRealPath))
				{
					throw(new Exception("Unexpected FullName"));
				}

				if (Item.Attributes.HasFlag(FileAttributes.Hidden))
				{
					continue;
				}

				var FileSystemEntry = new LocalFileSystemEntry(this, Path + "/" + Item.Name, Item);
				if (Item.Attributes.HasFlag(FileAttributes.Directory))
				{
					FileSystemEntry.Type = VirtualFileSystem.FileSystemEntry.EntryType.Directory;
				}
				else
				{
					FileSystemEntry.Type = VirtualFileSystem.FileSystemEntry.EntryType.File;
				}
				
				//FileSystemEntry.Time = Item.
				Items.AddLast(FileSystemEntry);
				//yield return FileSystemEntry;
			}

			return Items;
		}

		override protected void ImplCreateDirectory(String Path, int Mode = 0777)
		{
			Directory.CreateDirectory(RealPath(Path));
		}

		override protected FileSystemFileStream ImplOpenFile(String FileName, FileMode FileMode)
		{
			var Stream = File.Open(RealPath(FileName), FileMode);
			return new FileSystemFileStreamStream(this, Stream);
		}

		/*
		protected override int ImplReadFile(FileSystemFileStream FileStream, byte[] Buffer, int Offset, int Count)
		{
			return ((FileSystemFileStreamStream)FileStream).Stream.Read(Buffer, Offset, Count);
		}

		protected override void ImplWriteFile(FileSystemFileStream FileStream, byte[] Buffer, int Offset, int Count)
		{
			((FileSystemFileStreamStream)FileStream).Stream.Write(Buffer, Offset, Count);
		}
		*/
		/*
		protected override void ImplCloseFile(FileSystemFileStream FileStream)
		{
			base.ImplCloseFile(FileStream);
		}
		 * */
	}
}
