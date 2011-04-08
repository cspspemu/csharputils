using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpPspEmulator.Core
{
	public struct InstructionData
	{
		public uint Value;

		public uint RD { get { return (Value >> 11) & 0x1F; } }
		public uint RT { get { return (Value >> 16) & 0x1F; } }
		public uint RS { get { return (Value >> 20) & 0x1F; } }

		static public implicit operator InstructionData(uint Value)
		{
			InstructionData InstructionData = new InstructionData();
			InstructionData.Value = Value;
			return InstructionData;
		}
	}
}
