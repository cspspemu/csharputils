using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpUtils.Drawing
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
	}
}
