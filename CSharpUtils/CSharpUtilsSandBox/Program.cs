using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpUtils.Containers.RedBlackTree;
using System.Net.Sockets;
using System.Threading;

namespace CSharpUtilsSandBox
{
	class Program
	{
		static void Test1()
		{
			Console.WriteLine("-----------------------------------------");
			var Stats = (new RedBlackTreeWithStats<int>()).Clone();
			{
				var Start = DateTime.Now;
				for (int n = 0; n < 500000; n++)
				{
					Stats.insert(n); // 1
				}
				Console.WriteLine("Time: " + (DateTime.Now - Start).TotalMilliseconds);
			}
			//Stats.PrintTree();
			{
				var Start = DateTime.Now;
				int Value = 0;
				for (int n = 0; n < 100; n++)
				{
					Value = Stats.All.Length;
					Value = Stats.All.Skip(250000).Take(240000).Count();
				}
				Console.WriteLine(Value);
				Console.WriteLine("Time: " + (DateTime.Now - Start).TotalMilliseconds);
			}
			{
				var Start = DateTime.Now;
				int Value = 0;
				for (int n = 0; n < 1000; n++)
				{
					Value = Stats.All.GetItemPosition(250000);
					Value = Stats.Where(Item => Item < 250000).Count();
				}
				Console.WriteLine(Value);
				Console.WriteLine("Time: " + (DateTime.Now - Start).TotalMilliseconds);
			}
			/*
			{
				var Start = DateTime.Now;
				int Value = 0;
				for (int n = 0; n < 100; n++)
				{
					//int Value = Stats.All.Count();
					Value = Stats.Count();
					Value = Stats.Skip(50000).Count();
				}
				Console.WriteLine(Value);
				Console.WriteLine(DateTime.Now - Start);
			}
			 * */
			/*
			foreach (var Item in Stats.All.Where(Item => Item > 3).Count())
			{
				Console.WriteLine(Item);
			}
			*/
			//Stats.DebugValidateTree();
		}

		static void Test2()
		{
			Console.WriteLine("-----------------------------------------");
			var Stats = new Dictionary<int, int>();
			//var Stats = new SortedList<int, int>();
			{
				var Start = DateTime.Now;
				for (int n = 0; n < 500000; n++)
				{
					Stats.Add(500000 - n, n); // 1
				}
				Console.WriteLine("Time: " + (DateTime.Now - Start).TotalMilliseconds);
			}
			//Stats.PrintTree();
			{
				var Start = DateTime.Now;
				int Value = 0;
				for (int n = 0; n < 100; n++)
				{
					Value = Stats.Count;
					Value = Stats.Skip(250000).Take(240000).Count();
				}
				Console.WriteLine(Value);
				Console.WriteLine("Time: " + (DateTime.Now - Start).TotalMilliseconds);
			}
			{
				var Start = DateTime.Now;
				int Value = 0;
				for (int n = 0; n < 1000; n++)
				{
					//Value = Stats.IndexOfValue(250000);
				}
				Console.WriteLine(Value);
				Console.WriteLine("Time: " + (DateTime.Now - Start).TotalMilliseconds);
			}
			/*
			foreach (var Item in Stats.All.Where(Item => Item > 3).Count())
			{
				Console.WriteLine(Item);
			}
			*/
			//Stats.DebugValidateTree();
		}

		static void Test3()
		{
			var Stats = (new RedBlackTreeWithStats<int>()).Clone();
			Stats.Add(1);
			Stats.Add(2);
			Stats.Add(3);
			Console.WriteLine(
				Stats
					.Skip(1)
					.Where(Item => Item >= 3)
					.Count()
			);
		}

		static void Test4()
		{
			var TcpListener = new TcpListener(9999);
			while (true)
			{
				var TcpClient = TcpListener.AcceptTcpClient();
			}
			var ReaderWriterLock = new ReaderWriterLock();
			//ReaderWriterLock.wr
		}

		static void Main(string[] args)
		{
			Test4();
			//Test2();
			//Test3();
			//Test4();
			//Console.ReadKey();
		}
	}
}
