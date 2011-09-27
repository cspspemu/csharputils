using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;
using CSharpUtils.Extensions;

namespace CSharpUtils
{
	public class BitmapUtils
	{
		static public ColorPalette GetColorPalette(int nColors)
		{
			// Assume monochrome image.
			PixelFormat bitscolordepth = PixelFormat.Format1bppIndexed;
			ColorPalette palette;    // The Palette we are stealing
			Bitmap bitmap;     // The source of the stolen palette

			// Determine number of colors.
			if (nColors > 2) bitscolordepth = PixelFormat.Format4bppIndexed;
			if (nColors > 16) bitscolordepth = PixelFormat.Format8bppIndexed;

			// Make a new Bitmap object to get its Palette.
			bitmap = new Bitmap(1, 1, bitscolordepth);

			palette = bitmap.Palette;   // Grab the palette

			bitmap.Dispose();           // cleanup the source Bitmap

			return palette;             // Send the palette back
		}

		public enum Direction {
			FromBitmapToData = 0,
			FromDataToBitmap = 1,
		}

		unsafe static public void TransferChannelsDataLinear(Bitmap Bitmap, byte[] NewData, Direction Direction, params BitmapChannel[] Channels)
		{
			int WidthHeight = Bitmap.Width * Bitmap.Height;

			int NumberOfChannels = 1;
			foreach (var Channel in Channels)
			{
				if (Channel != BitmapChannel.Indexed)
				{
					NumberOfChannels = 4;
					break;
				}
			}

			Bitmap.LockBitsUnlock((NumberOfChannels == 1) ? PixelFormat.Format8bppIndexed : PixelFormat.Format32bppArgb, (BitmapData) =>
			{
				byte* BitmapDataScan0 = (byte*)BitmapData.Scan0.ToPointer();
				int Width = Bitmap.Width;
				int Height = Bitmap.Height;
				int Stride = BitmapData.Stride;
				if (NumberOfChannels == 1)
				{
					Stride = Width;
				}

				fixed (byte* OutputPtrStart = &NewData[0])
				{
					byte* OutputPtr = OutputPtrStart;
					foreach (var Channel in Channels)
					{
						for (int y = 0; y < Height; y++)
						{
							byte* InputPtr = BitmapDataScan0 + Stride * y;
							if (NumberOfChannels != 1)
							{
								InputPtr = InputPtr + (int)Channel;
							}
							for (int x = 0; x < Width; x++)
							{
								if (Direction == Direction.FromBitmapToData)
								{
									*OutputPtr = *InputPtr;
								}
								else
								{
									*InputPtr  = * OutputPtr;
								}
								OutputPtr++;
								InputPtr += NumberOfChannels;
							}
						}
					}
				}
			});
		}
	}
}
