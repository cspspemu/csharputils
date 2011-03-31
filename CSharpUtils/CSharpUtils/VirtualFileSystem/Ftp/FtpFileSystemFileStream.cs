using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpUtils.VirtualFileSystem.Local;
using System.IO;

namespace CSharpUtils.VirtualFileSystem.Ftp
{
	public class FtpFileSystemFileStream : FileSystemFileStreamStream
	{
		public FtpFileSystemFileStream(FileSystem FileSystem, Stream Stream) : base(FileSystem, Stream)
		{
		}

	}
}
