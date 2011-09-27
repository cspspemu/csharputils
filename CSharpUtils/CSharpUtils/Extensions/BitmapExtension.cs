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
		unsafe static public void SetPalette(this Bitmap Bitmap, IEnumerable<Color> Colors)
		{
			ColorPalette Palette = BitmapUtils.GetColorPalette(Colors.Count());

			int n = 0;
			foreach (var Color in Colors)
			{
				Palette.Entries[n++] = Color;
			}

			Bitmap.Palette = Palette;
		}

		unsafe static public byte[] GetIndexedDataLinear(this Bitmap Bitmap)
		{
			return Bitmap.GetChannelsDataLinear(BitmapChannel.Indexed);
		}

		unsafe static public byte[] GetIndexedDataLinear(this Bitmap Bitmap, Rectangle Rectangle)
		{
			return Bitmap.GetChannelsDataLinear(Rectangle, BitmapChannel.Indexed);
		}

		unsafe static public void SetIndexedDataLinear(this Bitmap Bitmap, byte[] NewData)
		{
			Bitmap.SetChannelsDataLinear(NewData, BitmapChannel.Indexed);
		}

		unsafe static public void SetIndexedDataLinear(this Bitmap Bitmap, Rectangle Rectangle, byte[] NewData)
		{
			Bitmap.SetChannelsDataLinear(NewData, Rectangle, BitmapChannel.Indexed);
		}

		unsafe static public byte[] GetChannelsDataLinear(this Bitmap Bitmap, params BitmapChannel[] Channels)
		{
			return Bitmap.GetChannelsDataLinear(Bitmap.GetFullRectangle(), Channels);
		}

		unsafe static public byte[] GetChannelsDataLinear(this Bitmap Bitmap, Rectangle Rectangle, params BitmapChannel[] Channels)
		{
			var NewData = new byte[Rectangle.Width * Rectangle.Height * Channels.Length];
			BitmapUtils.TransferChannelsDataLinear(Rectangle, Bitmap, NewData, BitmapUtils.Direction.FromBitmapToData, Channels);
			return NewData;
		}

		unsafe static public void SetChannelsDataLinear(this Bitmap Bitmap, byte[] NewData, params BitmapChannel[] Channels)
		{
			Bitmap.SetChannelsDataLinear(NewData, Bitmap.GetFullRectangle(), Channels);
		}

		unsafe static public void SetChannelsDataLinear(this Bitmap Bitmap, byte[] NewData, Rectangle Rectangle, params BitmapChannel[] Channels)
		{
			BitmapUtils.TransferChannelsDataLinear(Rectangle, Bitmap, NewData, BitmapUtils.Direction.FromDataToBitmap, Channels);
		}

		unsafe static public byte[] GetChannelsDataInterleaved(this Bitmap Bitmap, params BitmapChannel[] Channels)
		{
			throw(new NotImplementedException());
		}

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

		static public Rectangle GetFullRectangle(this Bitmap Bitmap)
		{
			return new Rectangle(0, 0, Bitmap.Width, Bitmap.Height);
		}
	}
}
