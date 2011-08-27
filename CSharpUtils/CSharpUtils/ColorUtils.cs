using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CSharpUtils
{
	public class ColorUtils
	{
		static public void InternalSum(ref int R, ref int G, ref int B, ref int A, params Color[] Colors)
		{
			foreach (var Color in Colors)
			{
				R += Color.R;
				G += Color.G;
				B += Color.B;
				A += Color.A;
			}
		}

		static public Color Average(params Color[] Colors)
		{
			int R = 0, G = 0, B = 0, A = 0;
			int L = Colors.Length;
			InternalSum(ref R, ref G, ref B, ref A, Colors);
			if (L == 0) L = 1;
			return Color.FromArgb(
				(byte)(A / L),
				(byte)(R / L),
				(byte)(G / L),
				(byte)(B / L)
			);
		}

		static public Color Sum(params Color[] Colors)
		{
			int R = 0, G = 0, B = 0, A = 0;
			InternalSum(ref R, ref G, ref B, ref A, Colors);
			return Color.FromArgb((byte)A, (byte)R, (byte)G, (byte)B);
		}

		static public Color Average(Bitmap Bitmap)
		{
			Color[] Colors = new Color[Bitmap.Width * Bitmap.Height];
			for (int y = 0, n = 0; y < Bitmap.Height; y++)
			{
				for (int x = 0; x < Bitmap.Width; x++, n++)
				{
					Colors[n] = Bitmap.GetPixel(x, y);
				}
			}
			return Average(Colors);
		}
	}
}
