using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CSharpUtils.Fastcgi
{
	public class FastcgiPacketWriter
	{
		public Stream Stream;

		public FastcgiPacketWriter(Stream Stream)
		{
			this.Stream = Stream;
		}

		public void WritePacketParams()
		{
		}

		static public void WriteVariableInt(Stream Stream, int Value)
		{
			throw (new NotImplementedException());
		}

		public void WritePacketEndRequest(ushort RequestId, int AppStatus, Fastcgi.ProtocolStatus ProtocolStatus)
		{
			var Header = new byte[8] {
				(byte)((AppStatus >> 24) & 0xFF),
				(byte)((AppStatus >> 16) & 0xFF),
				(byte)((AppStatus >>  8) & 0xFF),
				(byte)((AppStatus >>  0) & 0xFF),
				(byte)(ProtocolStatus),
				0, 0, 0,
			};

			WritePacket(RequestId, Fastcgi.PacketType.FCGI_END_REQUEST, Header);
		}

		public void WritePacket(ushort RequestId, Fastcgi.PacketType Type, byte[] Contents)
		{
			int ContentsLength = Contents.Length;
			var Header = new byte[8] {
				1,
				(byte)Type,
				(byte)((RequestId      >> 8) & 0xFF),
				(byte)((RequestId      >> 0) & 0xFF),
				(byte)((ContentsLength >> 8) & 0xFF),
				(byte)((ContentsLength >> 0) & 0xFF),
				0,
				0
			};

			Stream.Write(Header, 0, 8);
			Stream.Write(Contents, 0, ContentsLength);
		}
	}
}
