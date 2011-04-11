using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpPspEmulator.Core.Cpu.Interpreter
{
    /// <summary>
    /// http://code.google.com/p/pspemu/source/browse/trunk/pspemu/core/cpu/interpreted/ops/Branch.d
    /// </summary>
    public partial class InterpretedCpu : CpuBase 
    {
        protected void Branch(CpuState CpuState, bool Result, bool Likely = false, bool Link = false)
        {
            if (Result)
            {
                if (Link)
                {
                    CpuState.RegistersCpu[31] = CpuState.RegistersCpu.nPC + 4;
                }
                CpuState.AdvancePC(CpuState.OFFSET2);
            }
            else
            {
                if (Likely)
                {
                    CpuState.RegistersCpu.PC = CpuState.RegistersCpu.nPC + 4;
                    CpuState.RegistersCpu.nPC = CpuState.RegistersCpu.PC + 4;
                }
                else
                {
                    CpuState.AdvancePC(4);
                }
            }
        }

        public override void BEQ(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void BEQL(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void BGEZ(CpuState CpuState)
        {
            Branch(CpuState, ((int)CpuState.RS) >= 0, Likely: false, Link: false);
        }

        public override void BGEZL(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void BGEZAL(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void BGEZALL(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void BLTZ(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void BLTZL(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void BLTZAL(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void BLTZALL(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void BLEZ(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void BLEZL(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void BGTZ(CpuState CpuState)
        {
            Branch(CpuState, ((int)CpuState.RS) > 0, Likely: false, Link: false);
        }

        public override void BGTZL(CpuState CpuState)
        {
            Branch(CpuState, ((int)CpuState.RS) > 0, Likely: true, Link: false);
        }

        public override void BNE(CpuState CpuState)
        {
            Branch(CpuState, CpuState.RS != CpuState.RT);
        }

        public override void BNEL(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void J(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void JR(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void JALR(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void JAL(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void BC1F(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void BC1T(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void BC1FL(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void BC1TL(CpuState CpuState)
        {
            throw new NotImplementedException();
        }
    }
}
