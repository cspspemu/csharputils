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
		static public T Clamp<T>(T Value, T Min, T Max) where T : IComparable
		{
			if (Value.CompareTo(Min) < 0) return Min;
			if (Value.CompareTo(Max) > 0) return Max;
			return Value;
		}

		static public void Swap<Type>(ref Type A, ref Type B)
		{
			LanguageUtils.Swap(ref A, ref B);
		}

		/// <summary>
		/// Useful for converting LittleEndian to BigEndian and viceversa.
		/// </summary>
		/// <param name="Value"></param>
		/// <returns></returns>
		static public ushort ByteSwap(ushort Value)
		{
			return (ushort)((Value >> 8) | (Value << 8));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Value"></param>
		/// <returns></returns>
		static public uint ByteSwap(uint Value)
		{
			return (
				((uint)ByteSwap((ushort)(Value >> 0)) << 16) |
				((uint)ByteSwap((ushort)(Value >> 16)) << 0)
			);
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

		public static uint PrevAligned(uint Value, int Alignment)
		{
			if ((Value % Alignment) != 0)
			{
				Value -= (uint)((Value % Alignment));
			}
			return Value;
		}

		public static uint NextAligned(uint Value, int Alignment)
		{
			return (uint)NextAligned((long)Value, (long)Alignment);
		}

		public static long NextAligned(long Value, long Alignment)
		{
			if ((Value % Alignment) != 0)
			{
				Value += (long)(Alignment - (Value % Alignment));
			}
			return Value;
		}

		public static int NextPowerOfTwo(int BaseValue)
		{
			int NextPowerOfTwoValue = 1;
			while (NextPowerOfTwoValue < BaseValue) NextPowerOfTwoValue <<= 1;
			return NextPowerOfTwoValue;
		}
	}
}
