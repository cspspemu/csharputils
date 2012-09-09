using System.Collections.Generic;
using CSharpUtils.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
