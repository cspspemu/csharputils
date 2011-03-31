using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpUtils.Net;

namespace CSharpUtils.VirtualFileSystem.Ftp
{
	class FtpFileSystem : FileSystem
	{
		FTP Ftp = new FTP();
		String RootPath;

		public void Connect(string Host, int Port, string Username, string Password)
		{
			Ftp.Connect(Host, Port, Username, Password);
			RootPath = Ftp.GetWorkingDirectory();
		}

		protected String RealPath(String Path)
		{
			return RootPath + "/" + Path;
		}

		override public void Shutdown()
		{
			Ftp.Disconnect();
		}

		override protected FileSystemEntry.FileTime ImplGetFileTime(String Path)
		{
			FileSystemEntry.FileTime Time = new FileSystemEntry.FileTime();
			Time.LastWriteTime = Ftp.GetFileDate(RealPath(Path));
			return Time;
		}

		protected override LinkedList<FileSystemEntry> ImplFindFiles(string Path)
		{
			var Entries = new LinkedList<FileSystemEntry>();
			Ftp.ChangeDir(RealPath(Path));
			foreach (var FtpEntry in Ftp.ListEntries())
			{
				var FileSystemEntry = new FileSystemEntry(this, Path + "/" + FtpEntry.Name);
				//FtpEntry.
				FileSystemEntry.Time.LastWriteTime = FtpEntry.ModifiedTime;
				Entries.AddLast(FileSystemEntry);
			}
			return Entries;
		}
	}
}
