using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CSharpUtils.VirtualFileSystem.Local
{
	public class LocalFileSystem : FileSystem, IDisposable
	{
		protected String RootPath;

		override protected bool CaseInsensitiveFileSystem { get { return true; } }

		public LocalFileSystem(String RootPath)
		{
			this.RootPath = AbsoluteNormalizePath(RootPath);
		}

		protected String RealPath(String Path)
		{
			return (RootPath + "/" + Path).Replace('/', '\\');
		}

		override protected FileSystemEntry ImplGetFileInfo(String Path)
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

		override protected void ImplFindFiles(String Path, LinkedList<FileSystemEntry> Items)
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
				Items.AddLast(FileSystemEntry);
				//yield return FileSystemEntry;
			}
		}

		override protected void ImplCreateDirectory(String Path, int Mode = 0777)
		{
			Directory.CreateDirectory(RealPath(Path));
		}

		protected override void ImplDeleteFile(string Path)
		{
			File.Delete(RealPath(Path));
		}


		override protected FileSystemFileStream ImplOpenFile(String FileName, FileMode FileMode)
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

		public void Dispose()
		{
			Shutdown();
		}
	}
}
