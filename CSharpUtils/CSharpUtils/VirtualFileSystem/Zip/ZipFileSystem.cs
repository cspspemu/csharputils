using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpUtils.VirtualFileSystem.Zip
{
	class ZipFileSystem : FileSystem
	{
		String ZipFilePath;

		public ZipFileSystem(String ZipFilePath)
		{
			this.ZipFilePath = ZipFilePath;
			throw(new NotImplementedException());
		}
	}
}
