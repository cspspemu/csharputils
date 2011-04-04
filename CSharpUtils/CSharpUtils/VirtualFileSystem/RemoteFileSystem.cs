using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpUtils.VirtualFileSystem
{
	abstract public class RemoteFileSystem : FileSystem
	{
		abstract public void Connect(string Host, int Port, string Username, string Password, int timeout = 10000);
	}
}
