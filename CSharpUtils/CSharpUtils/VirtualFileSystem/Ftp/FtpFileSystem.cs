using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpUtils.Net;
using System.IO;

namespace CSharpUtils.VirtualFileSystem.Ftp
{
	public class FtpFileSystem : FileSystem
	{
		public FTP Ftp = new FTP();
		public String RootPath;

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

		public String GetTempFile()
		{
			return Path.GetTempPath() + Guid.NewGuid().ToString() + ".tmp";
		}

		public String DownloadFile(String RemoteFile, String LocalFile = null)
		{
			try
			{
				if (LocalFile == null) LocalFile = GetTempFile();
				Ftp.OpenDownload(RealPath(RemoteFile), LocalFile, false);
				while (Ftp.DoDownload() > 0) ;
				return LocalFile;
			}
			catch (Exception e)
			{
				throw(new Exception("Can't download ftp file '" + RemoteFile + "' : " + e.Message, e));
			}
		}

		public void UploadFile(String RemoteFile, String LocalFile)
		{
			try
			{
				Ftp.OpenUpload(LocalFile, RealPath(RemoteFile), false);
				while (Ftp.DoUpload() > 0) ;
			}
			catch (Exception e)
			{
				throw (new Exception("Can't upload ftp file '" + RemoteFile + "' : " + e.Message, e));
			}
		}

		override protected FileSystemEntry.FileTime ImplGetFileTime(String Path)
		{
			FileSystemEntry.FileTime Time = new FileSystemEntry.FileTime();
			Time.LastWriteTime = Ftp.GetFileDate(RealPath(Path));
			return Time;
		}

		protected override void ImplDeleteFile(string Path)
		{
			Ftp.RemoveFile(Path);
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
				FileSystemEntry.Size = FtpEntry.Size;
				FileSystemEntry.UserId = FtpEntry.UserId;
				FileSystemEntry.GroupId = FtpEntry.GroupId;
				switch (FtpEntry.Type) {
					case FTPEntry.FileType.Directory:
						FileSystemEntry.Type = FileSystemEntry.EntryType.Directory;
					break;
					case FTPEntry.FileType.Link:
						FileSystemEntry.Type = FileSystemEntry.EntryType.Link;
					break;
					default:
					case FTPEntry.FileType.File:
						FileSystemEntry.Type = FileSystemEntry.EntryType.File;
					break;
				}
				Entries.AddLast(FileSystemEntry);
			}
			return Entries;
		}

		override protected FileSystemFileStream ImplOpenFile(String FileName, FileMode FileMode)
		{
			return new FtpFileSystemFileStream(this, FileName, FileMode);
		}
	}
}
