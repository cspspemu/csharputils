using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CSharpUtils.VirtualFileSystem
{
	public class ImplFileSystem : FileSystem
	{
		protected override void ImplSetFileTime(string Path, FileSystemEntry.FileTime FileTime)
		{
			throw new NotImplementedException();
		}

		protected override FileSystemEntry ImplGetFileInfo(string Path)
		{
			throw new NotImplementedException();
		}

		protected override void ImplDeleteFile(string Path)
		{
			throw new NotImplementedException();
		}

		protected override void ImplDeleteDirectory(string Path)
		{
			throw new NotImplementedException();
		}

		protected override void ImplMoveFile(string ExistingFileName, string NewFileName, bool ReplaceExisiting)
		{
			throw new NotImplementedException();
		}

		protected override IEnumerable<FileSystemEntry> ImplFindFiles(string Path)
		{
			//throw new NotImplementedException();
			return new List<FileSystemEntry>();
			//yield return new FileSystemEntry();
		}

		protected override FileSystemFileStream ImplOpenFile(string FileName, FileMode FileMode)
		{
			throw new NotImplementedException();
		}

		protected override void ImplWriteFile(FileSystemFileStream FileStream, byte[] Buffer, int Offset, int Count)
		{
			throw new NotImplementedException();
		}

		protected override int ImplReadFile(FileSystemFileStream FileStream, byte[] Buffer, int Offset, int Count)
		{
			throw new NotImplementedException();
		}

		protected override void ImplCloseFile(FileSystemFileStream FileStream)
		{
			//throw new NotImplementedException();
		}

		protected override void ImplCreateDirectory(string Path, int Mode = 0777)
		{
			throw new NotImplementedException();
		}

		public override String Title
		{
			get
			{
				return this.ToString();
			}
		}
	}
}
