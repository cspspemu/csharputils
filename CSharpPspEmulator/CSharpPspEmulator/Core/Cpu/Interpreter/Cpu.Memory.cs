using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpPspEmulator.Core.Cpu.Interpreter
{
    public partial class InterpretedCpu : CpuBase
    {
        public override void LB(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void LH(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void LW(CpuState CpuState)
        {
            CpuState.RT = CpuState.Memory.ReadUnsigned32((uint)(CpuState.RS + CpuState.IMM));
            CpuState.AdvancePC(4);
        }

        public override void LWL(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void LWR(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void LBU(CpuState CpuState)
        {
            CpuState.RT = (uint)(0xFF & CpuState.Memory.ReadUnsigned8((uint)(CpuState.RS + CpuState.IMM)));
            CpuState.AdvancePC(4);
        }

        public override void LHU(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// SB -- Store byte
        /// Description:
        ///     The least significant byte of $t is stored at the specified address.
        /// Operation:
        ///     MEM[$s + offset] = (0xff & $t); advance_pc (4);
        /// Syntax:
        ///     sb $t, offset($s)
        /// Encoding:
        ///    1010 00ss ssst tttt iiii iiii iiii iiii
        /// </summary>
        /// <param name="CpuState"></param>
        public override void SB(CpuState CpuState)
        {
            CpuState.Memory.WriteUnsigned8((uint)(CpuState.RS + CpuState.IMM), (byte)(CpuState.RT & 0xFF));
            CpuState.AdvancePC(4);
        }

        public override void SH(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void SW(CpuState CpuState)
        {
            CpuState.Memory.WriteUnsigned32((uint)(CpuState.RS + CpuState.IMM), (uint)CpuState.RT);
            CpuState.AdvancePC(4);
        }

        public override void SWL(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void SWR(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void LL(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void SC(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void LWC1(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void SWC1(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

    }
}
