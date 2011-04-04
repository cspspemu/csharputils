using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tamir.SharpSsh.jsch;
using System.Windows.Forms;
using CSharpUtils.VirtualFileSystem.Local;
using System.Threading;

namespace CSharpUtils.VirtualFileSystem.Ssh
{
	public class SftpFileSystem : RemoteFileSystem
	{
		JSch _jsch = null;
		Session _session = null;
		private ChannelSftp _csftp = null;
		String RootPath = "";

		public SftpFileSystem()
			:base()
		{
		}

		public SftpFileSystem(string Host, int Port, string Username, string Password, int timeout = 10000)
			:base (Host, Port, Username, Password, timeout)
		{
		}


		override public void Connect(string Host, int Port, string Username, string Password, int timeout = 10000)
		{
			_jsch = new JSch();
			//session.setConfig();
			_session = _jsch.getSession(Username, Host, Port);
			UserInfo ui = new DirectPasswordUserInfo(Password);
			_session.setUserInfo(ui);
			_session.connect();

			_csftp = (ChannelSftp)_session.openChannel("sftp");
			_csftp.connect();

			//RootPath = csftp.getHome();
			RootPath = "";
		}

		ChannelSftp csftp
		{
			get
			{
				// Try reconnect.
				if ((_csftp != null) && !_csftp.connected)
				{
					_csftp = null;
				}

				if (_csftp == null)
				{
					Connect(this.Host, this.Port, this.Username, this.Password, this.timeout);
				}
				return _csftp;
			}
		}



		override protected String RealPath(String Path)
		{
			return RootPath + "/" + Path;
		}

		public override void Shutdown()
		{
			if (_csftp != null)
			{
				_csftp.disconnect();
				_csftp = null;
			}

			if (_session != null)
			{
				_session.disconnect();
				_session = null;
			}

			if (_jsch != null)
			{
				_jsch = null;
			}
		}

		override protected FileSystemEntry.FileTime ImplGetFileTime(String Path)
		{
			FileSystemEntry.FileTime Time = new FileSystemEntry.FileTime();
			var stat = csftp.lstat(RealPath(Path));
			Time.LastAccessTime = stat.getATime();
			Time.CreationTime = stat.getMTime();
			Time.LastWriteTime = stat.getMTime();
			return Time;
		}

		override protected void ImplFindFiles(String Path, LinkedList<FileSystemEntry> Items)
		{
			foreach (var i in csftp.ls(RealPath(Path)))
			{
				var LsEntry = (Tamir.SharpSsh.jsch.ChannelSftp.LsEntry)i;
				var FileSystemEntry = new FileSystemEntry(this, Path + "/" + LsEntry.getFilename());
				FileSystemEntry.Size = LsEntry.getAttrs().getSize();
				FileSystemEntry.GroupId = LsEntry.getAttrs().getGId();
				FileSystemEntry.UserId = LsEntry.getAttrs().getUId();
				if (LsEntry.getAttrs().isDir()) {
					FileSystemEntry.Type = VirtualFileSystem.FileSystemEntry.EntryType.Directory;
				} else if (LsEntry.getAttrs().isLink()) {
					FileSystemEntry.Type = VirtualFileSystem.FileSystemEntry.EntryType.Link;
				} else {
					FileSystemEntry.Type = VirtualFileSystem.FileSystemEntry.EntryType.File;
				}
				Items.AddLast(FileSystemEntry);
			}
		}

		override public String DownloadFile(String RemoteFile, String LocalFile = null)
		{
			try
			{
				if (LocalFile == null) LocalFile = GetTempFile();
				csftp.get(RemoteFile, LocalFile);
				return LocalFile;
			}
			catch (Exception e)
			{
				throw (new Exception("Can't download sftp file '" + RemoteFile + "' : " + e.Message, e));
			}
		}

		override public void UploadFile(String RemoteFile, String LocalFile)
		{
			try
			{
				csftp.put(LocalFile, RemoteFile);
			}
			catch (Exception e)
			{
				throw (new Exception("Can't upload sftp file '" + RemoteFile + "' : " + e.Message, e));
			}
		}
	}

	class DirectPasswordUserInfo : UserInfo
	{
		String Password;

		public DirectPasswordUserInfo(String Password) { this.Password = Password; }
		public String getPassword() { return Password; }
		public bool promptYesNo(String str) { return true; }
		public String getPassphrase() { return null; }
		public bool promptPassphrase(String message) { return true; }
		public bool promptPassword(String message) { return true; }
		public void showMessage(String message) { MessageBox.Show(message, "SharpSSH", MessageBoxButtons.OK, MessageBoxIcon.Asterisk); }
	}

}
