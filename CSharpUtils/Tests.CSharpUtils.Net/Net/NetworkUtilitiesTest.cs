using CSharpUtils.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Sockets;
using System.Net;

namespace CSharpUtilsTests
{
	[TestClass]
	public class NetworkUtilitiesTest
	{
		[TestMethod]
		public void GetAvailableTcpPortTest()
		{
			int Port = NetworkUtilities.GetAvailableTcpPort();
			var Listener = new TcpListener(IPAddress.Parse("127.0.0.1"), Port);
			Listener.Start();
			Listener.Stop();
		}
	}
}
