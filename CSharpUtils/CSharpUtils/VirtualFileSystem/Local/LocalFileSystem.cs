using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CSharpUtils.VirtualFileSystem.Local
{
	public class LocalFileSystem : ImplFileSystem
	{
		protected String RootPath;

		override protected bool CaseInsensitiveFileSystem { get { return true; } }

		public LocalFileSystem(String RootPath, bool CreatePath = false)
		{
			if (CreatePath && !Directory.Exists(RootPath))
			{
				Directory.CreateDirectory(RootPath);
			}
			this.RootPath = AbsoluteNormalizePath(RootPath);
		}

		protected String RealPath(String Path)
		{
			if (Environment.OSVersion.Platform == PlatformID.Unix)
			{
				return CombinePath(RootPath, Path);
			}
			return CombinePath(RootPath, Path).Replace('/', '\\');
		}

		override internal FileSystemEntry ImplGetFileInfo(String Path)
		{
			String CachedRealPath = RealPath(Path);
			FileSystemInfo FileSystemInfo;
			if (Directory.Exists(CachedRealPath)) {
				FileSystemInfo = new DirectoryInfo(CachedRealPath);
			} else {
				FileSystemInfo = new FileInfo(CachedRealPath);
			}
			
			return new LocalFileSystemEntry(this, Path, FileSystemInfo);
		}

		override internal IEnumerable<FileSystemEntry> ImplFindFiles(String Path)
		{
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


				//FileSystemEntry.Size = File.get
				//Item.Attributes == FileAttributes.
				//
				
				//FileSystemEntry.Time = Item.
				yield return FileSystemEntry;
			}
		}

		override internal void ImplCreateDirectory(String Path, int Mode = 0777)
		{
			Directory.CreateDirectory(RealPath(Path));
		}

		override internal void ImplDeleteFile(string Path)
		{
			File.Delete(RealPath(Path));
		}


		override internal FileSystemFileStream ImplOpenFile(String FileName, FileMode FileMode)
		{
			//var Stream = File.Open(RealPath(FileName), FileMode, FileAccess.Read, FileShare.ReadWrite);
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

		public override String Title
		{
			get
			{
				//return "local://" + RootPath;
				return RootPath;
			}
		}
	}
}
