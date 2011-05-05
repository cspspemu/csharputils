using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;

namespace CSharpUtils.Fastcgi
{
    public class FastcgiServer
    {
        bool Debug = false;
        int Count = 0;

        public void Listen(ushort Port)
        {
            var TcpListener = new TcpListener(IPAddress.Parse("0.0.0.0"), Port);
            TcpListener.Start();
            while (true)
            {
                if (Debug)
                {
                    Console.WriteLine("Waiting a connection...");
                }
                var AcceptedSocket = TcpListener.AcceptSocket();

                ThreadPool.QueueUserWorkItem(HandleAcceptedSocket, AcceptedSocket);
            }
        }

        public void HandleAcceptedSocket(Object _Socket)
        {
            var Socket = (Socket)_Socket;
            if (Debug)
            {
                Console.WriteLine("HandleAcceptedSocket: " + Socket);
            }
            var FastcgiHandler = new FastcgiHandler(Socket, Debug);
            FastcgiHandler.HandleFastcgiRequest += (FastcgiRequest) =>
            {
                using (var TextWriter = new StreamWriter(FastcgiRequest.StdoutStream))
                {
                    //String Output = "<html><body>Hello World!</body></html>";

                    TextWriter.WriteLine("X-Dynamic: C#");
                    TextWriter.WriteLine("Content-Type: text/html; charset=utf-8");
                    TextWriter.WriteLine("");
                    TextWriter.WriteLine(Count++);
                }
                //TextWriter.Flush();
            };
            FastcgiHandler.Reader.ReadAllPackets();
        }
    }
}
