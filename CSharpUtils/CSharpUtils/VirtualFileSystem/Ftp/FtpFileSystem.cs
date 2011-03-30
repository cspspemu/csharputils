using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpUtils.VirtualFileSystem.Ftp
{
	class FtpFileSystem : RemoteFileSystem
	{
		public override void Connect(string Host, int Port, string Username, string Password)
		{
			throw new NotImplementedException();
		}
	}
}
