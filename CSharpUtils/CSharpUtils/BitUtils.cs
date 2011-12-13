using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpUtils
{
	public class BitUtils
	{
		static public uint CreateMask(int Size)
		{
			return (uint)((1 << Size) - 1);
		}

		static public void Insert(ref uint Value, int Offset, int Count, uint ValueToInsert)
		{
			Value = Insert(Value, Offset, Count, ValueToInsert);
		}

		static public uint Insert(uint InitialValue, int Offset, int Count, uint ValueToInsert)
		{
			uint Mask = CreateMask(Count);
			InitialValue &= ~(Mask << Offset);
			InitialValue |= (ValueToInsert & Mask) << Offset;
			return InitialValue;
		}

		static public uint Extract(uint InitialValue, int Offset, int Count)
		{
			return (uint)((InitialValue >> Offset) & CreateMask(Count));
		}

		static public int ExtractSigned(uint InitialValue, int Offset, int Count)
		{
			uint SignBit = (uint)(1 << (Offset + (Count - 1)));
			int _Value = (int)((InitialValue >> Offset) & CreateMask(Count));
			if ((_Value & SignBit) != 0)
			{
				throw (new NotImplementedException());
			}
			return _Value;
		}

		public static float ExtractUnsignedScaled(uint Value, int Offset, int Count, float Scale = 1.0f)
		{
			return ((float)Extract(Value, Offset, Count) / (float)CreateMask(Count)) * Scale;
			throw new NotImplementedException();
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
