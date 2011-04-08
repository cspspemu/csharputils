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

        // Arithmetic operations.
		[Instruction("000000:rs:rt:rd:00000:100000", InstructionAttribute.AddressType.None)]
		abstract public void ADD(CpuState CpuState);

		[Instruction("000000:rs:rt:rd:00000:100001", InstructionAttribute.AddressType.None)]
		abstract public void ADDU(CpuState CpuState);

		[Instruction("001000:rs:rt:imm16"          , InstructionAttribute.AddressType.None)]
		abstract public void ADDI(CpuState CpuState);

        [Instruction("001001:rs:rt:imm16"          , InstructionAttribute.AddressType.None)]
        abstract public void ADDIU(CpuState CpuState);

        [Instruction("000000:rs:rt:rd:00000:100010", InstructionAttribute.AddressType.None)]
        abstract public void SUB(CpuState CpuState);

        [Instruction("000000:rs:rt:rd:00000:100011", InstructionAttribute.AddressType.None)]
        abstract public void SUBU(CpuState CpuState);
        
        // Logical Operations.
        [Instruction("000000:rs:rt:rd:00000:100100", InstructionAttribute.AddressType.None)]
        abstract public void AND(CpuState CpuState);

        [Instruction("001100:rs:rt:imm16"          , InstructionAttribute.AddressType.None)]
        abstract public void ANDI(CpuState CpuState);

        [Instruction("000000:rs:rt:rd:00000:100111", InstructionAttribute.AddressType.None)]
        abstract public void NOR(CpuState CpuState);

        [Instruction("000000:rs:rt:rd:00000:100101", InstructionAttribute.AddressType.None)]
        abstract public void OR(CpuState CpuState);

        [Instruction("001101:rs:rt:imm16"          , InstructionAttribute.AddressType.None)]
        abstract public void ORI(CpuState CpuState);

        [Instruction("000000:rs:rt:rd:00000:100110", InstructionAttribute.AddressType.None)]
        abstract public void XOR(CpuState CpuState);

        [Instruction("001110:rs:rt:imm16"          , InstructionAttribute.AddressType.None)]
        abstract public void XORI(CpuState CpuState);

        // Shift Left/Right Logical/Arithmethic (Variable).
        [Instruction("000000:00000:rt:rd:sa:000000", InstructionAttribute.AddressType.None)]
        abstract public void SLL(CpuState CpuState);

        [Instruction("000000:rs:rt:rd:00000:000100", InstructionAttribute.AddressType.None)]
        abstract public void SLLV(CpuState CpuState);

        [Instruction("000000:00000:rt:rd:sa:000011", InstructionAttribute.AddressType.None)]
        abstract public void SRA(CpuState CpuState);

        [Instruction("000000:rs:rt:rd:00000:000111", InstructionAttribute.AddressType.None)]
        abstract public void SRAV(CpuState CpuState);

        [Instruction("000000:00000:rt:rd:sa:000010", InstructionAttribute.AddressType.None)]
        abstract public void SRL(CpuState CpuState);

        [Instruction("000000:rs:rt:rd:00000:000110", InstructionAttribute.AddressType.None)]
        abstract public void SRLV(CpuState CpuState);

        [Instruction("000000:00001:rt:rd:sa:000010", InstructionAttribute.AddressType.None)]
        abstract public void ROTR(CpuState CpuState);

        [Instruction("000000:rs:rt:rd:00001:000110", InstructionAttribute.AddressType.None)]
        abstract public void ROTRV(CpuState CpuState);

        // Set Less Than (Immediate) (Unsigned).
        [Instruction("000000:rs:rt:rd:00000:101010", InstructionAttribute.AddressType.None)]
        abstract public void SLT(CpuState CpuState);

        [Instruction("001010:rs:rt:imm16"          , InstructionAttribute.AddressType.None)]
        abstract public void SLTI(CpuState CpuState);

        [Instruction("000000:rs:rt:rd:00000:101011", InstructionAttribute.AddressType.None)]
        abstract public void SLTU(CpuState CpuState);

        [Instruction("001011:rs:rt:imm16"          , InstructionAttribute.AddressType.None)]
        abstract public void SLTIU(CpuState CpuState);

        // Load Upper Immediate.
        [Instruction("001111:00000:rt:imm16"       ,     InstructionAttribute.AddressType.None)]
        abstract public void LUI(CpuState CpuState);

        // Sign Extend Byte/Half word.
        [Instruction("011111:00000:rt:rd:10000:100000", InstructionAttribute.AddressType.None)]
        abstract public void SEB(CpuState CpuState);
        [Instruction("011111:00000:rt:rd:11000:100000", InstructionAttribute.AddressType.None)]
        abstract public void SEH(CpuState CpuState);
        
        // BIT REVerse.
        [Instruction("011111:00000:rt:rd:10100:100000", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        abstract public void BITREV(CpuState CpuState);

        // MAXimum/MINimum.
        [Instruction("000000:rs:rt:rd:00000:101100", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        abstract public void MAX(CpuState CpuState);

        [Instruction("000000:rs:rt:rd:00000:101101", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        abstract public void MIN(CpuState CpuState);

        // DIVide (Unsigned).
        [Instruction("000000:rs:rt:00000:00000:011010", InstructionAttribute.AddressType.None)]
        abstract public void DIV(CpuState CpuState);

        [Instruction("000000:rs:rt:00000:00000:011011", InstructionAttribute.AddressType.None)]
        abstract public void DIVU(CpuState CpuState);

        // MULTiply (Unsigned).
        [Instruction("000000:rs:rt:00000:00000:011000", InstructionAttribute.AddressType.None)]
        abstract public void MULT(CpuState CpuState);

        [Instruction("000000:rs:rt:00000:00000:011001", InstructionAttribute.AddressType.None)]
        abstract public void MULTU(CpuState CpuState);

        // Multiply ADD/SUBstract (Unsigned).
        [Instruction("000000:rs:rt:00000:00000:011100", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        abstract public void MADD(CpuState CpuState);

        [Instruction("000000:rs:rt:00000:00000:011101", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        abstract public void MADDU(CpuState CpuState);

        [Instruction("000000:rs:rt:00000:00000:101110", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        abstract public void MSUB(CpuState CpuState);

        [Instruction("000000:rs:rt:00000:00000:101111", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        abstract public void MSUBU(CpuState CpuState);

        // Move To/From HI/LO.
        [Instruction("000000:00000:00000:rd:00000:010000",  InstructionAttribute.AddressType.None)]
        abstract public void MFHI(CpuState CpuState);

        [Instruction("000000:00000:00000:rd:00000:010010",  InstructionAttribute.AddressType.None)]
        abstract public void MFLO(CpuState CpuState);

        [Instruction("000000:rs:00000:00000:00000:010001",  InstructionAttribute.AddressType.None)]
        abstract public void MTHI(CpuState CpuState);

        [Instruction("000000:rs:00000:00000:00000:010011",  InstructionAttribute.AddressType.None)]
        abstract public void MTLO(CpuState CpuState);

        // Move if Zero/Non zero.
        [Instruction("000000:rs:rt:rd:00000:001010", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        abstract public void MOVZ(CpuState CpuState);

        [Instruction("000000:rs:rt:rd:00000:001011", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        abstract public void MOVN(CpuState CpuState);

        // EXTract/INSert.
        [Instruction("011111:rs:rt:msb:lsb:000000", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        abstract public void EXT(CpuState CpuState);

        [Instruction("011111:rs:rt:msb:lsb:000100", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        abstract public void INS(CpuState CpuState);

        // Count Leading Ones/Zeros in word.
        [Instruction("000000:rs:00000:rd:00000:010110", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        abstract public void CLZ(CpuState CpuState);

        [Instruction("000000:rs:00000:rd:00000:010111", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        abstract public void CLO(CpuState CpuState);

        // Word Swap Bytes Within Halfwords/Words.
        [Instruction("011111:00000:rt:rd:00010:100000", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        abstract public void WSBH(CpuState CpuState);

        [Instruction("011111:00000:rt:rd:00011:100000", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        abstract public void WSBW(CpuState CpuState);

        // Branch on EQuals (Likely).
        [Instruction("000100:rs:rt:imm16"   , InstructionAttribute.AddressType.S16,  InstructionAttribute.InstructionType.Branch)]
        abstract public void BEQ(CpuState CpuState);

        [Instruction("010100:rs:rt:imm16"   , InstructionAttribute.AddressType.S16,  InstructionAttribute.InstructionType.Branch)]
        abstract public void BEQL(CpuState CpuState);

        // Branch on Greater Equal Zero (And Link) (Likely).
        [Instruction("000001:rs:00001:imm16",     InstructionAttribute.AddressType.S16,  InstructionAttribute.InstructionType.Branch)]
        abstract public void BGEZ(CpuState CpuState);

        [Instruction("000001:rs:00011:imm16",     InstructionAttribute.AddressType.S16,  InstructionAttribute.InstructionType.Branch)]
        abstract public void BGEZL(CpuState CpuState);

        [Instruction("000001:rs:10001:imm16",     InstructionAttribute.AddressType.S16,  InstructionAttribute.InstructionType.JumpAndLink)]
        abstract public void BGEZAL(CpuState CpuState);

        [Instruction("000001:rs:10011:imm16",     InstructionAttribute.AddressType.S16,  InstructionAttribute.InstructionType.JumpAndLink)]
        abstract public void BGEZALL(CpuState CpuState);

        // Branch on Less Than Zero (And Link) (Likely).
        [Instruction("000001:rs:00000:imm16",     InstructionAttribute.AddressType.S16,  InstructionAttribute.InstructionType.Branch)]
        abstract public void BLTZ(CpuState CpuState);

        [Instruction("000001:rs:00010:imm16",     InstructionAttribute.AddressType.S16,  InstructionAttribute.InstructionType.Branch)]
        abstract public void BLTZL(CpuState CpuState);

        [Instruction("000001:rs:10000:imm16",     InstructionAttribute.AddressType.S16,  InstructionAttribute.InstructionType.JumpAndLink)]
        abstract public void BLTZAL(CpuState CpuState);

        [Instruction("000001:rs:10010:imm16",     InstructionAttribute.AddressType.S16,  InstructionAttribute.InstructionType.JumpAndLink)]
        abstract public void BLTZALL(CpuState CpuState);

        // Branch on Less Or Equals than Zero (Likely).
        [Instruction("000110:rs:00000:imm16",     InstructionAttribute.AddressType.S16,  InstructionAttribute.InstructionType.Branch)]
        abstract public void BLEZ(CpuState CpuState);

        [Instruction("010110:rs:00000:imm16",     InstructionAttribute.AddressType.S16,  InstructionAttribute.InstructionType.Branch)]
        abstract public void BLEZL(CpuState CpuState);

        // Branch on Great Than Zero (Likely).
        [Instruction("000111:rs:00000:imm16",     InstructionAttribute.AddressType.S16,  InstructionAttribute.InstructionType.Branch)]
        abstract public void BGTZ(CpuState CpuState);

        [Instruction("010111:rs:00000:imm16",     InstructionAttribute.AddressType.S16,  InstructionAttribute.InstructionType.Branch)]
        abstract public void BGTZL(CpuState CpuState);

        // Branch on Not Equals (Likely).
        [Instruction("000101:rs:rt:imm16"   , InstructionAttribute.AddressType.S16,  InstructionAttribute.InstructionType.Branch)]
        abstract public void BNE(CpuState CpuState);

        [Instruction("010101:rs:rt:imm16"   , InstructionAttribute.AddressType.S16,  InstructionAttribute.InstructionType.Branch)]
        abstract public void BNEL(CpuState CpuState);

        // Jump (And Link) (Register).
        [Instruction("000010:imm26"         ,                InstructionAttribute.AddressType.S26,  InstructionAttribute.InstructionType.Jump)]
        abstract public void J(CpuState CpuState);

        [Instruction("000000:rs:00000:00000:00000:001000",   InstructionAttribute.AddressType.Register, InstructionAttribute.InstructionType.Jump)]
        abstract public void JR(CpuState CpuState);

        [Instruction("000000:rs:00000:rd:00000:001001",  InstructionAttribute.AddressType.Register, InstructionAttribute.InstructionType.JumpAndLink)]
        abstract public void JALR(CpuState CpuState);

        [Instruction("000011:imm26"         ,                InstructionAttribute.AddressType.S26,  InstructionAttribute.InstructionType.JumpAndLink)]
        abstract public void JAL(CpuState CpuState);

        // Branch on C1 False/True (Likely).
        [Instruction("010001:01000:00000:imm16",      InstructionAttribute.AddressType.S16,  InstructionAttribute.InstructionType.Branch)]
        abstract public void BC1F(CpuState CpuState);

        [Instruction("010001:01000:00001:imm16",      InstructionAttribute.AddressType.S16,  InstructionAttribute.InstructionType.Branch)]
        abstract public void BC1T(CpuState CpuState);

        [Instruction("010001:01000:00010:imm16",      InstructionAttribute.AddressType.S16,  InstructionAttribute.InstructionType.Branch)]
        abstract public void BC1FL(CpuState CpuState);

        [Instruction("010001:01000:00011:imm16",      InstructionAttribute.AddressType.S16,  InstructionAttribute.InstructionType.Branch)]
        abstract public void BC1TL(CpuState CpuState);

        // Load Byte/Half word/Word (Left/Right/Unsigned).
        [Instruction("100000:rs:rt:imm16", InstructionAttribute.AddressType.None)]
        abstract public void LB(CpuState CpuState);

        [Instruction("100001:rs:rt:imm16", InstructionAttribute.AddressType.None)]
        abstract public void LH(CpuState CpuState);

        [Instruction("100011:rs:rt:imm16", InstructionAttribute.AddressType.None)]
        abstract public void LW(CpuState CpuState);

        [Instruction("100010:rs:rt:imm16", InstructionAttribute.AddressType.None)]
        abstract public void LWL(CpuState CpuState);

        [Instruction("100110:rs:rt:imm16", InstructionAttribute.AddressType.None)]
        abstract public void LWR(CpuState CpuState);

        [Instruction("100100:rs:rt:imm16", InstructionAttribute.AddressType.None)]
        abstract public void LBU(CpuState CpuState);

        [Instruction("100101:rs:rt:imm16", InstructionAttribute.AddressType.None)]
        abstract public void LHU(CpuState CpuState);

        // Store Byte/Half word/Word (Left/Right).
        [Instruction("101000:rs:rt:imm16", InstructionAttribute.AddressType.None)]
        abstract public void SB(CpuState CpuState);

        [Instruction("101001:rs:rt:imm16", InstructionAttribute.AddressType.None)]
        abstract public void SH(CpuState CpuState);

        [Instruction("101011:rs:rt:imm16", InstructionAttribute.AddressType.None)]
        abstract public void SW(CpuState CpuState);

        [Instruction("101010:rs:rt:imm16", InstructionAttribute.AddressType.None)]
        abstract public void SWL(CpuState CpuState);

        [Instruction("101110:rs:rt:imm16", InstructionAttribute.AddressType.None)]
        abstract public void SWR(CpuState CpuState);

        // Load Linked word.
        // Store Conditional word.
        [Instruction("110000:rs:rt:imm16", InstructionAttribute.AddressType.None)]
        abstract public void LL(CpuState CpuState);

        [Instruction("111000:rs:rt:imm16", InstructionAttribute.AddressType.None)]
        abstract public void SC(CpuState CpuState);

        // Load Word to Cop1 floating point.
        // Store Word from Cop1 floating point.
        [Instruction("110001:rs:ft:imm16", InstructionAttribute.AddressType.None)]
        abstract public void LWC1(CpuState CpuState);

        [Instruction("111001:rs:ft:imm16", InstructionAttribute.AddressType.None)]
        abstract public void SWC1(CpuState CpuState);

        // Binary Floating Point Unit Operations
        [Instruction("010001:10000:ft:fs:fd:000000"   , InstructionAttribute.AddressType.None)]
        abstract public void ADD_S(CpuState CpuState);

        [Instruction("010001:10000:ft:fs:fd:000001"   , InstructionAttribute.AddressType.None)]
        abstract public void SUB_S(CpuState CpuState);

        [Instruction("010001:10000:ft:fs:fd:000010"   , InstructionAttribute.AddressType.None)]
        abstract public void MUL_S(CpuState CpuState);

        [Instruction("010001:10000:ft:fs:fd:000011"   , InstructionAttribute.AddressType.None)]
        abstract public void DIV_S(CpuState CpuState);

        // Unary Floating Point Unit Operations
        [Instruction("010001:10000:00000:fs:fd:000100",     InstructionAttribute.AddressType.None)]
        abstract public void SQRT_S(CpuState CpuState);

        [Instruction("010001:10000:00000:fs:fd:000101",     InstructionAttribute.AddressType.None)]
        abstract public void ABS_S(CpuState CpuState);

        [Instruction("010001:10000:00000:fs:fd:000110",     InstructionAttribute.AddressType.None)]
        abstract public void MOV_S(CpuState CpuState);

        [Instruction("010001:10000:00000:fs:fd:000111",     InstructionAttribute.AddressType.None)]
        abstract public void NEG_S(CpuState CpuState);

        [Instruction("010001:10000:00000:fs:fd:001100",     InstructionAttribute.AddressType.None)]
        abstract public void ROUND_W_S(CpuState CpuState);

        [Instruction("010001:10000:00000:fs:fd:001101",     InstructionAttribute.AddressType.None)]
        abstract public void TRUNC_W_S(CpuState CpuState);

        [Instruction("010001:10000:00000:fs:fd:001110",     InstructionAttribute.AddressType.None)]
        abstract public void CEIL_W_S(CpuState CpuState);

        [Instruction("010001:10000:00000:fs:fd:001111",     InstructionAttribute.AddressType.None)]
        abstract public void FLOOR_W_S(CpuState CpuState);

        // Convert
        [Instruction("010001:10100:00000:fs:fd:100000",     InstructionAttribute.AddressType.None)]
        abstract public void CVT_S_W(CpuState CpuState);

        [Instruction("010001:10000:00000:fs:fd:100100",     InstructionAttribute.AddressType.None)]
        abstract public void CVT_W_S(CpuState CpuState);

        // Move float point registers
        [Instruction("010001:00000:rt:c1dr:00000:000000",   InstructionAttribute.AddressType.None)]
        abstract public void MFC1(CpuState CpuState);

        [Instruction("010001:00010:rt:c1cr:00000:000000",   InstructionAttribute.AddressType.None)]
        abstract public void CFC1(CpuState CpuState);

        [Instruction("010001:00100:rt:c1dr:00000:000000",   InstructionAttribute.AddressType.None)]
        abstract public void MTC1(CpuState CpuState);

        [Instruction("010001:00110:rt:c1cr:00000:000000",   InstructionAttribute.AddressType.None)]
        abstract public void CTC1(CpuState CpuState);

        // Compare <condition> Single.
        [Instruction("010001:10000:ft:fs:00000:11:0000",    InstructionAttribute.AddressType.None)]
        abstract public void C_F_S(CpuState CpuState);

        [Instruction("010001:10000:ft:fs:00000:11:0001",    InstructionAttribute.AddressType.None)]
        abstract public void C_UN_S(CpuState CpuState);

        [Instruction("010001:10000:ft:fs:00000:11:0010",    InstructionAttribute.AddressType.None)]
        abstract public void C_EQ_S(CpuState CpuState);

        [Instruction("010001:10000:ft:fs:00000:11:0011",    InstructionAttribute.AddressType.None)]
        abstract public void C_UEQ_S(CpuState CpuState);

        [Instruction("010001:10000:ft:fs:00000:11:0100",    InstructionAttribute.AddressType.None)]
        abstract public void C_OLT_S(CpuState CpuState);

        [Instruction("010001:10000:ft:fs:00000:11:0101",    InstructionAttribute.AddressType.None)]
        abstract public void C_ULT_S(CpuState CpuState);

        [Instruction("010001:10000:ft:fs:00000:11:0110",    InstructionAttribute.AddressType.None)]
        abstract public void C_OLE_S(CpuState CpuState);

        [Instruction("010001:10000:ft:fs:00000:11:0111",    InstructionAttribute.AddressType.None)]
        abstract public void C_ULE_S(CpuState CpuState);

        [Instruction("010001:10000:ft:fs:00000:11:1000",    InstructionAttribute.AddressType.None)]
        abstract public void C_SF_S(CpuState CpuState);

        [Instruction("010001:10000:ft:fs:00000:11:1001",    InstructionAttribute.AddressType.None)]
        abstract public void C_NGLE_S(CpuState CpuState);

        [Instruction("010001:10000:ft:fs:00000:11:1010",    InstructionAttribute.AddressType.None)]
        abstract public void C_SEQ_S(CpuState CpuState);

        [Instruction("010001:10000:ft:fs:00000:11:1011",    InstructionAttribute.AddressType.None)]
        abstract public void C_NGL_S(CpuState CpuState);

        [Instruction("010001:10000:ft:fs:00000:11:1100",    InstructionAttribute.AddressType.None)]
        abstract public void C_LT_S(CpuState CpuState);

        [Instruction("010001:10000:ft:fs:00000:11:1101",    InstructionAttribute.AddressType.None)]
        abstract public void C_NGE_S(CpuState CpuState);

        [Instruction("010001:10000:ft:fs:00000:11:1110",    InstructionAttribute.AddressType.None)]
        abstract public void C_LE_S(CpuState CpuState);

        [Instruction("010001:10000:ft:fs:00000:11:1111",    InstructionAttribute.AddressType.None)]
        abstract public void C_NGT_S(CpuState CpuState);

        // Syscall
        [Instruction("000000:imm20:001100" ,     InstructionAttribute.AddressType.None)]
        abstract public void SYSCALL(CpuState CpuState);

        [Instruction("101111--------------------------", InstructionAttribute.AddressType.None)]
        abstract public void CACHE(CpuState CpuState);

        [Instruction("000000:00000:00000:00000:00000:001111", InstructionAttribute.AddressType.None)]
        abstract public void SYNC(CpuState CpuState);

        [Instruction("000000:imm20:001101"                  ,  InstructionAttribute.AddressType.None)]
        abstract public void BREAK(CpuState CpuState);

        [Instruction("011100:00000:00000:00000:00000:111111", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        abstract public void DBREAK(CpuState CpuState);

        [Instruction("011100:00000:00000:00000:00000:000000", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        abstract public void HALT(CpuState CpuState);

        // (D?/Exception) RETurn
        [Instruction("011100:00000:00000:00000:00000:111110", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        abstract public void DRET(CpuState CpuState);

        [Instruction("010000:10000:00000:00000:00000:011000", InstructionAttribute.AddressType.None)]
        abstract public void ERET(CpuState CpuState);

        // Move (From/To) IC
        [Instruction("011100:rt:00000:00000:00000:100100", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        abstract public void MFIC(CpuState CpuState);

        [Instruction("011100:rt:00000:00000:00000:100110", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        abstract public void MTIC(CpuState CpuState);

        // Move (From/To) DR
        [Instruction("011100:00000:----------:00000:111101", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        abstract public void MFDR(CpuState CpuState);

        [Instruction("011100:00100:----------:00000:111101", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        abstract public void MTDR(CpuState CpuState);

        // C? (From/To) Cop0
        [Instruction("010000:00010:----------:00000:000000", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)] // CFC0(010000:00010:rt:c0cr:00000:000000)
        abstract public void CFC0(CpuState CpuState);

        [Instruction("010000:00110:----------:00000:000000", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)] // CTC0(010000:00110:rt:c0cr:00000:000000)
        abstract public void CTC0(CpuState CpuState);

        // Move (From/To) Cop0
        [Instruction("010000:00000:----------:00000:000000", InstructionAttribute.AddressType.None)]              // MFC0(010000:00000:rt:c0dr:00000:000000)
        abstract public void MFC0(CpuState CpuState);

        [Instruction("010000:00100:----------:00000:000000", InstructionAttribute.AddressType.None)]              // MTC0(010000:00100:rt:c0dr:00000:000000)
        abstract public void MTC0(CpuState CpuState);

        // Move From/to Vfpu (C?).
        [Instruction("010010:00:011:rt:0:0000000:0:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        abstract public void MFV(CpuState CpuState);

        [Instruction("010010:00:011:rt:0:0000000:1:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        abstract public void MFVC(CpuState CpuState);

        [Instruction("010010:00:111:rt:0:0000000:0:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        abstract public void MTV(CpuState CpuState);

        [Instruction("010010:00:111:rt:0:0000000:1:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        abstract public void MTVC(CpuState CpuState);

        /*
        // Load/Store Vfpu (Left/Right).
        ID("lv.s",        VM("110010:rs:vt5:imm14:vt2", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("lv.q",        VM("110110:rs:vt5:imm14:0:vt1", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("lvl.q",       VM("110101:rs:vt5:imm14:0:vt1", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("lvr.q",       VM("110101:rs:vt5:imm14:1:vt1", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("sv.q",        VM("111110:rs:vt5:imm14:0:vt1", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]

        // Vfpu DOT product
        // Vfpu SCaLe/ROTate
        ID("vdot",        VM("011001:001:vt:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vscl",        VM("011001:010:vt:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vslt",        VM("011011:100:vt:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vsge",        VM("011011:110:vt:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]

        // ROTate
        ID("vrot",        VM("111100:111:01:imm5:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]

        // Vfpu ZERO/ONE
        ID("vzero",       VM("110100:00:000:0:0110:two:0000000:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vone",        VM("110100:00:000:0:0111:two:0000000:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]

        // Vfpu MOVe/SiGN/Reverse SQuare root/COSine/Arc SINe/LOG2
        ID("vmov",        VM("110100:00:000:0:0000:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vabs",        VM("110100:00:000:0:0001:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vneg",        VM("110100:00:000:0:0010:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vocp",        VM("110100:00:010:0:0100:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vsgn",        VM("110100:00:010:0:1010:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vrcp",        VM("110100:00:000:1:0000:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vrsq",        VM("110100:00:000:1:0001:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vsin",        VM("110100:00:000:1:0010:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vcos",        VM("110100:00:000:1:0011:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vexp2",       VM("110100:00:000:1:0100:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vlog2",       VM("110100:00:000:1:0101:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vsqrt",       VM("110100:00:000:1:0110:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vasin",       VM("110100:00:000:1:0111:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vnrcp",       VM("110100:00:000:1:1000:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vnsin",       VM("110100:00:000:1:1010:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vrexp2",      VM("110100:00:000:1:1100:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]

        ID("vsat0",       VM("110100:00:000:0:0100:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vsat1",       VM("110100:00:000:0:0101:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]

        // Vfpu ConSTant
        ID("vcst",        VM("110100:00:011:imm5:two:0000000:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]

        // Vfpu Matrix MULtiplication
        ID("vmmul",       VM("111100:000:vt:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]

        // -
        ID("vhdp",        VM("011001:100:vt:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vcrs.t",      VM("011001:101:vt:1:vs:0:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vcrsp.t",     VM("111100:101:vt:1:vs:0:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]

        // Vfpu Integer to(2) Color
        ID("vi2c",        VM("110100:00:001:11:101:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vi2uc",       VM("110100:00:001:11:100:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]

        // -
        ID("vtfm2",       VM("111100:001:vt:0:vs:1:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vtfm3",       VM("111100:010:vt:1:vs:0:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vtfm4",       VM("111100:011:vt:1:vs:1:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]

        ID("vhtfm2",      VM("111100:001:vt:0:vs:0:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vhtfm3",      VM("111100:010:vt:0:vs:1:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vhtfm4",      VM("111100:011:vt:1:vs:0:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]

        ID("vsrt3",       VM("110100:00:010:01000:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]

        ID("vfad",        VM("110100:00:010:00110:two:vs:one:vd",      InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]

        // Vfpu MINimum/MAXium/ADD/SUB/DIV/MUL
        ID("vmin",        VM("011011:010:vt:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vmax",        VM("011011:011:vt:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vadd",        VM("011000:000:vt:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vsub",        VM("011000:001:vt:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vdiv",        VM("011000:111:vt:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vmul",        VM("011001:000:vt:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]

        // Vfpu (Matrix) IDenTity
        ID("vidt",        VM("110100:00:000:0:0011:two:0000000:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vmidt",       VM("111100:111:00:00011:two:0000000:one:vd",  InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]

        ID("viim",        VM("110111:11:0:vd:imm16", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]

        ID("vmmov",       VM("111100:111:00:00000:two:vs:one:vd",      InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vmzero",      VM("111100:111:00:00110:two:0000000:one:vd",      InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vmone",       VM("111100:111:00:00111:two:0000000:one:vd",           InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]

        ID("vnop",        VM("111111:1111111111:00000:00000000000"), "",         InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vsync",       VM("111111:1111111111:00000:01100100000"), "", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vflush",      VM("111111:1111111111:00000:10000001101"), "",              InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]

        ID("vpfxd",       VM("110111:10:------------:mskw:mskz:msky:mskx:satw:satz:saty:satx"), "[%vp4, %vp5, %vp6, %vp7]", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vpfxs",       VM("110111:00:----:negw:negz:negy:negx:cstw:cstz:csty:cstx:absw:absz:absy:absx:swzw:swzz:swzy:swzx"), "[%vp0, %vp1, %vp2, %vp3]", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vpfxt",       VM("110111:01:----:negw:negz:negy:negx:cstw:cstz:csty:cstx:absw:absz:absy:absx:swzw:swzz:swzy:swzx"), "[%vp0, %vp1, %vp2, %vp3]", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]

        ID("vdet",        VM("011001:110:vt:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]

        ID("vrnds",       VM("110100:00:001:00:000:two:vs:one:0000000", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vrndi",       VM("110100:00:001:00:001:two:0000000:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vrndf1",      VM("110100:00:001:00:010:two:0000000:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vrndf2",      VM("110100:00:001:00:011:two:0000000:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]

        ////////////////////////////
        /// Not implemented yet!
        ////////////////////////////
        ID("vcmp",        VM("011011:000:vt:two:vs:one:0000:imm3" , InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]

        ID("vcmovf",      VM("110100:10:101:01:imm3:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vcmovt",      VM("110100:10:101:00:imm3:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]

        ID("bvf",         VM("010010:01:000:imm3:00:imm16", InstructionAttribute.AddressType.S16, InstructionAttribute.InstructionType.PSP | InstructionAttribute.InstructionType.Branch)]
        ID("bvfl",        VM("010010:01:000:imm3:10:imm16", InstructionAttribute.AddressType.S16, InstructionAttribute.InstructionType.PSP | InstructionAttribute.InstructionType.Branch)]
        ID("bvt",         VM("010010:01:000:imm3:01:imm16", InstructionAttribute.AddressType.S16, InstructionAttribute.InstructionType.PSP | InstructionAttribute.InstructionType.Branch)]
        ID("bvtl",        VM("010010:01:000:imm3:11:imm16", InstructionAttribute.AddressType.S16, InstructionAttribute.InstructionType.PSP | InstructionAttribute.InstructionType.Branch)]

        ID("vavg",        VM("110100:00:010:00111:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vf2id",       VM("110100:10:011:imm5:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vf2in",       VM("110100:10:000:imm5:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vf2iu",       VM("110100:10:010:imm5:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vf2iz",       VM("110100:10:001:imm5:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vi2f",        VM("110100:10:100:imm5:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]

        ID("vscmp",       VM("011011:101:vt:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vmscl",       VM("111100:100:vt:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]

        ID("vt4444.q",    VM("110100:00:010:11001:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vt5551.q",    VM("110100:00:010:11010:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vt5650.q",    VM("110100:00:010:11011:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]

        ID("vmfvc",       VM("110100:00:010:10000:1:imm7:0:vd",      InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vmtvc",       VM("110100:00:010:10001:0:vs:1:imm7",      InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]

        ID("mfvme",       VM("011010--------------------------", InstructionAttribute.AddressType.None)]
        ID("mtvme",       VM("101100--------------------------", InstructionAttribute.AddressType.None)]

        ID("sv.s",        VM("111010:rs:vt5:imm14:vt2", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("svl.q",       VM("111101:rs:vt5:imm14:0:vt1", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("svr.q",       VM("111101:rs:vt5:imm14:1:vt1", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vbfy1",       VM("110100:00:010:00010:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vbfy2",       VM("110100:00:010:00011:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vfim",        VM("110111:11:1:vd:imm16",      InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]

        ID("vf2h",        VM("110100:00:001:10:010:two:vs:one:vd",      InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vh2f",        VM("110100:00:001:10:011:two:vs:one:vd",      InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vi2s",        VM("110100:00:001:11:111:two:vs:one:vd",      InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vi2us",       VM("110100:00:001:11:110:two:vs:one:vd",      InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vidt",        VM("110100:00:000:0:0011:two:0000000:one:vd",           InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]

        ID("vlgb",        VM("110100:00:001:10:111:two:vs:one:vd",      InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vqmul",       VM("111100:101:vt:1:vs:1:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vs2i",        VM("110100:00:001:11:011:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vsbn",        VM("011000:010:vt:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vsbz",        VM("110100:00:001:10:110:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vsocp",       VM("110100:00:010:00101:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vsrt1",       VM("110100:00:010:00000:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vsrt2",       VM("110100:00:010:00001:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vsrt4",       VM("110100:00:010:01001:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vus2i",       VM("110100:00:001:11:010:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vwb.q",       VM("111110------------------------1-", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        ID("vwbn",        VM("110100:11:imm8:two:vs:one:vd", InstructionAttribute.AddressType.None, InstructionAttribute.InstructionType.PSP)]
        */
	}
}
