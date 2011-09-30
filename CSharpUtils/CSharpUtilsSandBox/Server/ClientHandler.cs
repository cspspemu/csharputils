using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using CSharpUtils.Extensions;
using System.Runtime.InteropServices;

namespace CSharpUtilsSandBox.Server
{
	public class ClientHandler : BaseClientHandler
	{
		interface IPacketHandler
		{
			void HandlePacket(Packet ReceivedPacket, Packet PacketToSend);
		}

		class GetVersionHandler : IPacketHandler {
			public void HandlePacket(Packet ReceivedPacket, Packet PacketToSend)
			{
				PacketToSend.BinaryWriter.Write((uint)1);
			}
		}

		class GetRankingInfoHandler : IPacketHandler {
			struct RequestStruct
			{
				//[MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 64)]
				//internal string RankingName;
				uint RankingIndex;
			}

			struct ResponseStruct
			{
				internal uint Result;
				//internal uint Index;
				internal uint Length;
				internal ServerIndices.SortingDirection Direction;
				internal uint TopScore;
				internal uint BottomScore;
				internal int MaxElements;
				internal uint TreeHeight;
			}

			public void HandlePacket(Packet ReceivedPacket, Packet PacketToSend)
			{
				//Console.WriteLine(ReceivedPacket);
				var Request = ReceivedPacket.Stream.ReadStruct<RequestStruct>();
				var Response = new ResponseStruct();
				//Console.WriteLine("'" + Request.RankingName + "'");
				Response.Result = 0;
				PacketToSend.Stream.WriteStruct(Response);
			}
		}

		public ClientHandler(Socket ClientSocket)
			: base(ClientSocket)
		{
		}

		override protected void HandlePacket(Packet ReceivedPacket)
		{
			var PacketToSend = new Packet(ReceivedPacket.Type);
			switch (ReceivedPacket.Type)
			{
				case Packet.PacketType.GetVersion:
					new GetVersionHandler().HandlePacket(ReceivedPacket, PacketToSend);
					break;
				case Packet.PacketType.GetRankingInfo:
					new GetRankingInfoHandler().HandlePacket(ReceivedPacket, PacketToSend);
					break;
				default:
					throw (new NotImplementedException("Can't handle packet '" + ReceivedPacket + "'"));
			}
			PacketToSend.WritePacketTo(this.ClientNetworkStream);
		}
	}
}
