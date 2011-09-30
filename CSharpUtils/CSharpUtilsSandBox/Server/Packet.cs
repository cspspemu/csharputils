using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CSharpUtils.Extensions;

namespace CSharpUtilsSandBox.Server
{
	public class Packet
	{
		public enum PacketType : byte
		{
			////////////////////////////////
			/// Misc ///////////////////////
			////////////////////////////////
			Ping = 0x00,
			GetVersion = 0x01,
			////////////////////////////////
			/// Rankings ///////////////////
			////////////////////////////////
			SetRanking = 0x10,
			GetRankingInfo = 0x11,
			////////////////////////////////
			/// Elements ///////////////////
			////////////////////////////////
			SetElements = 0x20,
			GetElementOffset = 0x21,
			ListElements = 0x22,
			RemoveElements = 0x23,
			RemoveAllElements = 0x24,
			////////////////////////////////
		}

		public PacketType Type;
		public MemoryStream Stream;
		public BinaryWriter BinaryWriter;

		public Packet(PacketType Type, byte[] Data)
		{
			this.Type = Type;
			this.Stream = new MemoryStream(Data);
			this.BinaryWriter = new BinaryWriter(this.Stream);
		}

		public Packet(PacketType Type)
		{
			this.Type = Type;
			this.Stream = new MemoryStream();
			this.BinaryWriter = new BinaryWriter(this.Stream);
		}

		public void WritePacketTo(Stream OutputStream)
		{
			var PacketBytes = this.Stream.ReadAll(true);
			var BinaryWriter = new BinaryWriter(OutputStream);
			BinaryWriter.Write((ushort)(PacketBytes.Length));
			BinaryWriter.Write((byte)Type);
			BinaryWriter.Write(PacketBytes);
		}

		public override string ToString()
		{
			return String.Format("Packet(Type={0}, Data={1})", Type, this.Stream.ToArray().ToHexString());
		}
	}
}
