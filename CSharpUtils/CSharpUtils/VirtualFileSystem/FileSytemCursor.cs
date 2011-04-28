using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpUtils.VirtualFileSystem
{
	public class FileSytemCursor
	{
		public FileSystem FileSystem;
		public String CurrentPath;

		public FileSytemCursor(FileSystem FileSystem, String CurrentPath = "/")
		{
			this.FileSystem = FileSystem;
			this.CurrentPath = CurrentPath;
		}

		public void Access(String Path)
		{
		}
	}
}
