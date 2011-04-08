using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpPspEmulator.Core
{
	unsafe public class Registers
	{
		//public fixed uint _R[32];
		protected uint _PC = 0;
		protected uint _nPC = 0;
		protected uint[] _R = new uint[32];

		public void SetRegister(uint Register, uint Value)
		{
			if (Register != 0)
			{
				_R[Register] = Value;
			}
		}

		public uint GetRegister(uint Register)
		{
			if (Register != 0)
			{
				return _R[Register];
			}
			return 0;
		}
	}
}
