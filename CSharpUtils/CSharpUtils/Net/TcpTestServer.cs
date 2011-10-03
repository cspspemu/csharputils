using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace CSharpUtils.Net
{
	public class TcpTestServer
	{
		public TcpListener TcpListener;
		public TcpClient LocalTcpClient;
		public TcpClient RemoteTcpClient;

		static public TcpTestServer Create()
		{
			var TcpTestServer = new TcpTestServer();
			{
				var BindIp = "127.0.0.1";
				var BindPort = NetworkUtilities.GetAvailableTcpPort();
				TcpTestServer.TcpListener = new TcpListener(IPAddress.Parse(BindIp), BindPort);
				TcpTestServer.TcpListener.Start();
				var Event = new ManualResetEvent(false);
				TcpTestServer.TcpListener.BeginAcceptTcpClient((AsyncResult) =>
				{
					TcpTestServer.LocalTcpClient = TcpTestServer.TcpListener.EndAcceptTcpClient(AsyncResult);
					Event.Set();
				}, null);
				TcpTestServer.RemoteTcpClient = new TcpClient(BindIp, BindPort);
				Event.WaitOne();
			}
			return TcpTestServer;
		}
	}
}
