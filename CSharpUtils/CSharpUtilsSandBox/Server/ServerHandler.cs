using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CSharpUtilsSandBox.Server
{
	public class ServerHandler
	{
		protected TcpListener TcpListener;
		protected string ListenIp;
		protected int ListenPort;
		protected ManualResetEvent ClientConnected;

		public ServerHandler(string ListenIp, int ListenPort)
		{
			this.ListenIp = ListenIp;
			this.ListenPort = ListenPort;
			this.ClientConnected = new ManualResetEvent(false);
		}

		public void Listen()
		{
			this.TcpListener = new TcpListener(IPAddress.Parse(ListenIp), ListenPort);
			this.TcpListener.Start();

			Console.WriteLine("Listening {0}:{1}...", ListenIp, ListenPort);
		}

		public void Loop()
		{

			while (true)
			{
				AcceptClient();
			}
		}

		public void AcceptClient()
		{
			Console.WriteLine("Waiting for a connection...");
			ClientConnected.Reset();
			this.TcpListener.BeginAcceptSocket((AcceptState) =>
			{
				var ClientHandler = new ClientHandler((AcceptState.AsyncState as TcpListener).EndAcceptSocket(AcceptState));
				ClientHandler.StartReceivingData();

				ClientConnected.Set();
			}, this.TcpListener);
			ClientConnected.WaitOne();
		}
	}
}
