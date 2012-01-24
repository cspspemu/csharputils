using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpUtils
{
	public struct ColorFormat
	{
		public int RedOffset, GreenOffset, BlueOffset, AlphaOffset;
		public int RedSize, GreenSize, BlueSize, AlphaSize;

		public int[] Offsets {
			get
			{
				return new[] { RedOffset, GreenOffset, BlueOffset, AlphaOffset };
			}
			set
			{
				RedOffset = value[0];
				GreenOffset = value[1];
				BlueOffset = value[2];
				AlphaOffset = value[3];
			}
		}

		public int[] Sizes
		{
			get
			{
				return new[] { RedSize, GreenSize, BlueSize, AlphaSize };
			}
			set
			{
				RedSize = value[0];
				GreenSize = value[1];
				BlueSize = value[2];
				AlphaSize = value[3];
			}
		}

		public uint Encode(byte R, byte G, byte B, byte A)
		{
			uint Result = 0;
			Result |= ((uint)((R * BitUtils.CreateMask(RedSize)) / 255)) << RedOffset;
			Result |= ((uint)((G * BitUtils.CreateMask(GreenSize)) / 255)) << GreenOffset;
			Result |= ((uint)((B * BitUtils.CreateMask(BlueSize)) / 255)) << BlueOffset;
			Result |= ((uint)((A * BitUtils.CreateMask(AlphaSize)) / 255)) << AlphaOffset;
			return Result;
		}

		public void Decode(uint Data, out byte R, out byte G, out byte B, out byte A)
		{
			throw(new NotImplementedException());
		}
	}
}
