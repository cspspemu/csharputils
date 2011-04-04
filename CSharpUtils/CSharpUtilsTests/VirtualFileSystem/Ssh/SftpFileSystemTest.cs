using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSharpUtils.VirtualFileSystem.Ssh;
using System.Net;

namespace CSharpUtilsTests.VirtualFileSystem.Ssh
{
	[TestClass]
	public class SftpFileSystemTest
	{
		static protected SftpFileSystem SftpFileSystem;

		[ClassInitialize]
		public static void Initialize(TestContext testContext)
		{
			SftpFileSystem = new SftpFileSystem();
			SftpFileSystem.Connect("192.168.1.36", 22, "ubuntu", "ubuntu", 1000);
		}

		[ClassCleanup]
		public static void Cleanup()
		{
			SftpFileSystem.Shutdown();
		}

		[TestMethod]
		public void ConnectTest()
		{
			foreach (var Item in SftpFileSystem.FindFiles("/home/ubuntu"))
			{
				Console.WriteLine(Item);
			}
			//SftpFileSystem.OpenFile
			//Dns.GetHostEntry();
		}
	}
}
