using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpPspEmulator.Core.Cpu.Assembler
{
    public class Disassembler
    {
        ExecutionDelegate ExecutionDelegate;

        public Disassembler()
        {
            /*ExecutionDelegate = InstructionAttribute.GetExecutor(typeof(CpuBase), InstructionAttribute => delegate(CpuState CpuState)
            {
                InstructionAttribute.MethodInfo.Invoke(CpuState.CpuBase, new object[] { CpuState });
            }, delegate(CpuState CpuState)
            {
                //Type.GetMethod("INVALID").Invoke(CpuState.CpuBase, new object[] { CpuState });
            });*/
        }
    }
}
