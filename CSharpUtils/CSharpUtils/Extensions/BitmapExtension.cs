using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace CSharpUtils.Extensions
{
	static public class BitmapExtension
	{
		static public void LockBitsUnlock(this Bitmap Bitmap, PixelFormat PixelFormat, Action<BitmapData> Callback)
		{
			var BitmapData = Bitmap.LockBits(new Rectangle(0, 0, Bitmap.Width, Bitmap.Height), ImageLockMode.ReadWrite, PixelFormat);
			
			try {
				Callback(BitmapData);
			} finally {
				Bitmap.UnlockBits(BitmapData);
			}
		}

		static public void ForEach(this Bitmap Bitmap, Action<Color, int, int> Delegate)
		{
			int Width = Bitmap.Width, Height = Bitmap.Height;
			for (int y = 0; y < Height; y++)
			{
				for (int x = 0; x < Width; x++)
				{
					Delegate(Bitmap.GetPixel(x, y), x, y);
				}
			}
		}

		static public IEnumerable<Color> Colors(this Bitmap Bitmap)
		{
			int Width = Bitmap.Width, Height = Bitmap.Height;
			for (int y = 0; y < Height; y++)
			{
				for (int x = 0; x < Width; x++)
				{
					yield return Bitmap.GetPixel(x, y);
				}
			}
		}

	}
}
