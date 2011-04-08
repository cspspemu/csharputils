using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CSharpPspEmulator.Core
{
	abstract public class CpuBase
	{
		public ExecutionDelegate Execute;

		public CpuBase()
		{
			Execute = InstructionAttribute.ProcessClass(typeof(CpuBase));
		}

		// http://code.google.com/p/pspemu/source/browse/trunk/pspemu/core/cpu/Table.d

		[Instruction("000000:rs:rt:rd:00000:100000", InstructionAttribute.AddressType.None)]
		abstract public void ADD(CpuState CpuState);

		[Instruction("000000:rs:rt:rd:00000:100001", InstructionAttribute.AddressType.None)]
		abstract public void ADDU(CpuState CpuState);

		[Instruction("001000:rs:rt:imm16"          , InstructionAttribute.AddressType.None)]
		abstract public void ADDI(CpuState CpuState);
	}
}
