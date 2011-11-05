using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpUtils.Web._45.Fastcgi;

namespace CSharpUtils._45.Sandbox
{
	class Program
	{
		class MyFastcgiServerAsync : FastcgiServerAsync
		{
			public override async Task HandleRequest(FastcgiServerClientRequestHandlerAsync FastcgiServerClientRequestHandlerAsync)
			{
				//await Console.Out.WriteLineAsync("HandleRequest!");
				var StreamWriter = new StreamWriter(FastcgiServerClientRequestHandlerAsync.StdoutStream);
				await StreamWriter.WriteAsync("Content-type: text/html\r\n");
				await StreamWriter.WriteAsync("\r\n");
				await StreamWriter.WriteAsync("Hello World!");
				await StreamWriter.FlushAsync();
			}
		}

		static void Main(string[] args)
		{
			//new MyFastcgiServerAsync().Listen(9000);
			new MyFastcgiServerAsync().Listen(8000);
		}
	}
}
