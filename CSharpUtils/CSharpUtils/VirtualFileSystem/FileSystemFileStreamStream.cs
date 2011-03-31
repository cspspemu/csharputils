using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CSharpUtils.VirtualFileSystem.Local
{
	public class FileSystemFileStreamStream : FileSystemFileStream
	{
		public Stream Stream;
		private string FileName;
		private FileStream Stream_2;

		public FileSystemFileStreamStream(FileSystem FileSystem, Stream Stream)
			: base(FileSystem)
		{
			this.Stream = Stream;
		}

		public override bool CanRead
		{
			get { return this.Stream.CanRead; }
		}

		public override bool CanSeek
		{
			get { return this.Stream.CanSeek; }
		}

		public override bool CanWrite
		{
			get { return this.Stream.CanWrite; }
		}

		public override void Flush()
		{
			this.Stream.Flush();
		}

		public override long Length
		{
			get { return this.Stream.Length; }
		}

		public override long Position
		{
			get
			{
				return this.Stream.Position;
			}
			set
			{
				this.Stream.Position = value;
			}
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			return this.Stream.Seek(offset, origin);
		}

		public override void SetLength(long value)
		{
			this.Stream.SetLength(value);
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			return this.Stream.Read(buffer, offset, count);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			this.Stream.Write(buffer, offset, count);
		}

		public override void Close()
		{
			Stream.Close();
		}
	}
}
