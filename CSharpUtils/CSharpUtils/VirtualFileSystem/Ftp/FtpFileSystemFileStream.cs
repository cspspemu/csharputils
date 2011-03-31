using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpUtils.VirtualFileSystem.Local;
using System.IO;

namespace CSharpUtils.VirtualFileSystem.Ftp
{
	public class FtpFileSystemFileStream : FileSystemFileStreamStream
	{
		protected bool Modified = false;
		public String TempFileName;
		public String FileName;
		public FileMode FileMode;
		protected FtpFileSystem FtpFileSystem;

		public FtpFileSystemFileStream(FtpFileSystem FtpFileSystem, String FileName, FileMode FileMode)
			: base(FtpFileSystem, null)
		{
			this.FtpFileSystem = FtpFileSystem;
			this.FileName = FileName;
			this.FileMode = FileMode;
		}

		override public Stream Stream
		{
			get
			{
				if (_Stream == null)
				{
					TempFileName = FtpFileSystem.GetTempFile();
					switch (FileMode)
					{
						case FileMode.CreateNew:
							try
							{
								FtpFileSystem.GetFileTime(FileName);
								throw(new Exception("File '" + FileName + "' already exists can't open using FileMode.CreateNew"));
							}
							catch
							{
							}
							Modified = true;
						break;
						case FileMode.Create:
							Modified = true;
						break;
						case FileMode.Truncate:
							Modified = true;
						break;
						case FileMode.OpenOrCreate:
						case FileMode.Append:
						case FileMode.Open:
							FtpFileSystem.DownloadFile(FileName, TempFileName);
						break;
					}
					
					_Stream = new FileStream(TempFileName, FileMode);
				}
				return _Stream;
			}
		}

		public override void SetLength(long value)
		{
			Modified = true;
			base.SetLength(value);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			Modified = true;
			base.Write(buffer, offset, count);
		}

		public override void Close()
		{
			base.Close();
			// Has to reupload.
			if (_Stream != null)
			{
				if (Modified)
				{
					FtpFileSystem.UploadFile(FileName, TempFileName);
				}
				File.Delete(TempFileName);
			}
		}
	}
}
