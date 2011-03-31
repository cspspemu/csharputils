using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CSharpUtils.VirtualFileSystem
{
	public class FileSystemEntry
	{
		public struct FileTime
		{
			public DateTime CreationTime, LastAccessTime, LastWriteTime;
		}

		public FileSystem FileSystem;
		public String Path;
		public FileTime Time;

		public FileSystemEntry(FileSystem FileSystem, String Path)
		{
			this.FileSystem = FileSystem;
			this.Path = Path;
		}

		virtual public Stream Open(FileMode FileMode)
		{
			return FileSystem.OpenFile(Path, FileMode);
		}

		public override string ToString()
		{
			return "FileSystemEntry(Name=" + Path + ")";
		}
	}
}
