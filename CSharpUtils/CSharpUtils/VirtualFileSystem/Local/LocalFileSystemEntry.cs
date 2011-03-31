using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CSharpUtils.VirtualFileSystem.Local
{
	public class LocalFileSystemEntry : FileSystemEntry
	{
		protected FileSystemInfo FileSystemInfo;

		public LocalFileSystemEntry(FileSystem FileSystem, String Path, FileSystemInfo FileSystemInfo)
			: base(FileSystem, Path)
		{
			this.FileSystemInfo = FileSystemInfo;
		}
	}
}
