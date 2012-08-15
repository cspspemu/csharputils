using CSharpUtils.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CSharpUtils;

namespace CSharpUtilsTests
{
	[TestClass]
	public class TcpTestServerTest
	{
		[TestMethod]
		public void CreateTest()
		{
			var TestTcpTestServer = TcpTestServer.Create();
			TestTcpTestServer.RemoteTcpClient.GetStream().WriteStringz("Hello World");
			Assert.AreEqual("Hello World", TestTcpTestServer.LocalTcpClient.GetStream().ReadStringz());

			TestTcpTestServer.LocalTcpClient.GetStream().WriteStringz("Hello World");
			Assert.AreEqual("Hello World", TestTcpTestServer.RemoteTcpClient.GetStream().ReadStringz());
		}
	}
}
