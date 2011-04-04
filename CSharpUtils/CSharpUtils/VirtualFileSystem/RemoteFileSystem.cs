using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CSharpUtils.VirtualFileSystem
{
	abstract public class RemoteFileSystem : FileSystem
	{
		public String GetTempFile()
		{
			return Path.GetTempPath() + Guid.NewGuid().ToString() + ".tmp";
		}

		virtual protected String RealPath(String Path)
		{
			return Path;
		}

		virtual public String DownloadFile(String RemoteFile, String LocalFile = null)
		{
			throw(new NotImplementedException());
		}

		virtual public void UploadFile(String RemoteFile, String LocalFile)
		{
			throw(new NotImplementedException());
		}

		abstract public void Connect(string Host, int Port, string Username, string Password, int timeout = 10000);

		protected override FileSystemFileStream ImplOpenFile(string FileName, System.IO.FileMode FileMode)
		{
			return new RemoteFileSystemFileStream(this, RealPath(FileName), FileMode);
		}
	}
}
