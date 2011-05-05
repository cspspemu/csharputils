using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;

namespace CSharpUtils.Fastcgi
{
	public delegate bool FastcgiPacketHandleDelegate(Fastcgi.PacketType Type, ushort RequestId, byte[] Content);

	public class FastcgiPacketReader
	{
        public bool Debug = false;
        public IFastcgiPipe FastcgiPipe;
		public event FastcgiPacketHandleDelegate HandlePacket;
        static public byte[] Padding = new byte[8];
        public byte[] Header = new byte[8];

        public FastcgiPacketReader(IFastcgiPipe FastcgiPipe, bool Debug = false)
		{
            this.FastcgiPipe = FastcgiPipe;
            this.Debug = Debug;
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

        public void ReadAllPackets()
        {
            try
            {
                while (ReadPacket())
                {
                }
            }
            catch (Exception Exception)
            {
                if (Debug)
                {
                    Console.Error.WriteLine(Exception);
                }
            }
        }

		public bool ReadPacket()
		{
            int Readed = FastcgiPipe.Read(Header, 0, 8);
            if (Readed != 8)
            {
                Console.WriteLine("Header not completed");
                return false;
            }

            var Version = Header[0];
			var Type = (Fastcgi.PacketType)Header[1];
			var RequestId = (ushort)((Header[2] << 8) | (Header[3] << 0));
			var ContentLength = (Header[4] << 8) | (Header[5] << 0);
			var PaddingLength = Header[6];

            if (Version != 1)
            {
                Console.Error.WriteLine("Unknown Version " + Version);
                return false;
            }


			var Content = new byte[ContentLength];

            if (Debug)
            {
                Console.WriteLine("ReadPacket(Version={0}, Type={1}, RequestId={2}, ContentLength={3}, PaddingLength={4})", Version, Type, RequestId, ContentLength, PaddingLength);
            }

            if (ContentLength > 0)
            {
                Readed = FastcgiPipe.Read(Content, 0, ContentLength);
                if (Readed != ContentLength)
                {
                    Console.WriteLine("Content not completed");
                    return false;
                }
            }
            if (PaddingLength > 0)
            {
                Readed = FastcgiPipe.Read(Padding, 0, PaddingLength);
                if (Readed != PaddingLength)
                {
                    Console.WriteLine("Padding not completed");
                    return false;
                }
            }

            if (HandlePacket != null)
            {
                if (Debug)
                {
                    Console.WriteLine("Calling HandlePacket");
                }
                return HandlePacket(Type, RequestId, Content);
            }

            return true;
		}
	}
}
