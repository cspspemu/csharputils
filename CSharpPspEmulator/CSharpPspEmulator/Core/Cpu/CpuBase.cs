using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CSharpPspEmulator.Core.Cpu
{
    /// <summary>
    /// Principle: DRY - Don't Repeat Yourself
    /// 
    /// This class describes all the instructions available on the Allegrex CPU.
    /// It describes the name of the instructions, its assembler format and the instruction encoding.
    /// This information can be used to create an assembler, a disassembler, and runtime tables for
    /// an interpreter or for a JIT.
    /// 
    /// It's based on the Table I have made for the DPSPEmulator, that is based on jpcsp Allegrex.isa
    /// and ps2dev disasm.
    /// http://code.google.com/p/pspemu/source/browse/trunk/pspemu/core/cpu/Table.d
    /// 
    /// http://jpcsp.googlecode.com/svn/trunk/src/jpcsp/Allegrex.isa 
    /// http://svn.ps2dev.org/filedetails.php?repname=psp&path=/trunk/prxtool/disasm.C&rev=0&sc=0 
    /// 
    /// - Soywiz
    /// </summary>
	abstract public class CpuBase
	{
		public ExecutionDelegate Execute;

		public CpuBase()
		{
			Execute = InstructionAttribute.GetExecutor(typeof(CpuBase));
		}

        /*static public String AssemblerFormat_RType(String Name)
        {
            return Name + " {$d},{$s},{$t}";
        }*/

        #region ALU
        // Arithmetic operations.
        [Instruction(Name: "add", Format: "000000:rs:rt:rd:00000:100000", AssemblerFormat: "{$d},{$s},{$t}")]
		abstract public void ADD(CpuState CpuState);

        [Instruction(Name: "addu", Format: "000000:rs:rt:rd:00000:100001", AssemblerFormat: "{$d},{$s},{$t}")]
		abstract public void ADDU(CpuState CpuState);

        [Instruction(Name:"addi", Format: "001000:rs:rt:imm16", AssemblerFormat: "{$t},{$s},{$imm}")]
		abstract public void ADDI(CpuState CpuState);

        [Instruction(Name: "addiu", Format: "001001:rs:rt:imm16", AssemblerFormat: "{$t},{$s},{$immu}")]
        abstract public void ADDIU(CpuState CpuState);

        [Instruction(Name: "sub", Format: "000000:rs:rt:rd:00000:100010", AssemblerFormat: "{$d},{$s},{$t}")]
        abstract public void SUB(CpuState CpuState);

        [Instruction(Name: "subu", Format: "000000:rs:rt:rd:00000:100011", AssemblerFormat: "{$d},{$s},{$t}")]
        abstract public void SUBU(CpuState CpuState);
        
        // Logical Operations.
        [Instruction(Name: "and", Format: "000000:rs:rt:rd:00000:100100", AssemblerFormat: "{$d},{$s},{$t}")]
        abstract public void AND(CpuState CpuState);

        [Instruction(Name: "andi", Format: "001100:rs:rt:imm16", AssemblerFormat: "{$t},{$s},{$immu}")]
        abstract public void ANDI(CpuState CpuState);

        [Instruction(Name: "nor", Format: "000000:rs:rt:rd:00000:100111", AssemblerFormat: "{$d},{$s},{$t}")]
        abstract public void NOR(CpuState CpuState);

        [Instruction(Name: "or", Format: "000000:rs:rt:rd:00000:100101", AssemblerFormat: "{$d},{$s},{$t}")]
        abstract public void OR(CpuState CpuState);

        [Instruction(Name: "ori", Format: "001101:rs:rt:imm16", AssemblerFormat: "{$t},{$s},{$immu}")]
        abstract public void ORI(CpuState CpuState);

        [Instruction(Name: "xor", Format: "000000:rs:rt:rd:00000:100110", AssemblerFormat: "{$d},{$s},{$t}")]
        abstract public void XOR(CpuState CpuState);

        [Instruction(Name: "xori", Format: "001110:rs:rt:imm16", AssemblerFormat: "{$t},{$s},{$immu}")]
        abstract public void XORI(CpuState CpuState);

        // Shift Left/Right Logical/Arithmethic (Variable).
        [Instruction(Name: "sll", Format: "000000:00000:rt:rd:sa:000000", AssemblerFormat: "{$d},{$t},{$h}")]
        abstract public void SLL(CpuState CpuState);

        [Instruction(Name: "sllv", Format: "000000:rs:rt:rd:00000:000100")]
        abstract public void SLLV(CpuState CpuState);

        [Instruction(Name: "sra", Format: "000000:00000:rt:rd:sa:000011")]
        abstract public void SRA(CpuState CpuState);

        [Instruction(Name: "srav", Format: "000000:rs:rt:rd:00000:000111")]
        abstract public void SRAV(CpuState CpuState);

        [Instruction(Name: "srl", Format: "000000:00000:rt:rd:sa:000010")]
        abstract public void SRL(CpuState CpuState);

        [Instruction(Name: "srlv", Format: "000000:rs:rt:rd:00000:000110")]
        abstract public void SRLV(CpuState CpuState);

        [Instruction(Name: "rotr", Format: "000000:00001:rt:rd:sa:000010")]
        abstract public void ROTR(CpuState CpuState);

        [Instruction(Name: "rotrv", Format: "000000:rs:rt:rd:00001:000110")]
        abstract public void ROTRV(CpuState CpuState);

        // Set Less Than (Immediate) (Unsigned).
        [Instruction(Name: "slt", Format: "000000:rs:rt:rd:00000:101010")]
        abstract public void SLT(CpuState CpuState);

        [Instruction(Name: "slti", Format: "001010:rs:rt:imm16")]
        abstract public void SLTI(CpuState CpuState);

        [Instruction(Name: "sltu", Format: "000000:rs:rt:rd:00000:101011")]
        abstract public void SLTU(CpuState CpuState);

        [Instruction(Name: "sltiu", Format: "001011:rs:rt:imm16")]
        abstract public void SLTIU(CpuState CpuState);

        // Load Upper Immediate.
        [Instruction(Name: "lui", Format: "001111:00000:rt:imm16", AssemblerFormat: "{$t},{$immu}", _AddressType: InstructionAttribute.AddressType.None)]
        abstract public void LUI(CpuState CpuState);

        // Sign Extend Byte/Half word.
        [Instruction(Name: "seb", Format: "011111:00000:rt:rd:10000:100000")]
        abstract public void SEB(CpuState CpuState);

        [Instruction(Name: "seh", Format: "011111:00000:rt:rd:11000:100000")]
        abstract public void SEH(CpuState CpuState);
        
        // BIT REVerse.
        [Instruction(Name: "bitrev", Format: "011111:00000:rt:rd:10100:100000", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void BITREV(CpuState CpuState);

        // MAXimum/MINimum.
        [Instruction(Name: "max", Format: "000000:rs:rt:rd:00000:101100", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void MAX(CpuState CpuState);

        [Instruction(Name: "min", Format: "000000:rs:rt:rd:00000:101101", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void MIN(CpuState CpuState);

        // DIVide (Unsigned).
        [Instruction(Name: "div", Format: "000000:rs:rt:00000:00000:011010")]
        abstract public void DIV(CpuState CpuState);

        [Instruction(Name: "divu", Format: "000000:rs:rt:00000:00000:011011")]
        abstract public void DIVU(CpuState CpuState);

        // MULTiply (Unsigned).
        [Instruction(Name: "mult", Format: "000000:rs:rt:00000:00000:011000")]
        abstract public void MULT(CpuState CpuState);

        [Instruction(Name: "multu", Format: "000000:rs:rt:00000:00000:011001")]
        abstract public void MULTU(CpuState CpuState);

        // Multiply ADD/SUBstract (Unsigned).
        [Instruction(Name: "madd", Format: "000000:rs:rt:00000:00000:011100", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void MADD(CpuState CpuState);

        [Instruction(Name: "maddu", Format: "000000:rs:rt:00000:00000:011101", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void MADDU(CpuState CpuState);

        [Instruction(Name: "msub", Format: "000000:rs:rt:00000:00000:101110", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void MSUB(CpuState CpuState);

        [Instruction(Name: "msubu", Format: "000000:rs:rt:00000:00000:101111", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void MSUBU(CpuState CpuState);

        // Move To/From HI/LO.
        [Instruction(Name: "mfhi", Format: "000000:00000:00000:rd:00000:010000")]
        abstract public void MFHI(CpuState CpuState);

        [Instruction(Name: "mflo", Format: "000000:00000:00000:rd:00000:010010")]
        abstract public void MFLO(CpuState CpuState);

        [Instruction(Name: "mthi", Format: "000000:rs:00000:00000:00000:010001")]
        abstract public void MTHI(CpuState CpuState);

        [Instruction(Name: "mtlo", Format: "000000:rs:00000:00000:00000:010011")]
        abstract public void MTLO(CpuState CpuState);

        // Move if Zero/Non zero.
        [Instruction(Name: "movz", Format: "000000:rs:rt:rd:00000:001010", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void MOVZ(CpuState CpuState);

        [Instruction(Name: "movn", Format: "000000:rs:rt:rd:00000:001011", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void MOVN(CpuState CpuState);

        // EXTract/INSert.
        [Instruction(Name: "ext", Format: "011111:rs:rt:msb:lsb:000000", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void EXT(CpuState CpuState);

        [Instruction(Name: "ins", Format: "011111:rs:rt:msb:lsb:000100", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void INS(CpuState CpuState);

        // Count Leading Ones/Zeros in word.
        [Instruction(Name: "clz", Format: "000000:rs:00000:rd:00000:010110", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void CLZ(CpuState CpuState);

        [Instruction(Name: "clo", Format: "000000:rs:00000:rd:00000:010111", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void CLO(CpuState CpuState);

        // Word Swap Bytes Within Halfwords/Words.
        [Instruction(Name: "wsbh", Format: "011111:00000:rt:rd:00010:100000", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void WSBH(CpuState CpuState);

        [Instruction(Name: "wsbw", Format: "011111:00000:rt:rd:00011:100000", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void WSBW(CpuState CpuState);

        #endregion
        
        #region Branching
        // Branch on EQuals (Likely).
        [Instruction(Name: "beq", Format: "000100:rs:rt:imm16", _AddressType: InstructionAttribute.AddressType.S16, _InstructionType: InstructionAttribute.InstructionType.Branch)]
        abstract public void BEQ(CpuState CpuState);

        [Instruction(Name: "beql", Format: "010100:rs:rt:imm16", _AddressType: InstructionAttribute.AddressType.S16, _InstructionType: InstructionAttribute.InstructionType.Branch)]
        abstract public void BEQL(CpuState CpuState);

        // Branch on Greater Equal Zero (And Link) (Likely).
        [Instruction(Name: "bgez", Format: "000001:rs:00001:imm16", AssemblerFormat:"{$s},{$offset2}", _AddressType: InstructionAttribute.AddressType.S16, _InstructionType: InstructionAttribute.InstructionType.Branch)]
        abstract public void BGEZ(CpuState CpuState);

        [Instruction(Name: "bgezl", Format: "000001:rs:00011:imm16", _AddressType: InstructionAttribute.AddressType.S16, _InstructionType: InstructionAttribute.InstructionType.Branch)]
        abstract public void BGEZL(CpuState CpuState);

        [Instruction(Name: "bgezal", Format: "000001:rs:10001:imm16", _AddressType: InstructionAttribute.AddressType.S16, _InstructionType: InstructionAttribute.InstructionType.JumpAndLink)]
        abstract public void BGEZAL(CpuState CpuState);

        [Instruction(Name: "bgezall", Format: "000001:rs:10011:imm16", _AddressType: InstructionAttribute.AddressType.S16, _InstructionType: InstructionAttribute.InstructionType.JumpAndLink)]
        abstract public void BGEZALL(CpuState CpuState);

        // Branch on Less Than Zero (And Link) (Likely).
        [Instruction(Name: "bltz", Format: "000001:rs:00000:imm16", _AddressType: InstructionAttribute.AddressType.S16, _InstructionType: InstructionAttribute.InstructionType.Branch)]
        abstract public void BLTZ(CpuState CpuState);

        [Instruction(Name: "bltzl", Format: "000001:rs:00010:imm16", _AddressType: InstructionAttribute.AddressType.S16, _InstructionType: InstructionAttribute.InstructionType.Branch)]
        abstract public void BLTZL(CpuState CpuState);

        [Instruction(Name: "bltzal", Format: "000001:rs:10000:imm16", _AddressType: InstructionAttribute.AddressType.S16, _InstructionType: InstructionAttribute.InstructionType.JumpAndLink)]
        abstract public void BLTZAL(CpuState CpuState);

        [Instruction(Name: "bltzall", Format: "000001:rs:10010:imm16", _AddressType: InstructionAttribute.AddressType.S16, _InstructionType: InstructionAttribute.InstructionType.JumpAndLink)]
        abstract public void BLTZALL(CpuState CpuState);

        // Branch on Less Or Equals than Zero (Likely).
        [Instruction(Name: "blez", Format: "000110:rs:00000:imm16", _AddressType: InstructionAttribute.AddressType.S16, _InstructionType: InstructionAttribute.InstructionType.Branch)]
        abstract public void BLEZ(CpuState CpuState);

        [Instruction(Name: "blezl", Format: "010110:rs:00000:imm16", _AddressType: InstructionAttribute.AddressType.S16, _InstructionType: InstructionAttribute.InstructionType.Branch)]
        abstract public void BLEZL(CpuState CpuState);

        // Branch on Great Than Zero (Likely).
        [Instruction(Name: "bgtz", Format: "000111:rs:00000:imm16", _AddressType: InstructionAttribute.AddressType.S16, _InstructionType: InstructionAttribute.InstructionType.Branch)]
        abstract public void BGTZ(CpuState CpuState);

        [Instruction(Name: "bgtzl", Format: "010111:rs:00000:imm16", _AddressType: InstructionAttribute.AddressType.S16, _InstructionType: InstructionAttribute.InstructionType.Branch)]
        abstract public void BGTZL(CpuState CpuState);

        // Branch on Not Equals (Likely).
        [Instruction(Name: "bne", Format: "000101:rs:rt:imm16", _AddressType: InstructionAttribute.AddressType.S16, _InstructionType: InstructionAttribute.InstructionType.Branch)]
        abstract public void BNE(CpuState CpuState);

        [Instruction(Name: "bnel", Format: "010101:rs:rt:imm16", _AddressType: InstructionAttribute.AddressType.S16, _InstructionType: InstructionAttribute.InstructionType.Branch)]
        abstract public void BNEL(CpuState CpuState);

        // Jump (And Link) (Register).
        [Instruction(Name: "j", Format: "000010:imm26", _AddressType: InstructionAttribute.AddressType.S26, _InstructionType: InstructionAttribute.InstructionType.Jump)]
        abstract public void J(CpuState CpuState);

        [Instruction(Name: "jr", Format: "000000:rs:00000:00000:00000:001000", _AddressType: InstructionAttribute.AddressType.Register, _InstructionType: InstructionAttribute.InstructionType.Jump)]
        abstract public void JR(CpuState CpuState);

        [Instruction(Name: "jalr", Format: "000000:rs:00000:rd:00000:001001", _AddressType: InstructionAttribute.AddressType.Register, _InstructionType: InstructionAttribute.InstructionType.JumpAndLink)]
        abstract public void JALR(CpuState CpuState);

        [Instruction(Name: "jal", Format: "000011:imm26", _AddressType: InstructionAttribute.AddressType.S26, _InstructionType: InstructionAttribute.InstructionType.JumpAndLink)]
        abstract public void JAL(CpuState CpuState);

        // Branch on C1 False/True (Likely).
        [Instruction(Name: "bc1f", Format: "010001:01000:00000:imm16", _AddressType: InstructionAttribute.AddressType.S16, _InstructionType: InstructionAttribute.InstructionType.Branch)]
        abstract public void BC1F(CpuState CpuState);

        [Instruction(Name: "bc1t", Format: "010001:01000:00001:imm16", _AddressType: InstructionAttribute.AddressType.S16, _InstructionType: InstructionAttribute.InstructionType.Branch)]
        abstract public void BC1T(CpuState CpuState);

        [Instruction(Name: "bc1fl", Format: "010001:01000:00010:imm16", _AddressType: InstructionAttribute.AddressType.S16, _InstructionType: InstructionAttribute.InstructionType.Branch)]
        abstract public void BC1FL(CpuState CpuState);

        [Instruction(Name: "bc1tl", Format: "010001:01000:00011:imm16", _AddressType: InstructionAttribute.AddressType.S16, _InstructionType: InstructionAttribute.InstructionType.Branch)]
        abstract public void BC1TL(CpuState CpuState);

        #endregion

        #region Memory
        // Load Byte/Half word/Word (Left/Right/Unsigned).
        [Instruction(Name: "lb", Format: "100000:rs:rt:imm16")]
        abstract public void LB(CpuState CpuState);

        [Instruction(Name: "lh", Format: "100001:rs:rt:imm16")]
        abstract public void LH(CpuState CpuState);

        [Instruction(Name: "lw", Format: "100011:rs:rt:imm16")]
        abstract public void LW(CpuState CpuState);

        [Instruction(Name: "lwl", Format: "100010:rs:rt:imm16")]
        abstract public void LWL(CpuState CpuState);

        [Instruction(Name: "lwr", Format: "100110:rs:rt:imm16")]
        abstract public void LWR(CpuState CpuState);

        [Instruction(Name: "lbu", Format: "100100:rs:rt:imm16")]
        abstract public void LBU(CpuState CpuState);

        [Instruction(Name: "lhu", Format: "100101:rs:rt:imm16")]
        abstract public void LHU(CpuState CpuState);

        // Store Byte/Half word/Word (Left/Right).
        [Instruction(Name: "sb", Format: "101000:rs:rt:imm16")]
        abstract public void SB(CpuState CpuState);

        [Instruction(Name: "sh", Format: "101001:rs:rt:imm16")]
        abstract public void SH(CpuState CpuState);

        [Instruction(Name: "sw", Format: "101011:rs:rt:imm16")]
        abstract public void SW(CpuState CpuState);

        [Instruction(Name: "swl", Format: "101010:rs:rt:imm16")]
        abstract public void SWL(CpuState CpuState);

        [Instruction(Name: "swr", Format: "101110:rs:rt:imm16")]
        abstract public void SWR(CpuState CpuState);

        // Load Linked word.
        // Store Conditional word.
        [Instruction(Name: "ll", Format: "110000:rs:rt:imm16")]
        abstract public void LL(CpuState CpuState);

        [Instruction(Name: "sc", Format: "111000:rs:rt:imm16")]
        abstract public void SC(CpuState CpuState);

        #endregion

        #region Fpu
        // Load Word to Cop1 floating point.
        // Store Word from Cop1 floating point.
        [Instruction(Name: "lwc1", Format: "110001:rs:ft:imm16")]
        abstract public void LWC1(CpuState CpuState);

        [Instruction(Name: "swc1", Format: "111001:rs:ft:imm16")]
        abstract public void SWC1(CpuState CpuState);

        // Binary Floating Point Unit Operations
        [Instruction(Name: "add_s", Format: "010001:10000:ft:fs:fd:000000")]
        abstract public void ADD_S(CpuState CpuState);

        [Instruction(Name: "sub_s", Format: "010001:10000:ft:fs:fd:000001")]
        abstract public void SUB_S(CpuState CpuState);

        [Instruction(Name: "mul_s", Format: "010001:10000:ft:fs:fd:000010")]
        abstract public void MUL_S(CpuState CpuState);

        [Instruction(Name: "div_s", Format: "010001:10000:ft:fs:fd:000011")]
        abstract public void DIV_S(CpuState CpuState);

        // Unary Floating Point Unit Operations
        [Instruction(Name: "sqrt_s", Format: "010001:10000:00000:fs:fd:000100")]
        abstract public void SQRT_S(CpuState CpuState);

        [Instruction(Name: "abs_s", Format: "010001:10000:00000:fs:fd:000101")]
        abstract public void ABS_S(CpuState CpuState);

        [Instruction(Name: "mov_s", Format: "010001:10000:00000:fs:fd:000110")]
        abstract public void MOV_S(CpuState CpuState);

        [Instruction(Name: "neg_s", Format: "010001:10000:00000:fs:fd:000111")]
        abstract public void NEG_S(CpuState CpuState);

        [Instruction(Name: "round_w_s", Format: "010001:10000:00000:fs:fd:001100")]
        abstract public void ROUND_W_S(CpuState CpuState);

        [Instruction(Name: "trunc_w_s", Format: "010001:10000:00000:fs:fd:001101")]
        abstract public void TRUNC_W_S(CpuState CpuState);

        [Instruction(Name: "ceil_w_s", Format: "010001:10000:00000:fs:fd:001110")]
        abstract public void CEIL_W_S(CpuState CpuState);

        [Instruction(Name: "floor_w_s", Format: "010001:10000:00000:fs:fd:001111")]
        abstract public void FLOOR_W_S(CpuState CpuState);

        // Convert
        [Instruction(Name: "cvt_s_w", Format: "010001:10100:00000:fs:fd:100000")]
        abstract public void CVT_S_W(CpuState CpuState);

        [Instruction(Name: "cvt_w_s", Format: "010001:10000:00000:fs:fd:100100")]
        abstract public void CVT_W_S(CpuState CpuState);

        // Move float point registers
        [Instruction(Name: "mfc1", Format: "010001:00000:rt:c1dr:00000:000000")]
        abstract public void MFC1(CpuState CpuState);

        [Instruction(Name: "cfc1", Format: "010001:00010:rt:c1cr:00000:000000")]
        abstract public void CFC1(CpuState CpuState);

        [Instruction(Name: "mtc1", Format: "010001:00100:rt:c1dr:00000:000000")]
        abstract public void MTC1(CpuState CpuState);

        [Instruction(Name: "ctc1", Format: "010001:00110:rt:c1cr:00000:000000")]
        abstract public void CTC1(CpuState CpuState);

        // Compare <condition> Single.
        [Instruction(Name: "c_f_s", Format: "010001:10000:ft:fs:00000:11:0000")]
        abstract public void C_F_S(CpuState CpuState);

        [Instruction(Name: "c_un_s", Format: "010001:10000:ft:fs:00000:11:0001")]
        abstract public void C_UN_S(CpuState CpuState);

        [Instruction(Name: "c_eq_s", Format: "010001:10000:ft:fs:00000:11:0010")]
        abstract public void C_EQ_S(CpuState CpuState);

        [Instruction(Name: "c_ueq_s", Format: "010001:10000:ft:fs:00000:11:0011")]
        abstract public void C_UEQ_S(CpuState CpuState);

        [Instruction(Name: "c_olt_s", Format: "010001:10000:ft:fs:00000:11:0100")]
        abstract public void C_OLT_S(CpuState CpuState);

        [Instruction(Name: "c_ult_s", Format: "010001:10000:ft:fs:00000:11:0101")]
        abstract public void C_ULT_S(CpuState CpuState);

        [Instruction(Name: "c_ole_s", Format: "010001:10000:ft:fs:00000:11:0110")]
        abstract public void C_OLE_S(CpuState CpuState);

        [Instruction(Name: "c_ule_s", Format: "010001:10000:ft:fs:00000:11:0111")]
        abstract public void C_ULE_S(CpuState CpuState);

        [Instruction(Name: "c_sf_s", Format: "010001:10000:ft:fs:00000:11:1000")]
        abstract public void C_SF_S(CpuState CpuState);

        [Instruction(Name: "c_ngle_s", Format: "010001:10000:ft:fs:00000:11:1001")]
        abstract public void C_NGLE_S(CpuState CpuState);

        [Instruction(Name: "c_seq_s", Format: "010001:10000:ft:fs:00000:11:1010")]
        abstract public void C_SEQ_S(CpuState CpuState);

        [Instruction(Name: "c_ngl_s", Format: "010001:10000:ft:fs:00000:11:1011")]
        abstract public void C_NGL_S(CpuState CpuState);

        [Instruction(Name: "c_lt_s", Format: "010001:10000:ft:fs:00000:11:1100")]
        abstract public void C_LT_S(CpuState CpuState);

        [Instruction(Name: "c_nge_s", Format: "010001:10000:ft:fs:00000:11:1101")]
        abstract public void C_NGE_S(CpuState CpuState);

        [Instruction(Name: "c_le_s", Format: "010001:10000:ft:fs:00000:11:1110")]
        abstract public void C_LE_S(CpuState CpuState);

        [Instruction(Name: "c_ngt_s", Format: "010001:10000:ft:fs:00000:11:1111")]
        abstract public void C_NGT_S(CpuState CpuState);

        #endregion

        #region Special
        // Syscall
        [Instruction(Name: "syscall", Format: "000000:imm20:001100", AssemblerFormat: "{$code}")]
        abstract public void SYSCALL(CpuState CpuState);

        [Instruction(Name: "cache", Format: "101111--------------------------")]
        abstract public void CACHE(CpuState CpuState);

        [Instruction(Name: "sync", Format: "000000:00000:00000:00000:00000:001111")]
        abstract public void SYNC(CpuState CpuState);

        [Instruction(Name: "break", Format: "000000:imm20:001101")]
        abstract public void BREAK(CpuState CpuState);

        [Instruction(Name: "dbreak", Format: "011100:00000:00000:00000:00000:111111", AssemblerFormat: "", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void DBREAK(CpuState CpuState);

        [Instruction(Name: "halt", Format: "011100:00000:00000:00000:00000:000000", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void HALT(CpuState CpuState);

        // (D?/Exception) RETurn
        [Instruction(Name: "dret", Format: "011100:00000:00000:00000:00000:111110", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void DRET(CpuState CpuState);

        [Instruction(Name: "eret", Format: "010000:10000:00000:00000:00000:011000")]
        abstract public void ERET(CpuState CpuState);

        // Move (From/To) IC
        [Instruction(Name: "mfic", Format: "011100:rt:00000:00000:00000:100100", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void MFIC(CpuState CpuState);

        [Instruction(Name: "mtic", Format: "011100:rt:00000:00000:00000:100110", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void MTIC(CpuState CpuState);

        // Move (From/To) DR
        [Instruction(Name: "mfdr", Format: "011100:00000:----------:00000:111101", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void MFDR(CpuState CpuState);

        [Instruction(Name: "mtdr", Format: "011100:00100:----------:00000:111101", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void MTDR(CpuState CpuState);

        // C? (From/To) Cop0
        [Instruction(Name: "cfc0", Format: "010000:00010:----------:00000:000000", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)] // CFC0(010000:00010:rt:c0cr:00000:000000)
        abstract public void CFC0(CpuState CpuState);

        [Instruction(Name: "ctc0", Format: "010000:00110:----------:00000:000000", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)] // CTC0(010000:00110:rt:c0cr:00000:000000)
        abstract public void CTC0(CpuState CpuState);

        // Move (From/To) Cop0
        [Instruction(Name: "mfc0", Format: "010000:00000:----------:00000:000000")]              // MFC0(010000:00000:rt:c0dr:00000:000000)
        abstract public void MFC0(CpuState CpuState);

        [Instruction(Name: "mtc0", Format: "010000:00100:----------:00000:000000")]              // MTC0(010000:00100:rt:c0dr:00000:000000)
        abstract public void MTC0(CpuState CpuState);

        #endregion

        #region VFpu

        // Move From/to Vfpu (C?).
        [Instruction(Name: "mfv", Format: "010010:00:011:rt:0:0000000:0:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void MFV(CpuState CpuState);

        [Instruction(Name: "mfvc", Format: "010010:00:011:rt:0:0000000:1:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void MFVC(CpuState CpuState);

        [Instruction(Name: "mtv", Format: "010010:00:111:rt:0:0000000:0:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void MTV(CpuState CpuState);

        [Instruction(Name: "mtvc", Format: "010010:00:111:rt:0:0000000:1:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void MTVC(CpuState CpuState);

        // Load/Store Vfpu (Left/Right).
        [Instruction(Name: "lv.s", Format: "110010:rs:vt5:imm14:vt2", _AddressType:InstructionAttribute.AddressType.None, _InstructionType:InstructionAttribute.InstructionType.PSP)]
        abstract public void LV_S(CpuState CpuState);

        [Instruction(Name: "lv.q",        Format: "110110:rs:vt5:imm14:0:vt1", _AddressType:InstructionAttribute.AddressType.None, _InstructionType:InstructionAttribute.InstructionType.PSP)]
        abstract public void LV_Q(CpuState CpuState);

        [Instruction(Name: "lvl.q",       Format: "110101:rs:vt5:imm14:0:vt1", _AddressType:InstructionAttribute.AddressType.None, _InstructionType:InstructionAttribute.InstructionType.PSP)]
        abstract public void LVL_Q(CpuState CpuState);

        [Instruction(Name: "lvr.q",       Format: "110101:rs:vt5:imm14:1:vt1", _AddressType:InstructionAttribute.AddressType.None, _InstructionType:InstructionAttribute.InstructionType.PSP)]
        abstract public void LVR_Q(CpuState CpuState);

        [Instruction(Name: "sv.q",        Format: "111110:rs:vt5:imm14:0:vt1", _AddressType:InstructionAttribute.AddressType.None, _InstructionType:InstructionAttribute.InstructionType.PSP)]
        abstract public void SV_Q(CpuState CpuState);

        // Vfpu DOT product
        // Vfpu SCaLe/ROTate
        [Instruction(Name: "vdot", Format: "011001:001:vt:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VDOT(CpuState CpuState);

        [Instruction(Name: "vscl", Format: "011001:010:vt:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VSCL(CpuState CpuState);

        [Instruction(Name: "vslt", Format: "011011:100:vt:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VSLT(CpuState CpuState);

        [Instruction(Name: "vsge", Format: "011011:110:vt:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VSGE(CpuState CpuState);

        // ROTate
        [Instruction(Name: "vrot", Format: "111100:111:01:imm5:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VROT(CpuState CpuState);

        // Vfpu ZERO/ONE
        [Instruction(Name: "vzero", Format: "110100:00:000:0:0110:two:0000000:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VZERO(CpuState CpuState);

        [Instruction(Name: "vone", Format: "110100:00:000:0:0111:two:0000000:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VONE(CpuState CpuState);

        // Vfpu MOVe/SiGN/Reverse SQuare root/COSine/Arc SINe/LOG2
        [Instruction(Name: "vmov", Format: "110100:00:000:0:0000:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VMOV(CpuState CpuState);

        [Instruction(Name: "vabs", Format: "110100:00:000:0:0001:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VABS(CpuState CpuState);

        [Instruction(Name: "vneg", Format: "110100:00:000:0:0010:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VNEG(CpuState CpuState);

        [Instruction(Name: "vocp", Format: "110100:00:010:0:0100:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VOCP(CpuState CpuState);

        [Instruction(Name: "vsgn", Format: "110100:00:010:0:1010:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VSGN(CpuState CpuState);

        [Instruction(Name: "vrcp", Format: "110100:00:000:1:0000:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VRCP(CpuState CpuState);

        [Instruction(Name: "vrsq", Format: "110100:00:000:1:0001:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VRSQ(CpuState CpuState);

        [Instruction(Name: "vsin", Format: "110100:00:000:1:0010:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VSIN(CpuState CpuState);

        [Instruction(Name: "vcos", Format: "110100:00:000:1:0011:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VCOS(CpuState CpuState);

        [Instruction(Name: "vexp2", Format: "110100:00:000:1:0100:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VEXP2(CpuState CpuState);

        [Instruction(Name: "vlog2", Format: "110100:00:000:1:0101:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VLOG2(CpuState CpuState);

        [Instruction(Name: "vsqrt", Format: "110100:00:000:1:0110:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VSQRT(CpuState CpuState);

        [Instruction(Name: "vasin", Format: "110100:00:000:1:0111:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VASIN(CpuState CpuState);

        [Instruction(Name: "vnrcp", Format: "110100:00:000:1:1000:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VNRCP(CpuState CpuState);

        [Instruction(Name: "vnsin", Format: "110100:00:000:1:1010:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VNSIN(CpuState CpuState);
        
        [Instruction(Name: "vrexp2", Format: "110100:00:000:1:1100:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VREXP2(CpuState CpuState);

        [Instruction(Name: "vsat0", Format: "110100:00:000:0:0100:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VSAT0(CpuState CpuState);

        [Instruction(Name: "vsat1", Format: "110100:00:000:0:0101:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VSAT1(CpuState CpuState);

        // Vfpu ConSTant
        [Instruction(Name: "vcst", Format: "110100:00:011:imm5:two:0000000:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VCST(CpuState CpuState);

        // Vfpu Matrix MULtiplication
        [Instruction(Name: "vmmul",       Format: "111100:000:vt:two:vs:one:vd", _AddressType:InstructionAttribute.AddressType.None, _InstructionType:InstructionAttribute.InstructionType.PSP)]
        abstract public void VMMUL(CpuState CpuState);

        // -
        [Instruction(Name: "vhdp",        Format: "011001:100:vt:two:vs:one:vd", _AddressType:InstructionAttribute.AddressType.None, _InstructionType:InstructionAttribute.InstructionType.PSP)]
        abstract public void VHDP(CpuState CpuState);

        [Instruction(Name: "vcrs.t",      Format: "011001:101:vt:1:vs:0:vd", _AddressType:InstructionAttribute.AddressType.None, _InstructionType:InstructionAttribute.InstructionType.PSP)]
        abstract public void VCRS(CpuState CpuState);

        [Instruction(Name: "vcrsp.t",     Format: "111100:101:vt:1:vs:0:vd", _AddressType:InstructionAttribute.AddressType.None, _InstructionType:InstructionAttribute.InstructionType.PSP)]
        abstract public void VCRSP_T(CpuState CpuState);

        // Vfpu Integer to(2) Color
        [Instruction(Name: "vi2c",        Format: "110100:00:001:11:101:two:vs:one:vd", _AddressType:InstructionAttribute.AddressType.None, _InstructionType:InstructionAttribute.InstructionType.PSP)]
        abstract public void VI2C(CpuState CpuState);

        [Instruction(Name: "vi2uc",       Format: "110100:00:001:11:100:two:vs:one:vd", _AddressType:InstructionAttribute.AddressType.None, _InstructionType:InstructionAttribute.InstructionType.PSP)]
        abstract public void VI2UC(CpuState CpuState);

        // -
        [Instruction(Name: "vtfm2",       Format: "111100:001:vt:0:vs:1:vd", _AddressType:InstructionAttribute.AddressType.None, _InstructionType:InstructionAttribute.InstructionType.PSP)]
        abstract public void VTFM2(CpuState CpuState);

        [Instruction(Name: "vtfm3",       Format: "111100:010:vt:1:vs:0:vd", _AddressType:InstructionAttribute.AddressType.None, _InstructionType:InstructionAttribute.InstructionType.PSP)]
        abstract public void VTFM3(CpuState CpuState);

        [Instruction(Name: "vtfm4",       Format: "111100:011:vt:1:vs:1:vd", _AddressType:InstructionAttribute.AddressType.None, _InstructionType:InstructionAttribute.InstructionType.PSP)]
        abstract public void VTFM4(CpuState CpuState);

        [Instruction(Name: "vhtfm2",      Format: "111100:001:vt:0:vs:0:vd", _AddressType:InstructionAttribute.AddressType.None, _InstructionType:InstructionAttribute.InstructionType.PSP)]
        abstract public void VHTFM2(CpuState CpuState);

        [Instruction(Name: "vhtfm3",      Format: "111100:010:vt:0:vs:1:vd", _AddressType:InstructionAttribute.AddressType.None, _InstructionType:InstructionAttribute.InstructionType.PSP)]
        abstract public void VHTFM3(CpuState CpuState);

        [Instruction(Name: "vhtfm4",      Format: "111100:011:vt:1:vs:0:vd", _AddressType:InstructionAttribute.AddressType.None, _InstructionType:InstructionAttribute.InstructionType.PSP)]
        abstract public void VHTFM4(CpuState CpuState);

        [Instruction(Name: "vsrt3",       Format: "110100:00:010:01000:two:vs:one:vd", _AddressType:InstructionAttribute.AddressType.None, _InstructionType:InstructionAttribute.InstructionType.PSP)]
        abstract public void VSRT3(CpuState CpuState);

        [Instruction(Name: "vfad", Format: "110100:00:010:00110:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VFAD(CpuState CpuState);

        // Vfpu MINimum/MAXium/ADD/SUB/DIV/MUL
        [Instruction(Name: "vmin",        Format: "011011:010:vt:two:vs:one:vd", _AddressType:InstructionAttribute.AddressType.None, _InstructionType:InstructionAttribute.InstructionType.PSP)]
        abstract public void VMIN(CpuState CpuState);

        [Instruction(Name: "vmax",        Format: "011011:011:vt:two:vs:one:vd", _AddressType:InstructionAttribute.AddressType.None, _InstructionType:InstructionAttribute.InstructionType.PSP)]
        abstract public void VMAX(CpuState CpuState);

        [Instruction(Name: "vadd", Format: "011000:000:vt:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VADD(CpuState CpuState);

        [Instruction(Name: "vsub", Format: "011000:001:vt:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VSUB(CpuState CpuState);

        [Instruction(Name: "vdiv", Format: "011000:111:vt:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VDIV(CpuState CpuState);

        [Instruction(Name: "vmul", Format: "011001:000:vt:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VMUL(CpuState CpuState);


        // Vfpu (Matrix) IDenTity
        [Instruction(Name: "vidt",        Format: "110100:00:000:0:0011:two:0000000:one:vd", _AddressType:InstructionAttribute.AddressType.None, _InstructionType:InstructionAttribute.InstructionType.PSP)]
        abstract public void VIDT(CpuState CpuState);

        [Instruction(Name: "vmidt",       Format: "111100:111:00:00011:two:0000000:one:vd",  _AddressType:InstructionAttribute.AddressType.None, _InstructionType:InstructionAttribute.InstructionType.PSP)]
        abstract public void VMIDT(CpuState CpuState);
        
        [Instruction(Name: "viim",        Format: "110111:11:0:vd:imm16", _AddressType:InstructionAttribute.AddressType.None, _InstructionType:InstructionAttribute.InstructionType.PSP)]
        abstract public void VIIM(CpuState CpuState);

        [Instruction(Name: "vmmov",       Format: "111100:111:00:00000:two:vs:one:vd",      _AddressType:InstructionAttribute.AddressType.None, _InstructionType:InstructionAttribute.InstructionType.PSP)]
        abstract public void VMMOV(CpuState CpuState);

        [Instruction(Name: "vmzero",      Format: "111100:111:00:00110:two:0000000:one:vd",      _AddressType:InstructionAttribute.AddressType.None, _InstructionType:InstructionAttribute.InstructionType.PSP)]
        abstract public void VMZERO(CpuState CpuState);

        [Instruction(Name: "vmone",       Format: "111100:111:00:00111:two:0000000:one:vd",           _AddressType:InstructionAttribute.AddressType.None, _InstructionType:InstructionAttribute.InstructionType.PSP)]
        abstract public void VMONE(CpuState CpuState);

        [Instruction(Name: "vnop",        Format: "111111:1111111111:00000:00000000000", _AddressType:InstructionAttribute.AddressType.None, _InstructionType:InstructionAttribute.InstructionType.PSP)]
        abstract public void VNOP(CpuState CpuState);

        [Instruction(Name: "vsync",       Format: "111111:1111111111:00000:01100100000", _AddressType:InstructionAttribute.AddressType.None, _InstructionType:InstructionAttribute.InstructionType.PSP)]
        abstract public void VSYNC(CpuState CpuState);

        [Instruction(Name: "vflush",      Format: "111111:1111111111:00000:10000001101",      _AddressType:InstructionAttribute.AddressType.None, _InstructionType:InstructionAttribute.InstructionType.PSP)]
        abstract public void VFLUSH(CpuState CpuState);

        [Instruction(Name: "vpfxd",       Format: "110111:10:------------:mskw:mskz:msky:mskx:satw:satz:saty:satx", _AddressType:InstructionAttribute.AddressType.None, _InstructionType:InstructionAttribute.InstructionType.PSP)]
        abstract public void VPFXD(CpuState CpuState);

        [Instruction(Name: "vpfxs",       Format: "110111:00:----:negw:negz:negy:negx:cstw:cstz:csty:cstx:absw:absz:absy:absx:swzw:swzz:swzy:swzx", _AddressType:InstructionAttribute.AddressType.None, _InstructionType:InstructionAttribute.InstructionType.PSP)]
        abstract public void VPFXS(CpuState CpuState);

        [Instruction(Name: "vpfxt",       Format: "110111:01:----:negw:negz:negy:negx:cstw:cstz:csty:cstx:absw:absz:absy:absx:swzw:swzz:swzy:swzx", _AddressType:InstructionAttribute.AddressType.None, _InstructionType:InstructionAttribute.InstructionType.PSP)]
        abstract public void VPFXT(CpuState CpuState);

        [Instruction(Name: "vdet",        Format: "011001:110:vt:two:vs:one:vd", _AddressType:InstructionAttribute.AddressType.None, _InstructionType:InstructionAttribute.InstructionType.PSP)]
        abstract public void VDET(CpuState CpuState);

        [Instruction(Name: "vrnds",       Format: "110100:00:001:00:000:two:vs:one:0000000", _AddressType:InstructionAttribute.AddressType.None, _InstructionType:InstructionAttribute.InstructionType.PSP)]
        abstract public void VRNDS(CpuState CpuState);

        [Instruction(Name: "vrndi",       Format: "110100:00:001:00:001:two:0000000:one:vd", _AddressType:InstructionAttribute.AddressType.None, _InstructionType:InstructionAttribute.InstructionType.PSP)]
        abstract public void VRNDI(CpuState CpuState);

        [Instruction(Name: "vrndf1",      Format: "110100:00:001:00:010:two:0000000:one:vd", _AddressType:InstructionAttribute.AddressType.None, _InstructionType:InstructionAttribute.InstructionType.PSP)]
        abstract public void VRNDF1(CpuState CpuState);

        [Instruction(Name: "vrndf2",      Format: "110100:00:001:00:011:two:0000000:one:vd", _AddressType:InstructionAttribute.AddressType.None, _InstructionType:InstructionAttribute.InstructionType.PSP)]
        abstract public void VRNDF2(CpuState CpuState);

        ////////////////////////////
        /// Not implemented yet!
        ////////////////////////////
        [Instruction(Name: "vcmp",        Format: "011011:000:vt:two:vs:one:0000:imm3" , _AddressType:InstructionAttribute.AddressType.None, _InstructionType:InstructionAttribute.InstructionType.PSP)]
        abstract public void VCMP(CpuState CpuState);

        [Instruction(Name: "vcmovf",      Format: "110100:10:101:01:imm3:two:vs:one:vd", _AddressType:InstructionAttribute.AddressType.None, _InstructionType:InstructionAttribute.InstructionType.PSP)]
        abstract public void VCMOVF(CpuState CpuState);

        [Instruction(Name: "vcmovt", Format: "110100:10:101:00:imm3:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VCMOVT(CpuState CpuState);

        [Instruction(Name: "bvf", Format: "010010:01:000:imm3:00:imm16", _AddressType: InstructionAttribute.AddressType.S16, _InstructionType: InstructionAttribute.InstructionType.PSP | InstructionAttribute.InstructionType.Branch)]
        abstract public void BVF(CpuState CpuState);

        [Instruction(Name: "bvfl", Format: "010010:01:000:imm3:10:imm16", _AddressType: InstructionAttribute.AddressType.S16, _InstructionType: InstructionAttribute.InstructionType.PSP | InstructionAttribute.InstructionType.Branch)]
        abstract public void BVFL(CpuState CpuState);

        [Instruction(Name: "bvt", Format: "010010:01:000:imm3:01:imm16", _AddressType: InstructionAttribute.AddressType.S16, _InstructionType: InstructionAttribute.InstructionType.PSP | InstructionAttribute.InstructionType.Branch)]
        abstract public void BVT(CpuState CpuState);

        [Instruction(Name: "bvtl", Format: "010010:01:000:imm3:11:imm16", _AddressType: InstructionAttribute.AddressType.S16, _InstructionType: InstructionAttribute.InstructionType.PSP | InstructionAttribute.InstructionType.Branch)]
        abstract public void BVTL(CpuState CpuState);

        [Instruction(Name: "vavg",        Format: "110100:00:010:00111:two:vs:one:vd", _AddressType:InstructionAttribute.AddressType.None, _InstructionType:InstructionAttribute.InstructionType.PSP)]
        abstract public void VAVG(CpuState CpuState);

        [Instruction(Name: "vf2id", Format: "110100:10:011:imm5:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VF2ID(CpuState CpuState);

        [Instruction(Name: "vf2in", Format: "110100:10:000:imm5:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VF2IN(CpuState CpuState);

        [Instruction(Name: "vf2iu", Format: "110100:10:010:imm5:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VF2IU(CpuState CpuState);

        [Instruction(Name: "vf2iz", Format: "110100:10:001:imm5:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VF2IZ(CpuState CpuState);

        [Instruction(Name: "vi2f", Format: "110100:10:100:imm5:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VI2F(CpuState CpuState);

        [Instruction(Name: "vscmp",       Format: "011011:101:vt:two:vs:one:vd", _AddressType:InstructionAttribute.AddressType.None, _InstructionType:InstructionAttribute.InstructionType.PSP)]
        abstract public void VSCMP(CpuState CpuState);

        [Instruction(Name: "vmscl", Format: "111100:100:vt:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VMSCL(CpuState CpuState);

        [Instruction(Name: "vt4444.q",    Format: "110100:00:010:11001:two:vs:one:vd", _AddressType:InstructionAttribute.AddressType.None, _InstructionType:InstructionAttribute.InstructionType.PSP)]
        abstract public void VT4444_Q(CpuState CpuState);

        [Instruction(Name: "vt5551.q", Format: "110100:00:010:11010:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VT5551_Q(CpuState CpuState);

        [Instruction(Name: "vt5650.q", Format: "110100:00:010:11011:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VT5650_Q(CpuState CpuState);

        [Instruction(Name: "vmfvc", Format: "110100:00:010:10000:1:imm7:0:vd", _AddressType:InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VMFVC(CpuState CpuState);

        [Instruction(Name: "vmtvc", Format: "110100:00:010:10001:0:vs:1:imm7", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VMTVC(CpuState CpuState);

        [Instruction(Name: "mfvme",       Format: "011010--------------------------")]
        abstract public void MFVME(CpuState CpuState);

        [Instruction(Name: "mtvme", Format: "101100--------------------------")]
        abstract public void MTVME(CpuState CpuState);

        [Instruction(Name: "sv.s",        Format: "111010:rs:vt5:imm14:vt2", _AddressType:InstructionAttribute.AddressType.None, _InstructionType:InstructionAttribute.InstructionType.PSP)]
        abstract public void SV_S(CpuState CpuState);

        [Instruction(Name: "svl.q", Format: "111101:rs:vt5:imm14:0:vt1", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void SVL_Q(CpuState CpuState);

        [Instruction(Name: "svr.q", Format: "111101:rs:vt5:imm14:1:vt1", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void SVR_Q(CpuState CpuState);

        [Instruction(Name: "vbfy1", Format: "110100:00:010:00010:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VBFY1(CpuState CpuState);

        [Instruction(Name: "vbfy2", Format: "110100:00:010:00011:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VBFY2(CpuState CpuState);

        [Instruction(Name: "vfim", Format: "110111:11:1:vd:imm16", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VFIM(CpuState CpuState);

        [Instruction(Name: "vf2h", Format: "110100:00:001:10:010:two:vs:one:vd", _AddressType:InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VF2H(CpuState CpuState);

        [Instruction(Name: "vh2f", Format: "110100:00:001:10:011:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VH2F(CpuState CpuState);

        [Instruction(Name: "vi2s", Format: "110100:00:001:11:111:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VI2S(CpuState CpuState);

        [Instruction(Name: "vi2us", Format: "110100:00:001:11:110:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VI2US(CpuState CpuState);

        /*
        [Instruction(Name: "vidt", Format: "110100:00:000:0:0011:two:0000000:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VIDT(CpuState CpuState);
        */

        [Instruction(Name: "vlgb", Format: "110100:00:001:10:111:two:vs:one:vd", _AddressType:InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VLGB(CpuState CpuState);

        [Instruction(Name: "vqmul", Format: "111100:101:vt:1:vs:1:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VQMUL(CpuState CpuState);

        [Instruction(Name: "vs2i", Format: "110100:00:001:11:011:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VS2I(CpuState CpuState);

        [Instruction(Name: "vsbn", Format: "011000:010:vt:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VSBN(CpuState CpuState);

        [Instruction(Name: "vsbz", Format: "110100:00:001:10:110:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VSBZ(CpuState CpuState);

        [Instruction(Name: "vsocp", Format: "110100:00:010:00101:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VSOCP(CpuState CpuState);

        [Instruction(Name: "vsrt1", Format: "110100:00:010:00000:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VSRT1(CpuState CpuState);

        [Instruction(Name: "vsrt2", Format: "110100:00:010:00001:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VSRT2(CpuState CpuState);

        [Instruction(Name: "vsrt4", Format: "110100:00:010:01001:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VSRT4(CpuState CpuState);

        [Instruction(Name: "vus2i", Format: "110100:00:001:11:010:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VUS2I(CpuState CpuState);

        [Instruction(Name: "vwb.q", Format: "111110------------------------1-", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VWB_Q(CpuState CpuState);

        [Instruction(Name: "vwbn", Format: "110100:11:imm8:two:vs:one:vd", _AddressType: InstructionAttribute.AddressType.None, _InstructionType: InstructionAttribute.InstructionType.PSP)]
        abstract public void VWBN(CpuState CpuState);

        #endregion

        virtual public void INVALID(CpuState CpuState)
        {
            throw (new InvalidOperationException());
        }
	}
}
