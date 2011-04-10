using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpPspEmulator.Core.Cpu
{
	unsafe public class RegistersCpu
	{
		//public fixed uint _R[32];
		protected uint _PC = 0;
		protected uint _nPC = 0;
		protected uint[] _R = new uint[32];

        public uint PC
        {
            get { return _PC; }
            set { _PC = value; }
        }

        public uint nPC
        {
            get { return _nPC; }
            set { _nPC = value; }
        }

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

        public uint this[String RegisterName]
        {
            get
            {
                return _R[RegisterNameToIndex(RegisterName)];
            }
            set
            {
                _R[RegisterNameToIndex(RegisterName)] = value;
            }
        }

        static public int RegisterNameToIndex(String RegisterName)
        {
            if (RegisterName.Length < 2) throw (new Exception("Invalid RegisterName '" + RegisterName + "'"));
            if (RegisterName[0] == 'r')
            {
                return Convert.ToInt32(RegisterName.Substring(1), 10);
            }
            else
            {
                switch (RegisterName)
                {
                    case "zr": return 0;
                }
                throw (new Exception("Invalid RegisterName '" + RegisterName + "'"));
            }
        }
	}

    unsafe public class RegistersFpu
    {
    }

    unsafe public class RegistersVFpu
    {
    }
}
