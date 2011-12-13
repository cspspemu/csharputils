using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpUtils._45.MysqlAsync;

namespace CSharpUtils.Sandbox
{
	public class Sandbox
	{
		async static public Task Test()
		{
			var MysqlClientAsync = new MysqlClientAsync();
			await MysqlClientAsync.ConnectAsync();
			await MysqlClientAsync.HandlePacketAsync();
		}

		static public void Main(string[] Arguments)
		{
			Test().Wait();
			Console.WriteLine("Press a key...");
			Console.ReadKey();
		}
	}
}
