using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using CSharpUtils;
using CSharpUtils.Extensions;

namespace CSharpUtilsSandBox.Server
{
	abstract public class BaseClientHandler
	{
		public Socket ClientSocket;
		public NetworkStream ClientNetworkStream;
		public ProduceConsumeBuffer<byte> DataBuffer;
		protected List<ArraySegment<byte>> InternalSocketBuffers;

		public BaseClientHandler(Socket ClientSocket)
		{
			this.ClientSocket = ClientSocket;
			this.ClientNetworkStream = new NetworkStream(ClientSocket);
			this.InternalSocketBuffers = new List<ArraySegment<byte>>();
			this.InternalSocketBuffers.Add(new ArraySegment<byte>(new byte[1024]));
			this.DataBuffer = new ProduceConsumeBuffer<byte>();
		}

		public void StartReceivingData()
		{
			this.ClientSocket.BeginReceive(this.InternalSocketBuffers, SocketFlags.None, this.HandleDataReceived, null);
		}

		protected void HandleDataReceived(IAsyncResult AsyncResult)
		{
			try
			{
				int ReadedBytes = this.ClientSocket.EndReceive(AsyncResult);
				{
					DataBuffer.Produce(this.InternalSocketBuffers[0].Array, 0, ReadedBytes);
					TryHandlePacket(this.DataBuffer);
				}
				StartReceivingData();
			}
			catch (Exception Exception)
			{
				Console.WriteLine(Exception);
			}
		}

		abstract protected void HandlePacket(Packet ReceivedPacket);

		protected void TryHandlePacket(ProduceConsumeBuffer<byte> DataBuffer)
		{
			if (DataBuffer.ConsumeRemaining >= 3)
			{
				var PeekData = DataBuffer.ConsumePeek(3);
				var PacketSize = PeekData[0] | (PeekData[1] << 8);
				var PacketType = (Packet.PacketType)PeekData[2];
				var RealPacketSize = 2 + 1 + PacketSize;

				//Console.WriteLine("RealPacketSize={0}", RealPacketSize);

				if (DataBuffer.ConsumeRemaining >= RealPacketSize)
				{
					DataBuffer.Consume(3);
					var PacketData = DataBuffer.Consume(PacketSize);
					//Console.WriteLine(PacketData.ToHexString());
					HandlePacket(new Packet(PacketType, PacketData));
				}
			}
		}
	}
}
