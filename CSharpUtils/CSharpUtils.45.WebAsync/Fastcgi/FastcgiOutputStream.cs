using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpUtils.Web._45.Fastcgi
{
	public class FastcgiOutputStream : Stream
	{
		public MemoryStream MemoryStream = new MemoryStream();
		public Stream ClientStream;
		public ushort RequestId;
		public Fastcgi.PacketType PacketType;

		public FastcgiOutputStream(Stream ClientStream, ushort RequestId, Fastcgi.PacketType PacketType)
		{
			this.ClientStream = ClientStream;
			this.RequestId = RequestId;
			this.PacketType = PacketType;
		}

		public override bool CanRead
		{
			get { return false; }
		}

		public override bool CanSeek
		{
			get { return false; }
		}

		public override bool CanWrite
		{
			get { return true; }
		}

		public override void Flush()
		{
			throw (new NotImplementedException());
		}

		async public override Task FlushAsync(CancellationToken cancellationToken)
		{
			var Buffer = MemoryStream.GetBuffer();
			var BufferRealLength = (int)MemoryStream.Length;

			for (int n = 0; n < BufferRealLength; n += ushort.MaxValue)
			{
				await new FastcgiPacket()
				{
					RequestId = RequestId,
					Type = PacketType,
					Content = new ArraySegment<byte>(Buffer, n, Math.Min(ushort.MaxValue, BufferRealLength - n)),
				}.WriteToAsync(ClientStream);
			}

			MemoryStream.SetLength(0);
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

		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			//MemoryStream.Write(buffer, offset, count);
			throw (new NotImplementedException());
		}

		async public override Task WriteAsync(byte[] buffer, int offset, int count, System.Threading.CancellationToken cancellationToken)
		{
			await MemoryStream.WriteAsync(buffer, offset, count, cancellationToken);
		}
	}

}
