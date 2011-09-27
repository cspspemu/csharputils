using CSharpUtils.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using CSharpUtils;

namespace CSharpUtilsTests
{
	[TestClass()]
	public class BitmapExtensionTest
	{
		[TestMethod()]
		public void GetChannelsDataLinearTest()
		{
			Bitmap Bitmap = new Bitmap(2, 2);
			Bitmap.SetPixel(0, 0, Color.FromArgb(0x00, 0x04, 0x08, 0x0C));
			Bitmap.SetPixel(1, 0, Color.FromArgb(0x01, 0x05, 0x09, 0x0D));
			Bitmap.SetPixel(0, 1, Color.FromArgb(0x02, 0x06, 0x0A, 0x0E));
			Bitmap.SetPixel(1, 1, Color.FromArgb(0x03, 0x07, 0x0B, 0x0F));
			Assert.AreEqual(
				"000102030405060708090a0b0c0d0e0f",
				Bitmap.GetChannelsDataLinear(BitmapChannel.Alpha, BitmapChannel.Red, BitmapChannel.Green, BitmapChannel.Blue).ToHexString()
			);
		}
	}
}
