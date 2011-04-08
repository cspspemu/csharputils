using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpPspEmulator.Core
{
	public class CpuState
	{
		public InstructionData InstructionData;
		public Registers Registers;
		public Memory Memory;
		public CpuBase CpuBase;

		public CpuState(Registers Registers, CpuBase CpuBase, Memory Memory)
		{
			this.Registers = Registers;
			this.CpuBase = CpuBase;
			this.Memory = Memory;
		}

		public uint RT
		{
			set { Registers.SetRegister(InstructionData.RT, value); }
			get { return Registers.GetRegister(InstructionData.RT); }
		}

		public uint RS
		{
			set { Registers.SetRegister(InstructionData.RS, value); }
			get { return Registers.GetRegister(InstructionData.RS); }
		}

		public uint RD
		{
			set { Registers.SetRegister(InstructionData.RD, value); }
			get { return Registers.GetRegister(InstructionData.RD); }
		}
	}
}
