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
        int Count = 0;

        public void Listen(ushort Port)
        {
            var TcpListener = new TcpListener(IPAddress.Parse("0.0.0.0"), Port);
            TcpListener.Start();
            while (true)
            {
                //Console.WriteLine("Waiting a connection...");
                var Socket = TcpListener.AcceptSocket();
                new Thread(() =>
                {
                    var FastcgiHandler = new FastcgiHandler(Socket, false);
                    FastcgiHandler.HandleFastcgiRequest += (FastcgiRequest) =>
                    {
                        using (var TextWriter = new StreamWriter(FastcgiRequest.StdoutStream))
                        {
                            String Output = "<html><body>Hello World!</body></html>";

                            TextWriter.WriteLine("X-Dynamic: C#");
                            TextWriter.WriteLine("Content-Type: text/html; charset=utf-8");
                            TextWriter.WriteLine("");
                            TextWriter.WriteLine(Count++);
                        }
                        //TextWriter.Flush();
                    };
                    FastcgiHandler.Reader.ReadAllPackets();

                    //Console.WriteLine("End of ReadAllPackets");
                }).Start();
            }
        }
    }
}
