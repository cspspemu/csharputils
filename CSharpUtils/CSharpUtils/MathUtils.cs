using System;

namespace CSharpUtils
{
	public static class MathUtils
	{
		// http://www.lambda-computing.com/publications/articles/generics2/
		// http://www.codeproject.com/KB/cs/genericoperators.aspx
		public static T Clamp<T>(T Value, T Min, T Max) where T : IComparable
		{
			if (Value.CompareTo(Min) < 0) return Min;
			if (Value.CompareTo(Max) > 0) return Max;
			return Value;
		}

		public static int FastClamp(int Value, int Min, int Max)
		{
			if (Value < Min) return Min;
			if (Value > Max) return Max;
			return Value;
		}

		public static void Swap<Type>(ref Type A, ref Type B)
		{
			LanguageUtils.Swap(ref A, ref B);
		}

		/// <summary>
		/// Useful for converting LittleEndian to BigEndian and viceversa.
		/// </summary>
		/// <param name="Value"></param>
		/// <returns></returns>
		public static ushort ByteSwap(ushort Value)
		{
			return (ushort)((Value >> 8) | (Value << 8));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Value"></param>
		/// <returns></returns>
		public static uint ByteSwap(uint Value)
		{
			return (
				((uint)ByteSwap((ushort)(Value >> 0)) << 16) |
				((uint)ByteSwap((ushort)(Value >> 16)) << 0)
			);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Value"></param>
		/// <returns></returns>
		public static ulong ByteSwap(ulong Value)
		{
			return (
				((ulong)ByteSwap((uint)(Value >> 0)) << 32) |
				((ulong)ByteSwap((uint)(Value >> 32)) << 0)
			);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Value"></param>
		/// <returns></returns>
		unsafe public static float ByteSwap(float Value)
		{
			var ValueSW = ByteSwap(*(uint*)&Value);
			return *(float*)&ValueSW;
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

		public static long RequiredBlocks(long Size, long BlockSize)
		{
			if ((Size % BlockSize) != 0)
			{
				return (Size / BlockSize) + 1;
			}
			else
			{
				return Size / BlockSize;
			}
		}

		public static uint PrevAligned(uint Value, int Alignment)
		{
			if ((Value % Alignment) != 0)
			{
				Value -= (uint)((Value % Alignment));
			}
			return Value;
		}

		public static uint NextAligned2(uint Value, uint Alignment)
		{
			return (Value + Alignment) & ~Alignment;
		}

		public static uint NextAligned(uint Value, int Alignment)
		{
			return (uint)NextAligned((long)Value, (long)Alignment);
		}

		public static long NextAligned(long Value, long Alignment)
		{
			if (Alignment != 0)
			{
				if ((Value % Alignment) != 0)
				{
					Value += (long)(Alignment - (Value % Alignment));
				}
			}
			return Value;
		}

		public static int NextPowerOfTwo(int BaseValue)
		{
			int NextPowerOfTwoValue = 1;
			while (NextPowerOfTwoValue < BaseValue) NextPowerOfTwoValue <<= 1;
			return NextPowerOfTwoValue;
		}

		public static int Max(params int[] Items)
		{
			var MaxValue = Items[0];
			foreach (var Item in Items) if (MaxValue < Item) MaxValue = Item;
			return MaxValue;
		}

		public static uint Max(params uint[] Items)
		{
			var MaxValue = Items[0];
			foreach (var Item in Items) if (MaxValue < Item) MaxValue = Item;
			return MaxValue;
		}
	}
}
