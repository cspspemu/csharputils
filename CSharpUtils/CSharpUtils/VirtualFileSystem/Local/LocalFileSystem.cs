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
		FileSystemEntry _Root;

		public LocalFileSystem(String RootPath)
		{
			this.RootPath = RootPath;
			this._Root = new LocalFileSystemEntry(this, new DirectoryInfo(RootPath), null);
		}

		override public FileSystemEntry Root {
			get
			{
				return _Root;
			}
		}
	}
}
