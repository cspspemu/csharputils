using CSharpUtils._45.Redis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using System.IO;
using CSharpUtils.Extensions;

namespace CSharpUtils._45.Tests
{
	[TestClass()]
	public class RedisClientTest
	{
		async public Task _CommandTestAsync()
		{
			var RedisClient = new RedisClientAsync();
			/*
			await RedisClient.Connect("localhost", 6379);
			Console.WriteLine((await RedisClient.Command("set", "hello-csharp", "1")).ToJson());
			Console.WriteLine((await RedisClient.Command("get", "hello-csharp")).ToJson());
			*/
		}

		[TestMethod()]
		public void CommandTest()
		{
			var Task = _CommandTestAsync();
			Task.WaitAll(Task);
		}
	}
}
