using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSharpUtils.Http;

namespace CSharpUtilsTests.Http
{
	[TestClass]
	public class HttpUtilsTest
	{
		[TestMethod]
		public void TestParseUrlEncoded()
		{
			CollectionAssert.AreEquivalent(
				new Dictionary<String, String> { },
				HttpUtils.ParseUrlEncoded("")
			);

			CollectionAssert.AreEquivalent(
				new Dictionary<String, String> { { "test", "1" } },
				HttpUtils.ParseUrlEncoded("test=1")
			);

			CollectionAssert.AreEquivalent(
				new Dictionary<String, String> { { "test", "1" }, { "test2", "2" } },
				HttpUtils.ParseUrlEncoded("test=1&test2=2")
			);

		}

		[TestMethod]
		public void TestDecodeURIComponentUnicode()
		{
			Assert.AreEqual("Hello", HttpUtils.DecodeURIComponent("Hello"));
			Assert.AreEqual("HellodTest", HttpUtils.DecodeURIComponent("Hello%64Test"));
			Assert.AreEqual("HellodTest", HttpUtils.DecodeURIComponent("Hello%u0064Test"));
			Assert.AreEqual("dd", HttpUtils.DecodeURIComponent("%64%64"));
		}
	}
}
