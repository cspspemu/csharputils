using CSharpUtils.Fastcgi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using CSharpUtils;

namespace CSharpUtilsTests
{
	[TestClass]
	public class FastcgiHandlerTest
	{
		[TestMethod]
		public void FastcgiHandlerConstructorTest()
		{
			var InputStream = new ConsumerMemoryStream();
			var OutputStream = new ConsumerMemoryStream();

			// Version
			InputStream.WriteByte(1);

			// Type
			InputStream.WriteByte((byte)Fastcgi.PacketType.FCGI_BEGIN_REQUEST);

			// RequestId
			InputStream.WriteByte(0);
			InputStream.WriteByte(0);

			// Content Length
			InputStream.WriteByte(0);
			InputStream.WriteByte(8);

			// Padding Length
			InputStream.WriteByte(0);

			// Reserved
			InputStream.WriteByte(0);

			// Data
			InputStream.WriteByte(1);
			InputStream.WriteByte(2);
			InputStream.WriteByte(3);
			InputStream.WriteByte(4);
			InputStream.WriteByte(5);
			InputStream.WriteByte(6);
			InputStream.WriteByte(7);
			InputStream.WriteByte(8);

			var FastcgiHandler = new FastcgiHandler(InputStream, OutputStream);
			FastcgiHandler.Reader.HandlePacket += delegate(Fastcgi.PacketType Type, ushort RequestId, byte[] Content)
			{
				Assert.AreEqual(Type, Fastcgi.PacketType.FCGI_BEGIN_REQUEST);
				Assert.AreEqual(RequestId, 0);
				Console.WriteLine(Content.Implode(","));
				CollectionAssert.AreEqual(Content, new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 });
			};
			FastcgiHandler.Reader.ReadPacket();
		}
	}
}
