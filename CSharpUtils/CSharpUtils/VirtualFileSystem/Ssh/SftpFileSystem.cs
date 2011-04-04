using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tamir.SharpSsh.jsch;
using System.Windows.Forms;
using CSharpUtils.VirtualFileSystem.Local;

namespace CSharpUtils.VirtualFileSystem.Ssh
{
	public class SftpFileSystem : RemoteFileSystem
	{
		JSch jsch;
		Session session;
		ChannelSftp csftp;
		String RootPath;

		override public void Connect(string Host, int Port, string Username, string Password, int timeout = 10000)
		{
			RootPath = "";

			jsch = new JSch();
			//session.setConfig();
			session = jsch.getSession(Username, Host, Port);
			UserInfo ui = new DirectPasswordUserInfo(Password);
			session.setUserInfo(ui);
			session.connect();

			csftp = (ChannelSftp)session.openChannel("sftp");
			csftp.connect();
		}

		protected String RealPath(String Path)
		{
			return RootPath + "/" + Path;
		}

		public override void Shutdown()
		{
			csftp.disconnect(); csftp = null;
			session.disconnect(); session = null;
			jsch = null;
		}

		override protected LinkedList<FileSystemEntry> ImplFindFiles(String Path)
		{
			var Items = new LinkedList<FileSystemEntry>();

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

			return Items;
		}

		protected override FileSystemFileStream ImplOpenFile(string FileName, System.IO.FileMode FileMode)
		{
			return new FileSystemFileStreamStream(this, csftp.get(FileName));
			//new FileSystemFileStreamStream();
			//return base.ImplOpenFile(FileName, FileMode);
		}

		//csftp.get(


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
