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
	}
}
