using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;

namespace CSharpUtils.Fastcgi
{
	public class FastcgiPacketWriter
	{
        public bool Debug = false;
        public Socket Socket;
		public Stream Stream;
        static byte[] Padding = new byte[8];

		public FastcgiPacketWriter(Stream Stream, bool Debug = false)
		{
			this.Stream = Stream;
            this.Debug = Debug;
		}

		public void WritePacketParams()
		{
		}

		static public void WriteVariableInt(Stream Stream, int Value)
		{
			throw (new NotImplementedException());
		}

		public bool WritePacketEndRequest(ushort RequestId, int AppStatus, Fastcgi.ProtocolStatus ProtocolStatus)
		{
			var Header = new byte[8] {
				(byte)((AppStatus >> 24) & 0xFF),
				(byte)((AppStatus >> 16) & 0xFF),
				(byte)((AppStatus >>  8) & 0xFF),
				(byte)((AppStatus >>  0) & 0xFF),
				(byte)(ProtocolStatus),
				0, 0, 0,
                //0x74, 0x20, 0x61,
			};

            if (Debug)
            {
                Console.WriteLine("WritePacketEndRequest(" + Header.Implode(",") + ")");
            }

			return WritePacket(RequestId, Fastcgi.PacketType.FCGI_END_REQUEST, Header);
		}

		public bool WritePacket(ushort RequestId, Fastcgi.PacketType Type, byte[] Contents)
		{
			int ContentsLength = Contents.Length;

            int PaddingLength = (8 - Contents.Length & 7) & 7;

            if (Debug)
            {
                Console.WriteLine("WritePacket(RequestId=" + RequestId + ", Type=" + Type + ", Contents=" + Contents.Length + ", Padding=" + PaddingLength + ")");
                if (Type == Fastcgi.PacketType.FCGI_STDOUT)
                {
                    Console.Write(Encoding.UTF8.GetString(Contents));
                }
            }

			var Header = new byte[8] {
				1,
				(byte)Type,
				(byte)((RequestId      >> 8) & 0xFF),
				(byte)((RequestId      >> 0) & 0xFF),
				(byte)((ContentsLength >> 8) & 0xFF),
				(byte)((ContentsLength >> 0) & 0xFF),
				(byte)PaddingLength,
				0
			};

            try
            {
                Stream.Write(Header, 0, 8);
                if (ContentsLength > 0)
                {
                    Stream.Write(Contents, 0, ContentsLength);
                }
                if (PaddingLength > 0)
                {
                    Stream.Write(Padding, 0, PaddingLength);
                }
                return true;
            }
            catch (IOException IOException)
            {
                //Console.WriteLine("WritePacket(RequestId=" + RequestId + ", Type=" + Type + ", Contents=" + Contents.Length + ", Padding=" + PaddingLength + ")");
                //Console.Error.WriteLine(IOException.Message);
                return false;
            }
		}
	}
}
