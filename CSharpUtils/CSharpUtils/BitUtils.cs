using System;
using System.Runtime.CompilerServices;

namespace CSharpUtils
{
	public static class BitUtils
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="Size"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint CreateMask(int Size)
		{
			return (uint)((1 << Size) - 1);
		}

	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="Value"></param>
	    /// <param name="Offset"> </param>
	    /// <param name="Count"> </param>
	    /// <param name="ValueToInsert"> </param>
	    /// <returns></returns>
	    [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Insert(ref uint Value, int Offset, int Count, uint ValueToInsert)
		{
			Value = Insert(Value, Offset, Count, ValueToInsert);
		}

	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="InitialValue"> </param>
	    /// <param name="Offset"> </param>
	    /// <param name="Count"> </param>
	    /// <param name="ValueToInsert"> </param>
	    /// <returns></returns>
	    [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint Insert(uint InitialValue, int Offset, int Count, uint ValueToInsert)
		{
			uint Mask = CreateMask(Count);
			InitialValue &= ~(Mask << Offset);
			InitialValue |= (ValueToInsert & Mask) << Offset;
			return InitialValue;
		}

	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="InitialValue"> </param>
	    /// <param name="Offset"> </param>
	    /// <param name="Count"> </param>
	    /// <returns></returns>
	    [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint Extract(uint InitialValue, int Offset, int Count)
		{
			if (Count == 0) return 0;
			return (uint)((InitialValue >> Offset) & CreateMask(Count));
		}

	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="InitialValue"> </param>
	    /// <param name="Offset"> </param>
	    /// <param name="Count"> </param>
	    /// <param name="Scale"> </param>
	    /// <returns></returns>
	    [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint ExtractScaled(uint InitialValue, int Offset, int Count, int Scale)
		{
			if (Count == 0) return 0;
			return (uint)((Extract(InitialValue, Offset, Count) * Scale) / CreateMask(Count));
		}

	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="InitialValue"> </param>
	    /// <param name="Offset"> </param>
	    /// <returns></returns>
	    [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool ExtractBool(uint InitialValue, int Offset)
		{
			return Extract(InitialValue, Offset, 1) != 0;
		}

	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="InitialValue"> </param>
	    /// <param name="Offset"> </param>
	    /// <param name="Count"> </param>
	    /// <returns></returns>
	    [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ExtractSigned(uint InitialValue, int Offset, int Count)
		{
			var Mask = CreateMask(Count);
			uint SignBit = (uint)(1 << (Offset + (Count - 1)));
			uint _Value = (uint)((InitialValue >> Offset) & Mask);
			if ((_Value & SignBit) != 0)
			{
				_Value |= ~Mask;
			}
			return (int)_Value;
		}

	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="Value"> </param>
	    /// <param name="Offset"> </param>
	    /// <param name="Count"> </param>
	    /// <param name="Scale"> </param>
	    /// <returns></returns>
	    [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float ExtractUnsignedScaled(uint Value, int Offset, int Count, float Scale = 1.0f)
		{
			return ((float)Extract(Value, Offset, Count) / (float)CreateMask(Count)) * Scale;
		}

		public static byte[] XorBytes(params byte[][] Arrays)
		{
			int Length = Arrays[0].Length;
			foreach (var Array in Arrays) if (Array.Length != Length) throw(new InvalidOperationException("Arrays sizes must match"));
			var Bytes = new byte[Length];
			foreach (var Array in Arrays)
			{
				for (int n = 0; n < Length; n++) Bytes[n] ^= Array[n];
			}
			return Bytes;
		}
	}
}
