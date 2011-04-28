using CSharpUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;

namespace CSharpUtilsTests
{
	[TestClass]
	public class StreamExtensionsTest
	{
		[TestMethod]
		public void CopyToFastTest()
		{
			var Input = new MemoryStream();
			var Output = new MemoryStream();
			var Output2 = new MemoryStream();

			for (int n = 0; n < 0x100; n++)
			{
				Input.WriteByte((byte)n);
			}

			Input.Position = 0; Input.CopyTo(Output2);
			Input.Position = 0; Input.CopyToFast(Output);

			Assert.AreEqual(Output.Length, Input.Length);
			CollectionAssert.AreEqual(Input.ToArray(), Output.ToArray());
			CollectionAssert.AreEqual(Input.ToArray(), Output2.ToArray());
		}
	}
}
