using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpUtils.VirtualFileSystem
{
	abstract class RemoteFileSystem : FileSystem
	{
		abstract public void Connect(String Host, int Port, String Username, String Password);
	}
}
