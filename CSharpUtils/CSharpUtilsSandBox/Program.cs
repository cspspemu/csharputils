using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpUtils.Containers.RedBlackTree;

namespace CSharpUtilsSandBox
{
	class Program
	{
		static void Test1()
		{
			var Stats = new RedBlackTreeWithStats<int>();
			for (int n = 0; n < 100000; n++)
			{
				Stats.insert(n); // 1
			}
			//Stats.PrintTree();
			{
				var Start = DateTime.Now;
				for (int n = 0; n < 100; n++)
				{
					int Value = Stats.All.Length;
				}
				Console.WriteLine(DateTime.Now - Start);
			}
			{
				var Start = DateTime.Now;
				for (int n = 0; n < 100; n++)
				{
					int Value = Stats.All.Count();
				}
				Console.WriteLine(DateTime.Now - Start);
			}
			/*
			foreach (var Item in Stats.All.Where(Item => Item > 3).Count())
			{
				Console.WriteLine(Item);
			}
			*/
			//Stats.DebugValidateTree();
		}

		static void Main(string[] args)
		{
			Test1();
			Console.ReadKey();
		}
	}
}
