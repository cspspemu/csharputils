using CSharpUtils.Containers.RedBlackTree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;

namespace CSharpUtilsTests
{
	[TestClass]
	public class RedBlackTreeWithStatsTest
	{
		[TestMethod]
		public void Test1()
		{
			var Stats = new RedBlackTreeWithStats<int>();
			Stats.insert(5);
			Stats.PrintTree();
			Console.WriteLine("-------------------------------");
			Stats.insert(4);
			Stats.PrintTree();
			Console.WriteLine("-------------------------------");
			Stats.insert(6);
			Stats.insert(3);
			Stats.insert(2);
			Stats.insert(1);
			Stats.insert(6);
			Stats.insert(7);
			Stats.insert(8);
			Stats.insert(9);
			Stats.insert(10);
			Stats.PrintTree();
			Stats.DebugValidateTree();
		}
	}
}
