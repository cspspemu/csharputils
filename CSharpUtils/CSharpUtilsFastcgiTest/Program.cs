using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpUtils.Fastcgi;

namespace CSharpUtilsFastcgiTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var FastcgiServer = new FastcgiServer();
            FastcgiServer.Listen(9000);
        }
    }
}
