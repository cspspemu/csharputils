using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CSharpUtils.VirtualFileSystem.Memory
{
	public class MemoryFileSystem : NodeFileSystem
	{
		public void AddFile(String AddFileName, Stream Contents)
		{
			AddFileName = AbsoluteNormalizePath(AddFileName);
			var Node = RootNode.Access(AddFileName, true);
			Node.Time = new FileSystemEntry.FileTime()
			{
				CreationTime = DateTime.Now,
				LastAccessTime = DateTime.Now,
				LastWriteTime = DateTime.Now,
			};
			Node.FileSystemFileStream = new FileSystemFileStreamStream(this, Contents);
		}

		public override String Title
		{
			get
			{
				return String.Format("memory://");
			}
		}
	}
}
