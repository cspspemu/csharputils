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

        public override void ADDIU(CpuState CpuState)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public override void BGTZL(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void BNE(CpuState CpuState)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public override void LHU(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void SB(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void SH(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void SW(CpuState CpuState)
        {
            throw new NotImplementedException();
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

        public override void ADD_S(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void SUB_S(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void MUL_S(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void DIV_S(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void SQRT_S(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void ABS_S(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void MOV_S(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void NEG_S(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void ROUND_W_S(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void TRUNC_W_S(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void CEIL_W_S(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void FLOOR_W_S(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void CVT_S_W(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void CVT_W_S(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void MFC1(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void CFC1(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void MTC1(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void CTC1(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void C_F_S(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void C_UN_S(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void C_EQ_S(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void C_UEQ_S(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void C_OLT_S(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void C_ULT_S(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void C_OLE_S(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void C_ULE_S(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void C_SF_S(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void C_NGLE_S(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void C_SEQ_S(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void C_NGL_S(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void C_LT_S(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void C_NGE_S(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void C_LE_S(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void C_NGT_S(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void SYSCALL(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void CACHE(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void SYNC(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void BREAK(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void DBREAK(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void HALT(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void DRET(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void ERET(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void MFIC(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void MTIC(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void MFDR(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void MTDR(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void CFC0(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void CTC0(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void MFC0(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void MTC0(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void MFV(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void MFVC(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void MTV(CpuState CpuState)
        {
            throw new NotImplementedException();
        }

        public override void MTVC(CpuState CpuState)
        {
            throw new NotImplementedException();
        }
    }
}
