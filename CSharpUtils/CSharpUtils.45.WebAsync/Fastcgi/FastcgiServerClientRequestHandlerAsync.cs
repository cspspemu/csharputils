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
		public MemoryStream ParamsStream = new MemoryStream();
		public FastcgiRequestAsync FastcgiRequestAsync;
		internal FastcgiServerClientHandlerAsync FastcgiServerClientHandlerAsync;

		public FastcgiServerClientRequestHandlerAsync(FastcgiServerClientHandlerAsync FastcgiServerClientHandlerAsync, Stream ClientStream, ushort RequestId)
		{
			this.FastcgiServerClientHandlerAsync = FastcgiServerClientHandlerAsync;
			this.ClientStream = ClientStream;
			this.RequestId = RequestId;
			this.FastcgiRequestAsync = new FastcgiRequestAsync()
			{
				Stdin = new FastcgiInputStream(),
				Stdout = new FastcgiOutputStream(),
				Stderr = new FastcgiOutputStream(),
			};
		}

		static protected int ReadVariable(Stream Stream)
		{
			int Value = 0;
			byte Data;
			do
			{
				Data = (byte)Stream.ReadByte();
				Value <<= 7;
				Value |= Data & 0x7F;
			} while ((Data & 0x80) != 0);
			return Value;
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
						ParamsStream.Position = 0;
						FastcgiRequestAsync.Params = new Dictionary<string, string>();
						while (ParamsStream.Position < ParamsStream.Length)
						{
							int KeyLength = ReadVariable(ParamsStream);
							int ValueLength = ReadVariable(ParamsStream);
							var Key = ParamsStream.ReadString(KeyLength, Encoding.UTF8);
							var Value = ParamsStream.ReadString(ValueLength, Encoding.UTF8);
							FastcgiRequestAsync.Params[Key] = Value;
						}
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
						FastcgiRequestAsync.Stdin.Position = 0;
						Exception Exception = null;
						try
						{
							await FastcgiServerClientHandlerAsync.FastcgiServerAsync.HandleRequestAsync(this.FastcgiRequestAsync);
						}
						catch (Exception _Exception)
						{
							Exception = _Exception;
						}

						if (Exception != null)
						{
							var StreamWriter = new StreamWriter(FastcgiRequestAsync.Stderr);
							StreamWriter.WriteLine(String.Format("{0}", Exception));
							StreamWriter.Flush();
						}

						await FastcgiPacket.WriteMemoryStreamToAsync(RequestId: RequestId, PacketType: Fastcgi.PacketType.FCGI_STDOUT, From: FastcgiRequestAsync.Stdout, ClientStream: ClientStream);
						await FastcgiPacket.WriteMemoryStreamToAsync(RequestId: RequestId, PacketType: Fastcgi.PacketType.FCGI_STDERR, From: FastcgiRequestAsync.Stderr, ClientStream: ClientStream);

						await new FastcgiPacket() { Type = Fastcgi.PacketType.FCGI_STDOUT, RequestId = RequestId, Content = new ArraySegment<byte>() }.WriteToAsync(ClientStream);
						await new FastcgiPacket() { Type = Fastcgi.PacketType.FCGI_STDERR, RequestId = RequestId, Content = new ArraySegment<byte>() }.WriteToAsync(ClientStream);
						await new FastcgiPacket() { Type = Fastcgi.PacketType.FCGI_END_REQUEST, RequestId = RequestId, Content = new ArraySegment<byte>(new byte[] { 0, 0, 0, 0, (byte)Fastcgi.ProtocolStatus.FCGI_REQUEST_COMPLETE }) }.WriteToAsync(ClientStream);
						//await ClientStream.FlushAsync();
						ClientStream.Close();
					}
					else
					{
						await FastcgiRequestAsync.Stdin.WriteAsync(Content, 0, ContentLength);
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
