using CSharpUtils.Scgi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CSharpUtils.Net;
using System.Threading;
using System.Net.Sockets;
using CSharpUtils;
using System.IO;
using CSharpUtils.Http;
using System.Collections.Generic;

namespace CSharpUtilsTests
{
	[TestClass]
	public class ScgiServerTest
	{
		class TestScgiServer : ScgiServer
		{
			public AutoResetEvent HandleRequestEvent = new AutoResetEvent(false);

			public TestScgiServer(string BindIp, int BindPort) : base(BindIp, BindPort)
			{
			}

			protected override void HandleRequest(HttpHeaderList HttpHeaderList, Dictionary<string, string> Parameters, byte[] PostContent)
			{
				HandleRequestEvent.Set();
			}
		}

		[TestMethod]
		public void ScgiServerConstructorTest()
		{
			string BindIp = "127.0.0.1";
			int BindPort = NetworkUtilities.GetAvailableTcpPort();
			var ScgiServer = new TestScgiServer(BindIp, BindPort);
			ScgiServer.Listen();
			new Thread(() =>
			{
				ScgiServer.AcceptLoop();
			}).Start();

			var TcpClient = new TcpClient(BindIp, BindPort);
			var TcpClientStream = TcpClient.GetStream();
			var HeaderStream = new MemoryStream().PreservePositionAndLock((Stream) =>
			{
				Stream.WriteStringzPair("CONTENT_LENGTH", "0");
				Stream.WriteStringzPair("SCGI", "1");
				Stream.WriteStringzPair("REQUEST_METHOD", "GET");
				Stream.WriteStringzPair("REQUEST_URI", "/test");
			});
			TcpClientStream.WriteString(HeaderStream.Length + ":");
			TcpClientStream.WriteBytes(HeaderStream.ToArray());
			TcpClientStream.WriteByte((byte)',');
			Assert.IsTrue(ScgiServer.HandleRequestEvent.WaitOne(1000));
		}
	}
}
