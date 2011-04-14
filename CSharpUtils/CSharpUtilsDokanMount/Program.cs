using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpUtils.VirtualFileSystem;
using CSharpUtils.VirtualFileSystem.Local;
using Dokan;
using System.Threading;
using CSharpUtils.VirtualFileSystem.Ssh;
using CSharpUtils.VirtualFileSystem.Ftp;

namespace CSharpUtilsDokanMount
{
	unsafe class Program
	{
		static void Main(string[] args)
		{
			//(new TestDokanTest).Main(new string[] {});
			(new ProcessTest()).ProcessToTest();
			Console.ReadKey();
		}
	}
}
