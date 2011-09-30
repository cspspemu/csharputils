using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpUtils.Containers.RedBlackTree;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using CSharpUtils;
using CSharpUtils.Extensions;
using System.IO;
using CSharpUtils.Streams;
using CSharpUtilsSandBox.Server;

namespace CSharpUtilsSandBox
{
	class Program
	{
		

		static void Test1()
		{
			Console.WriteLine("-----------------------------------------");
			var Stats = new RedBlackTreeWithStats<int>();
			{
				var Start = DateTime.Now;
				for (int n = 0; n < 500000; n++)
				{
					Stats.Add(n); // 1
				}
				Console.WriteLine("Time: " + (DateTime.Now - Start).TotalMilliseconds);
			}
			//Stats.PrintTree();
			{
				var Start = DateTime.Now;
				int Value = 0;
				for (int n = 0; n < 100; n++)
				{
					Value = Stats.All.Count;
					Value = Stats.All.Skip(250000).Take(240000).Count;
					//Value = Stats.All.Slice(250000, 250000 + 240000).Count;
				}
				Console.WriteLine(Value);
				Console.WriteLine("Time: " + (DateTime.Now - Start).TotalMilliseconds);
			}
			{
				var Start = DateTime.Now;
				int Value = 0;
				for (int n = 0; n < 1000; n++)
				{
					Value = Stats.All.GetItemPosition(250000);
					//Value = Stats.Where(Item => Item < 250000).Count();
				}
				Console.WriteLine(Value);
				Console.WriteLine("Time: " + (DateTime.Now - Start).TotalMilliseconds);
			}
			/*
			{
				var Start = DateTime.Now;
				int Value = 0;
				for (int n = 0; n < 100; n++)
				{
					//int Value = Stats.All.Count();
					Value = Stats.Count();
					Value = Stats.Skip(50000).Count();
				}
				Console.WriteLine(Value);
				Console.WriteLine(DateTime.Now - Start);
			}
			 * */
			/*
			foreach (var Item in Stats.All.Where(Item => Item > 3).Count())
			{
				Console.WriteLine(Item);
			}
			*/
			//Stats.DebugValidateTree();
		}

		static void Test2()
		{
			Console.WriteLine("-----------------------------------------");
			var Stats = new Dictionary<int, int>();
			//var Stats = new SortedList<int, int>();
			{
				var Start = DateTime.Now;
				for (int n = 0; n < 500000; n++)
				{
					Stats.Add(500000 - n, n); // 1
				}
				Console.WriteLine("Time: " + (DateTime.Now - Start).TotalMilliseconds);
			}
			//Stats.PrintTree();
			{
				var Start = DateTime.Now;
				int Value = 0;
				for (int n = 0; n < 100; n++)
				{
					Value = Stats.Count;
					Value = Stats.Skip(250000).Take(240000).Count();
				}
				Console.WriteLine(Value);
				Console.WriteLine("Time: " + (DateTime.Now - Start).TotalMilliseconds);
			}
			{
				var Start = DateTime.Now;
				int Value = 0;
				for (int n = 0; n < 1000; n++)
				{
					//Value = Stats.IndexOfValue(250000);
				}
				Console.WriteLine(Value);
				Console.WriteLine("Time: " + (DateTime.Now - Start).TotalMilliseconds);
			}
			/*
			foreach (var Item in Stats.All.Where(Item => Item > 3).Count())
			{
				Console.WriteLine(Item);
			}
			*/
			//Stats.DebugValidateTree();
		}

		static void Test3()
		{
			var Stats = (new RedBlackTreeWithStats<int>()).Clone();
			Stats.Add(1);
			Stats.Add(2);
			Stats.Add(3);
			Console.WriteLine(
				Stats
					.Skip(1)
					.Where(Item => Item >= 3)
					.Count()
			);
		}

		static void Test4()
		{
			ManualResetEvent ClientConnected = new ManualResetEvent(false);
			
			var ListenIp = "127.0.0.1";
			var ListenPort = 9777;

			var TcpListener = new TcpListener(IPAddress.Parse(ListenIp), ListenPort);
			TcpListener.Start();

			Console.WriteLine("Listening {0}:{1}...", ListenIp, ListenPort);

			while (true)
			{
				Console.WriteLine("Waiting for a connection...");
				ClientConnected.Reset();
				TcpListener.BeginAcceptSocket((AcceptState) =>
				{
					Console.WriteLine("  BeginAcceptSocket");
					var Datas = new List<ArraySegment<byte>>();
					Datas.Add(new ArraySegment<byte>(new byte[1024]));
					var ClientSocket = TcpListener.EndAcceptSocket(AcceptState);
					var ClientStream = new NetworkStream(ClientSocket);
					var ReceivedBuffer = new ProduceConsumeBuffer<byte>();

					AsyncCallback ReceiveCallback = null;

					Action<Packet.PacketType, byte[]> HandlePacket = (PacketType, PacketData) =>
					{
						Console.WriteLine("      HandlePacket({0}, {1})", Enum.GetName(typeof(Packet.PacketType), PacketType), PacketData.ToHexString());
						switch (PacketType)
						{
							case Packet.PacketType.GetVersion:
								{
									var VersionPacket = new Packet(Packet.PacketType.GetVersion);
									VersionPacket.BinaryWriter.Write((uint)1);
									VersionPacket.WritePacketTo(ClientStream);
								}
								break;
							default:
								throw(new NotImplementedException());
						}
					};

					ReceiveCallback = ((ReceiveState) =>
					{
						Console.WriteLine("    BeginReceive");
						int BytesReceived = ClientSocket.EndReceive(ReceiveState);
						ReceivedBuffer.Produce(Datas[0].Array, 0, BytesReceived);
						{
							byte[] SizeByteArray = ReceivedBuffer.ConsumePeek(2);
							ushort PacketSize = (ushort)(SizeByteArray[0] | (SizeByteArray[1] << 8));
							int TotalPacketSize = 2 + 1 + PacketSize;
							// Packet Filled.
							if (ReceivedBuffer.ConsumeRemaining >= TotalPacketSize)
							{
								ReceivedBuffer.Consume(2);
								var PacketType = (Packet.PacketType)ReceivedBuffer.Consume(1)[0];
								var PacketData = ReceivedBuffer.Consume(PacketSize);
								HandlePacket(PacketType, PacketData);
							}
						}
						ClientSocket.BeginReceive(Datas, SocketFlags.None, ReceiveCallback, null);
					});

					ClientSocket.BeginReceive(Datas, SocketFlags.None, ReceiveCallback, null);
					ClientConnected.Set();
				}, null);
				ClientConnected.WaitOne();
			}

			//var ReaderWriterLock = new ReaderWriterLock();
			//ReaderWriterLock.wr
		}

		static void Main(string[] args)
		{
			var ServerHandler = new ServerHandler("127.0.0.1", 9777);
			ServerHandler.Listen();
			ServerHandler.Loop();

			/*
			try
			{
				Test4();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.StackTrace);
				Console.ReadKey();
			}
			*/
			//Test1();
			//Test2();
			//Test3();
			//Test4();
			//Console.ReadKey();
		}
	}
}
