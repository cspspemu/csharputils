using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpUtils.Web._45.Fastcgi
{
	abstract public class FastcgiServerAsync
	{
		protected TcpListener TcpListener;
		protected CancellationToken CancellationToken;
		public bool Debug = false;

		abstract public Task HandleRequest(FastcgiServerClientRequestHandlerAsync FastcgiServerClientRequestHandlerAsync);

		async public void ListenAsync(ushort Port, string Address = "0.0.0.0")
		{
			TcpListener = new TcpListener(IPAddress.Parse(Address), Port);
			TcpListener.Start();
			if (Debug) await Console.Out.WriteLineAsync(String.Format("Listening {0}:{1}", Address, Port));
			
			while (true)
			{
				var FastcgiServerClientHandlerAsync = new FastcgiServerClientHandlerAsync(this, await TcpListener.AcceptTcpClientAsync());
				await FastcgiServerClientHandlerAsync.Handle();
			}
		}

		public void Listen(ushort Port, string Address = "0.0.0.0")
		{
			ListenAsync(Port, Address);
			//new Mutex().WaitOne();
			while (true)
			{
				Thread.Sleep(int.MaxValue);
			}
		}
	}
}
