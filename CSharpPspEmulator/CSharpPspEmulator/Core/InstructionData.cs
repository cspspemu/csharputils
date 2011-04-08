using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpPspEmulator.Utils;

namespace CSharpPspEmulator.Core
{
	public struct InstructionData
	{
		public uint Value;

        public uint RD {
            get { return Value.ExtractUnsigned(11 + 5 * 0, 5); }
            set { Value = Value.Insert(11 + 5 * 0, 5, (int)value); }
        }
        public uint   RT   {
            get { return Value.ExtractUnsigned(11 + 5 * 1, 5); }
            set { Value = Value.Insert(11 + 5 * 1, 5, (int)value); }
        }
        public uint   RS   {
            get { return Value.ExtractUnsigned(11 + 5 * 2, 5); }
            set { Value = Value.Insert(11 + 5 * 2, 5, (int)value); }
        }
        public short  IMM  {
            get { return (short)((Value >> 0) & 0xFFFF); }
            set { Value = Value.Insert(0, 16, (int)value); }
        }
        public ushort IMMU {
            get { return (ushort)Value.ExtractUnsigned(0, 16); }
            set { Value = Value.Insert(0, 16, (int)value); }
        }

		static public implicit operator InstructionData(uint Value)
		{
			InstructionData InstructionData = new InstructionData();
			InstructionData.Value = Value;
			return InstructionData;
		}
	}
}
