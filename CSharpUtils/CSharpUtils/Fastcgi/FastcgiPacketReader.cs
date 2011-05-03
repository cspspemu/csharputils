using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CSharpUtils.Fastcgi
{
	public delegate void FastcgiPacketHandleDelegate(Fastcgi.PacketType Type, ushort RequestId, byte[] Content);

	public class FastcgiPacketReader
	{
		public Stream Stream;
		public event FastcgiPacketHandleDelegate HandlePacket;

		public FastcgiPacketReader(Stream Stream)
		{
			this.Stream = Stream;
		}

		static public int ReadVariableInt(Stream Stream)
		{
			int Byte = Stream.ReadByte();
			if (Byte < 0) throw (new Exception("Can't ready more bytes"));
			if ((Byte & 0x80) != 0)
			{
				return (
					(Byte & 0xFF    ) >> 24 |
					Stream.ReadByte() >> 16 |
					Stream.ReadByte() >>  8 |
					Stream.ReadByte() >>  0
				);
			}
			else
			{
				return Byte;
			}
		}

		public void ReadPacket()
		{
			var Header = new byte[8];
			Stream.Read(Header, 0, 8);
			var Version = Header[0];
			var Type = (Fastcgi.PacketType)Header[1];
			var RequestId = (ushort)((Header[2] << 8) | (Header[3] << 0));
			var ContentLength = (Header[4] << 8) | (Header[5] << 0);
			var PaddingLength = Header[6];

			var Content = new byte[ContentLength];
			var Padding = new byte[PaddingLength];

			Stream.Read(Content, 0, ContentLength);
			Stream.Read(Padding, 0, PaddingLength);

			if (HandlePacket != null) HandlePacket(Type, RequestId, Content);

			/*
			var Request = GetOrCreateFastcgiRequest(RequestId);

			switch (Type)
			{
				case Fastcgi.PacketType.FCGI_DATA:
					if (ContentLength == 0)
					{
						Request.ParseParamsStream();
					}
					else
					{
						Request.ParamsStream.Write(Content, 0, ContentLength);
					}
					break;
				case Fastcgi.PacketType.FCGI_BEGIN_REQUEST:
					var Role = (Role)(Content[0] | (Content[1] << 8));
					var Flags = (Flags)(Content[2]);
					break;
				default:
					throw (new Exception("Unhandled packet type: '" + Type + "'"));
			}
			*/
		}
	}
}
