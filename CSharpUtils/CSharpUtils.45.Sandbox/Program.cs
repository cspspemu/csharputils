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
			public override async Task HandleRequestAsync(FastcgiRequestAsync Request)
			{
				var StreamWriter = new StreamWriter(Request.Stdout);
				await StreamWriter.WriteAsync("Content-type: text/html\r\n");
				await StreamWriter.WriteAsync("\r\n");
				await StreamWriter.WriteAsync("Hello World!");
				await StreamWriter.FlushAsync();
			}
		}

		static void Main(string[] args)
		{
			new MyFastcgiServerAsync().Listen(8000);
		}
	}
}
