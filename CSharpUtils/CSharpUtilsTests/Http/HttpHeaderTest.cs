using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSharpUtils.Http;

namespace CSharpUtilsTests.Http
{
	[TestClass]
	public class HttpHeaderTest
	{
		[TestMethod]
		public void TestMethod1()
		{
			HttpHeader ContentType = new HttpHeader("Content-Type", "multipart/form-data; boundary=----WebKitFormBoundaryIMw3ByBOPx38V6Bd");
			var ContentTypeParts = ContentType.ParseValue("type");
			CollectionAssert.AreEqual(new Dictionary<string, string>() {
				{ "type", "multipart/form-data" },
				{ "boundary", "----WebKitFormBoundaryIMw3ByBOPx38V6Bd" },
			}, ContentTypeParts);

			ContentType = new HttpHeader("Content-Type", "multipart/form-data; boundary=\"----WebKitFormBoundaryIMw3ByBOPx38V6Bd\"");
			ContentTypeParts = ContentType.ParseValue("type");
			CollectionAssert.AreEqual(new Dictionary<string, string>() {
				{ "type", "multipart/form-data" },
				{ "boundary", "----WebKitFormBoundaryIMw3ByBOPx38V6Bd" },
			}, ContentTypeParts);
		}
	}
}
