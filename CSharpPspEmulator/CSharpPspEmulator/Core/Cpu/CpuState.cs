using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpPspEmulator.Core.Cpu
{
	public class CpuState
	{
		public InstructionData InstructionData;
		public RegistersCpu RegistersCpu;
        public RegistersFpu RegistersFpu;
        public RegistersVFpu RegistersVFpu;
		public Memory Memory;
		public CpuBase CpuBase;

		public CpuState(RegistersCpu Registers, CpuBase CpuBase, Memory Memory)
		{
			this.RegistersCpu = Registers;
			this.CpuBase = CpuBase;
			this.Memory = Memory;
		}

		public uint RT
		{
			set { RegistersCpu.SetRegister(InstructionData.RT, value); }
			get { return RegistersCpu.GetRegister(InstructionData.RT); }
		}

		public uint RS
		{
			set { RegistersCpu.SetRegister(InstructionData.RS, value); }
			get { return RegistersCpu.GetRegister(InstructionData.RS); }
		}

		public uint RD
		{
			set { RegistersCpu.SetRegister(InstructionData.RD, value); }
			get { return RegistersCpu.GetRegister(InstructionData.RD); }
		}

        public int IMM
        {
            get { return InstructionData.IMM ; }
        }
    
        public uint IMMU
        {
            get { return InstructionData.IMMU; }
        }
    }
}
