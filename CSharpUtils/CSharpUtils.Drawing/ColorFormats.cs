using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpUtils
{
	public class ColorFormats
	{
		static public readonly ColorFormat RGB_565 = new ColorFormat()
		{
			Offsets = new int[] { 0, 5, 11, 0 },
			Sizes = new int[] { 5, 6, 5, 0 },
		};
	}
}
