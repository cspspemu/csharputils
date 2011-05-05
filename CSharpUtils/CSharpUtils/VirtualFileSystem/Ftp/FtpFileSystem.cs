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
		public FTP _Ftp = null;
		public String RootPath;

		public FtpFileSystem()
			:base()
		{
		}

		public FTP Ftp
		{
			get
			{
				// Reconnect if disconnected.
				if ((_Ftp != null) && !(_Ftp.IsConnected))
				{
					_Ftp = null;
				}

				if (_Ftp == null)
				{
					_Ftp = new FTP();
					_Connect();
				}
				return _Ftp;
			}
		}

		override public RemoteFileSystem EnsureConnect()
		{
			var Ftp = this.Ftp;
			return this;
		}

		public FtpFileSystem(string Host, int Port, string Username, string Password, int timeout = 10000)
			:base (Host, Port, Username, Password, timeout)
		{
		}

		override public void Connect(string Host, int Port, string Username, string Password, int timeout = 10000)
		{
			this.Host = Host;
			this.Port = Port;
			this.Username = Username;
			this.Password = Password;
			this.timeout = timeout;
			_Ftp = null;
		}

		protected void _Connect()
		{
			_Ftp.Connect(this.Host, this.Port, this.Username, this.Password);
			_Ftp.timeout = this.timeout;
			RootPath = _Ftp.GetWorkingDirectory();
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
				Ftp.OpenDownload(RemoteFile, LocalFile, false);
				while (Ftp.DoDownload() > 0) ;
				return LocalFile;
			}
			catch (Exception e)
			{
				throw (new Exception("Can't download ftp file '" + RemoteFile + "' to '" + LocalFile + "' : " + e.Message, e));
			}
		}

		override public void UploadFile(String RemoteFile, String LocalFile)
		{
			try
			{
				Ftp.OpenUpload(LocalFile, RemoteFile, false);
				while (Ftp.DoUpload() > 0) ;
			}
			catch (Exception e)
			{
				throw (new Exception("Can't upload ftp file '" + RemoteFile + "' : " + e.Message, e));
			}
		}

		override internal FileSystemEntry ImplGetFileInfo(String Path)
		{
			String CachedRealPath = RealPath(Path);
			var Info = new FileSystemEntry(this, Path);
			Info.Size = Ftp.GetFileSize(CachedRealPath);
			Info.Time.LastWriteTime = Ftp.GetFileDate(CachedRealPath);
			return Info;
		}

		override internal void ImplDeleteFile(string Path)
		{
			String CachedRealPath = RealPath(Path);
			Ftp.RemoveFile(CachedRealPath);
		}

		override internal IEnumerable<FileSystemEntry> ImplFindFiles(string Path)
		{
			Ftp.ChangeDir(RealPath(Path));
			var Entries = Ftp.ListEntries();
			Ftp.ChangeDir(RootPath);
			foreach (var FTPEntry in Entries)
			{
				var FileSystemEntry = new FtpFileSystemEntry(this, Path + "/" + FTPEntry.Name, FTPEntry);

				yield return FileSystemEntry;
			}
		}

		override internal void ImplCreateDirectory(String Path, int Mode = 0777)
		{
			Ftp.MakeDir(Path);
			//Directory.CreateDirectory(Path);
		}

		public override String Title
		{
			get
			{
				return String.Format("ftp://{0}@{1}/{2}", Username, Host, RootPath);
			}
		}

	}
}
