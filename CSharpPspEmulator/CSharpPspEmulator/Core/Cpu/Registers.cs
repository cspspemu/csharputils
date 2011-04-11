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

        public uint HI, LO;

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

        public uint this[uint Index]
        {
            get
            {
                return GetRegister(Index);
            }
            set
            {
                SetRegister(Index, value);
            }
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

        public static String[] Memonics = new String[] {
            "zr", "at", "v0", "v1", "a0", "a1", "a2", "a3",
            "t0", "t1", "t2", "t3", "t4", "t5", "t6", "t7",
            "s0", "s1", "s2", "s3", "s4", "s5", "s6", "s7",
            "t8", "t9", "k0", "k1", "gp", "sp", "fp", "ra",
        };

        public static String[] NoMemonics = new String[] {
            "r0", "r1", "r2", "r3", "r4", "r5", "r6", "r7",
            "r8", "r9", "r10", "r11", "r12", "r13", "r14", "r15",
            "r16", "r17", "r18", "r19", "r20", "r21", "r22", "r23",
            "r24", "r25", "r26", "r27", "r28", "r29", "r30", "r31",
        };

        static Dictionary<String, int> Memonics_rev = new Dictionary<String, int>();

        static RegistersCpu()
        {
            for (int n = 0; n < 32; n++)
            {
                Memonics_rev[Memonics[n]] = n;
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
                if (Memonics_rev.ContainsKey(RegisterName))
                {
                    return Memonics_rev[RegisterName];
                }
                else
                {
                    throw (new Exception("Invalid RegisterName '" + RegisterName + "'"));
                }
            }
        }

        internal void Reset()
        {
            for (int n = 1; n < 32; n++) _R[n] = 0;
        }
    }

    unsafe public class RegistersFpu
    {
    }

    unsafe public class RegistersVFpu
    {
    }
}
