using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpUtils.VirtualFileSystem
{
	class FileSystemEntryNameComparer : IEqualityComparer<FileSystemEntry>
	{
		public bool Equals(FileSystemEntry x, FileSystemEntry y)
		{
			return x.Name == y.Name;
		}

		public int GetHashCode(FileSystemEntry obj)
		{
			return obj.GetHashCode();
		}
	}
}
