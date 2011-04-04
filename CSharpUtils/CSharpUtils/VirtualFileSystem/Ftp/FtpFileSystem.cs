using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpUtils.Net;
using System.IO;

namespace CSharpUtils.VirtualFileSystem.Ftp
{
	/**
	 * http://www.networksorcery.com/enp/protocol/ftp.htm
	 */
	public class FtpFileSystem : RemoteFileSystem
	{
		public FTP Ftp = new FTP();
		public String RootPath;

		override public void Connect(string Host, int Port, string Username, string Password, int timeout = 10000)
		{
			Ftp.Connect(Host, Port, Username, Password);
			Ftp.timeout = timeout;
			RootPath = Ftp.GetWorkingDirectory();
		}

		override protected String RealPath(String Path)
		{
			return RootPath + "/" + Path;
		}

		override public void Shutdown()
		{
			Ftp.Disconnect();
		}

		override public String DownloadFile(String RemoteFile, String LocalFile = null)
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

		override public void UploadFile(String RemoteFile, String LocalFile)
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

		protected override void ImplFindFiles(string Path, LinkedList<FileSystemEntry> Entries)
		{
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
		}

		override protected void ImplCreateDirectory(String Path, int Mode = 0777)
		{
			Ftp.MakeDir(Path);
			//Directory.CreateDirectory(Path);
		}
	}
}
