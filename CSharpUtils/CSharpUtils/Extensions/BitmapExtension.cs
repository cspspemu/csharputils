using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CSharpUtils.Extensions
{
	static public class BitmapExtension
	{
		static public void ForEach(this Bitmap Bitmap, Action<Color, int, int> Delegate)
		{
			int W = Bitmap.Width, H = Bitmap.Height;
			for (int y = 0; y < H; y++)
			{
				for (int x = 0; x < W; x++)
				{
					Delegate(Bitmap.GetPixel(x, y), x, y);
				}
			}
		}
	}
}
