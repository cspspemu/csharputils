using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpUtils.VirtualFileSystem
{
	abstract public class FileSystem
	{
		virtual public FileSystemEntry Root
		{
			get
			{
				return new FileSystemEntry();
			}
		}
	}
}
