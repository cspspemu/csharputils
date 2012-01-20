using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CSharpUtils.Extensions
{
	static public class ColorExtensions
	{
		static public ushort Decode565(this Color Color)
		{
			return (ushort)Color.Decode(ColorFormats.RGB_565);
		}

		static public uint Decode(this Color Color, ColorFormat Format)
		{
			uint Result = 0;
			Result |= ((uint)((Color.R * BitUtils.CreateMask(Format.RedSize)) / 255)) << Format.RedOffset;
			Result |= ((uint)((Color.G * BitUtils.CreateMask(Format.GreenSize)) / 255)) << Format.GreenOffset;
			Result |= ((uint)((Color.B * BitUtils.CreateMask(Format.BlueSize)) / 255)) << Format.BlueOffset;
			Result |= ((uint)((Color.A * BitUtils.CreateMask(Format.AlphaSize)) / 255)) << Format.AlphaOffset;
			return Result;
		}
	}
}
