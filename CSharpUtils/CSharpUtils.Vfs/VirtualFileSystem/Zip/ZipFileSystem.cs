using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Compression;

namespace CSharpUtils.VirtualFileSystem.Zip
{
	class ZipFileSystem : ImplFileSystem
	{
		String ZipFilePath;

		public ZipFileSystem(String ZipFilePath)
		{
			this.ZipFilePath = ZipFilePath;
			throw(new NotImplementedException());
		}
	}
}
