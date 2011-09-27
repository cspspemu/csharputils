using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpUtils
{
	static public class MathUtils
	{
		// http://www.lambda-computing.com/publications/articles/generics2/
		// http://www.codeproject.com/KB/cs/genericoperators.aspx
		static public T Clamp<T>(T Value, T Min, T Max)
		{
			if ((dynamic)Value < (dynamic)Min) return Min;
			if ((dynamic)Value > (dynamic)Max) return Max;
			return Value;
		}

		static public void Swap<Type>(ref Type A, ref Type B)
		{
			Type T = A;
			A = B;
			B = T;
		}

		/// <summary>
		/// Returns the upper minimum value that will be divisible by AlignValue.
		/// </summary>
		/// <example>
		/// Align(0x1200, 0x800) == 0x1800
		/// </example>
		/// <param name="Value"></param>
		/// <param name="AlignValue"></param>
		/// <returns></returns>
		public static long Align(long Value, long AlignValue)
		{
			if ((Value % AlignValue) != 0)
			{
				Value += (AlignValue - (Value % AlignValue));
			}
			return Value;
		}
	}
}
