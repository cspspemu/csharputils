using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CSharpUtils.Extensions;

namespace CSharpUtils.Web._45.Fastcgi
{
	public class FastcgiServerClientRequestHandlerAsync
	{
		protected Stream ClientStream;
		protected ushort RequestId;
		public Stream ParamsStream = new MemoryStream();
		public Stream StdinStream = new MemoryStream();
		public FastcgiOutputStream StdoutStream;
		public FastcgiOutputStream StderrStream;
		internal FastcgiServerClientHandlerAsync FastcgiServerClientHandlerAsync;

		public FastcgiServerClientRequestHandlerAsync(FastcgiServerClientHandlerAsync FastcgiServerClientHandlerAsync, Stream ClientStream, ushort RequestId)
		{
			this.FastcgiServerClientHandlerAsync = FastcgiServerClientHandlerAsync;
			this.ClientStream = ClientStream;
			this.RequestId = RequestId;
			this.StdoutStream = new FastcgiOutputStream(ClientStream, RequestId, Fastcgi.PacketType.FCGI_STDOUT);
			this.StderrStream = new FastcgiOutputStream(ClientStream, RequestId, Fastcgi.PacketType.FCGI_STDERR);
		}

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
				throw(new NotImplementedException());
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
				throw(new NotImplementedException());
			}

			async public override Task WriteAsync(byte[] buffer, int offset, int count, System.Threading.CancellationToken cancellationToken)
			{
				await MemoryStream.WriteAsync(buffer, offset, count, cancellationToken);
			}
		}

		async public Task HandlePacket(FastcgiPacket Packet)
		{
			if (FastcgiServerClientHandlerAsync.FastcgiServerAsync.Debug)
			{
				await Console.Out.WriteLineAsync(String.Format("HandlePacket"));
			}
			var Content = Packet.Content.Array;
			var ContentLength = Content.Length;

			switch (Packet.Type)
			{
				case Fastcgi.PacketType.FCGI_BEGIN_REQUEST:
					var Role = (Fastcgi.Role)(Content[0] | (Content[1] << 8));
					var Flags = (Fastcgi.Flags)(Content[2]);
					break;
				case Fastcgi.PacketType.FCGI_PARAMS:
					if (Content.Length == 0)
					{
						//Request.ParamsStream.Finalized = true;
					}
					else
					{
						ParamsStream.Write(Content, 0, ContentLength);
					}
					break;
				case Fastcgi.PacketType.FCGI_STDIN:
					if (Content.Length == 0)
					{
						await FastcgiServerClientHandlerAsync.FastcgiServerAsync.HandleRequest(this);
						await StdoutStream.FlushAsync();
						await StderrStream.FlushAsync();
						await new FastcgiPacket() { Type = Fastcgi.PacketType.FCGI_STDOUT, RequestId = RequestId, Content = new ArraySegment<byte>() }.WriteToAsync(ClientStream);
						await new FastcgiPacket() { Type = Fastcgi.PacketType.FCGI_STDERR, RequestId = RequestId, Content = new ArraySegment<byte>() }.WriteToAsync(ClientStream);
						await new FastcgiPacket() { Type = Fastcgi.PacketType.FCGI_END_REQUEST, RequestId = RequestId, Content = new ArraySegment<byte>(new byte[] { 0, 0, 0, 0, (byte)Fastcgi.ProtocolStatus.FCGI_REQUEST_COMPLETE }) }.WriteToAsync(ClientStream);
						await ClientStream.FlushAsync();
						ClientStream.Close();
					}
					else
					{
						StdinStream.Write(Content, 0, ContentLength);
					}
					break;
				default:
					Console.Error.WriteLine("Unhandled packet type: '" + Packet.Type + "'");
					//throw (new Exception("Unhandled packet type: '" + Type + "'"));
					break;
			}
		}
	}
}
