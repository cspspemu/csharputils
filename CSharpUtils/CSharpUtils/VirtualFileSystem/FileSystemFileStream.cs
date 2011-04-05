using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CSharpUtils.VirtualFileSystem
{
	public class FileSystemFileStream : Stream
	{
		FileSystem FileSystem;
		FileSystemEntry FileSystemEntry;

		public FileSystemFileStream(FileSystem FileSystem, FileSystemEntry FileSystemEntry = null)
		{
			this.FileSystem = FileSystem;
			this.FileSystemEntry = FileSystemEntry;
		}

		public override bool CanRead
		{
			get { throw new NotImplementedException(); }
		}

		public override bool CanSeek
		{
			get { throw new NotImplementedException(); }
		}

		public override bool CanWrite
		{
			get { throw new NotImplementedException(); }
		}

		public override void Flush()
		{
			throw new NotImplementedException();
		}

		public override long Length
		{
			get { throw new NotImplementedException(); }
		}

		public override long Position
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			return this.FileSystem.ReadFile(this, buffer, offset, count);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			this.FileSystem.WriteFile(this, buffer, offset, count);
		}

		public override void Close()
		{
			this.FileSystem.CloseFile(this);
			base.Close();
		}
	}
}
