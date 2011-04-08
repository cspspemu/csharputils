using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpPspEmulator.Core
{
	public class Cpu : CpuBase
	{
		static Cpu()
		{
			//Execute = InstructionAttribute.ProcessClass(typeof(Cpu));
		}

		public override void ADD(CpuState CpuState)
		{
			CpuState.RT = CpuState.RD + CpuState.RS;
		}

		public override void ADDU(CpuState CpuState)
		{
			CpuState.RT = CpuState.RD + CpuState.RS;
		}

		public override void ADDI(CpuState CpuState)
		{
			throw new NotImplementedException();
		}
	}
}
