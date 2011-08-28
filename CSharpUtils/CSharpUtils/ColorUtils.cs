using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CSharpUtils
{
	public class ColorUtils
	{
		static public void InternalAdd(ref int R, ref int G, ref int B, ref int A, params Color[] Colors)
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
			InternalAdd(ref R, ref G, ref B, ref A, Colors);
			if (L == 0) L = 1;
			return Color.FromArgb(
				(byte)(A / L),
				(byte)(R / L),
				(byte)(G / L),
				(byte)(B / L)
			);
		}

		static public Color Average(Color Color1, Color Color2)
		{
			return Color.FromArgb(
				(byte)((Color1.A + Color2.A) / 2),
				(byte)((Color1.R + Color2.R) / 2),
				(byte)((Color1.G + Color2.G) / 2),
				(byte)((Color1.B + Color2.B) / 2)
			);
		}

		static public Color Add(params Color[] Colors)
		{
			int R = 0, G = 0, B = 0, A = 0;
			InternalAdd(ref R, ref G, ref B, ref A, Colors);
			return Color.FromArgb((byte)A, (byte)R, (byte)G, (byte)B);
		}

		static public Color Add(Color Color1, Color Color2)
		{
			return Color.FromArgb(
				(byte)((Color1.A + Color2.A)),
				(byte)((Color1.R + Color2.R)),
				(byte)((Color1.G + Color2.G)),
				(byte)((Color1.B + Color2.B))
			);
		}

		static public Color Substract(Color ColorLeft, Color ColorRight)
		{
			return Color.FromArgb(
				(byte)(ColorLeft.A - ColorRight.A),
				(byte)(ColorLeft.R - ColorRight.R),
				(byte)(ColorLeft.G - ColorRight.G),
				(byte)(ColorLeft.B - ColorRight.B)
			);
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
