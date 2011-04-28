using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpUtils.VirtualFileSystem
{
	public class ProxyFileSystem : FileSystem
	{
		protected FileSystem ParentFileSystem;

		public ProxyFileSystem(FileSystem ParentFileSystem)
		{
			this.ParentFileSystem = ParentFileSystem;
		}

		internal override void ImplSetFileTime(string Path, FileSystemEntry.FileTime FileTime)
		{
			this.ParentFileSystem.ImplSetFileTime(Path, FileTime);
		}

		internal override FileSystemEntry ImplGetFileInfo(string Path)
		{
			return this.ParentFileSystem.ImplGetFileInfo(Path);
		}

		internal override void ImplDeleteFile(string Path)
		{
			this.ParentFileSystem.ImplDeleteFile(Path);
		}

		internal override void ImplDeleteDirectory(string Path)
		{
			this.ParentFileSystem.ImplDeleteDirectory(Path);
		}

		internal override void ImplMoveFile(string ExistingFileName, string NewFileName, bool ReplaceExisiting)
		{
			this.ParentFileSystem.ImplMoveFile(ExistingFileName, NewFileName, ReplaceExisiting);
		}

		internal override IEnumerable<FileSystemEntry> ImplFindFiles(string Path)
		{
			return this.ParentFileSystem.ImplFindFiles(Path);
		}

		internal override FileSystemFileStream ImplOpenFile(string FileName, System.IO.FileMode FileMode)
		{
			return this.ParentFileSystem.ImplOpenFile(FileName, FileMode);
		}

		internal override void ImplWriteFile(FileSystemFileStream FileStream, byte[] Buffer, int Offset, int Count)
		{
			this.ParentFileSystem.ImplWriteFile(FileStream, Buffer, Offset, Count);
		}

		internal override int ImplReadFile(FileSystemFileStream FileStream, byte[] Buffer, int Offset, int Count)
		{
			return this.ParentFileSystem.ImplReadFile(FileStream, Buffer, Offset, Count);
		}

		internal override void ImplCloseFile(FileSystemFileStream FileStream)
		{
			this.ParentFileSystem.ImplCloseFile(FileStream);
		}

		internal override void ImplCreateDirectory(string Path, int Mode = 0777)
		{
			this.ParentFileSystem.ImplCreateDirectory(Path, Mode);
		}
	}
}
