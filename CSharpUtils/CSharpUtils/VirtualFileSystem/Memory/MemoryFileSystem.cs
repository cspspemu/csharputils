using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CSharpUtils.VirtualFileSystem.Memory
{
	class MemoryFileSystem : ImplFileSystem
	{
		//Dictionary<String, FileSystemFileStream> Files;

		public MemoryFileSystem()
		{
			throw(new NotImplementedException());
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
