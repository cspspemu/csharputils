using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpPspEmulator.Core.Cpu
{
    public partial class Cpu : CpuBase
    {
        public override void ADD(CpuState CpuState)
        {
            CpuState.RT = CpuState.RD + CpuState.RS;
            CpuState.AdvancePC(4);
        }

        public override void ADDU(CpuState CpuState)
        {
            CpuState.RT = CpuState.RD + CpuState.RS;
            CpuState.AdvancePC(4);
        }

        public override void ADDI(CpuState CpuState)
        {
            CpuState.RT = CpuState.RD + CpuState.IMMU;
            CpuState.AdvancePC(4);
        }

        public override void ADDIU(CpuState CpuState)
        {
            CpuState.RT = CpuState.RD + CpuState.IMMU;
            CpuState.AdvancePC(4);
        }

        public override void SUB(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void SUBU(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void AND(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void ANDI(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void NOR(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void OR(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void ORI(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void XOR(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void XORI(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void SLL(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void SLLV(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void SRA(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void SRAV(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void SRL(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void SRLV(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void ROTR(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void ROTRV(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void SLT(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void SLTI(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void SLTU(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void SLTIU(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void LUI(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void SEB(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void SEH(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void BITREV(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void MAX(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void MIN(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void DIV(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void DIVU(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void MULT(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void MULTU(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void MADD(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void MADDU(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void MSUB(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void MSUBU(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void MFHI(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void MFLO(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void MTHI(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void MTLO(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void MOVZ(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void MOVN(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void EXT(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void INS(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void CLZ(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void CLO(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void WSBH(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void WSBW(CpuState CpuState)
        {
            throw new NotImplementedException();
        }
    }
}
