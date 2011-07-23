using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpUtils.Fastcgi;
using System.IO;
using System.Threading;

namespace CSharpUtilsFastcgiTest
{
    class MyFastcgiServer : FastcgiServer
    {
        int Count = 0;

        protected override void HandleFascgiRequest(FastcgiRequest FastcgiRequest)
        {
            using (var TextWriter = new StreamWriter(FastcgiRequest.StdoutStream, Encoding.UTF8))
            {
                TextWriter.WriteLine("X-Dynamic: C#");
                TextWriter.WriteLine("Content-Type: text/html; charset=utf-8");
                TextWriter.WriteLine("");
                TextWriter.WriteLine(Count++);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var FastcgiServer = new MyFastcgiServer();
            FastcgiServer.Listen(9001, "127.0.0.1");
        }
    }
}
