using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using CSharpPspEmulator.Core.Cpu.Assembler;
using CSharpPspEmulator.Hle;

namespace CSharpPspEmulator.Core.Cpu
{
	sealed public class CpuState
	{
        public SystemHle SystemHle;
		public InstructionData InstructionData;
		public RegistersCpu RegistersCpu;
        public RegistersFpu RegistersFpu;
        public RegistersVFpu RegistersVFpu;
        public ThreadInfo ThreadInfo;
		public Memory Memory;
		public CpuBase CpuBase;

        public CpuState(SystemHle SystemHle, RegistersCpu Registers, CpuBase CpuBase, Memory Memory, ThreadInfo ThreadInfo = null)
		{
            if (ThreadInfo == null)
            {
                ThreadInfo = new ThreadInfo();
            }
            this.SystemHle = SystemHle;
			this.RegistersCpu = Registers;
			this.CpuBase = CpuBase;
			this.Memory = Memory;
            this.ThreadInfo = ThreadInfo;
		}

		public uint RT
		{
			set { RegistersCpu.SetRegister(InstructionData.RT, value); }
			get { return RegistersCpu.GetRegister(InstructionData.RT); }
		}

		public uint RS
		{
			set { RegistersCpu.SetRegister(InstructionData.RS, value); }
			get { return RegistersCpu.GetRegister(InstructionData.RS); }
		}

		public uint RD
		{
			set { RegistersCpu.SetRegister(InstructionData.RD, value); }
			get { return RegistersCpu.GetRegister(InstructionData.RD); }
		}

        public uint PS
        {
            set { InstructionData.PS = value; }
            get { return InstructionData.PS; }
        }

        public uint CODE
        {
            set { InstructionData.CODE = value; }
            get { return InstructionData.CODE; }
        }

        public int OFFSET2
        {
            set { InstructionData.OFFSET2 = value; }
            get { return InstructionData.OFFSET2; }
        }

        public int IMM
        {
            get { return InstructionData.IMM ; }
        }
    
        public uint IMMU
        {
            get { return InstructionData.IMMU; }
        }

        public uint PC
        {
            get { return RegistersCpu.PC; }
            set {
                RegistersCpu.PC = value;
                RegistersCpu.nPC = RegistersCpu.PC + 4;
            }
        }

        public void ReadInstructionAtPC()
        {
            InstructionData.Value = Memory.ReadUnsigned32(RegistersCpu.PC);
        }

        public void ExecuteStoredInstruction()
        {
            ThreadInfo.ExecutedInstructionCount++;
            CpuBase.Execute(this);
        }

        public void ExecuteNextInstruction()
        {
            ReadInstructionAtPC();
            ExecuteStoredInstruction();
        }

        /*public void ExecuteRaw()
        {
            while (true)
            {
                ExecuteNextInstruction();
            }
        }*/

        void DumpAsm(uint PC)
        {
            Console.WriteLine("{0,8:X}({1}): {2}", PC, ThreadInfo.Thread.ManagedThreadId, Disassembler.Instance.Disassemble(new Disassembler.State(Memory.ReadUnsigned32(PC), PC)));
        }

        void DumpAsm()
        {
            DumpAsm(PC);
        }

        public void Execute(bool Trace = false)
        {
            Console.WriteLine("## Start Execute: (" + ThreadInfo.ID + ")");
            ThreadInfo.Running = true;
            try
            {
                while (true)
                {
                    //Console.Write("{0,8:X} : {1,8:X}\r", RegistersCpu.PC, RegistersCpu[8]);
                    if (Trace)
                    {
                        //Console.WriteLine("R8: {0,8:X}", RegistersCpu[8]);
                        DumpAsm();
                        //Console.ReadKey();
                    }
                    ExecuteNextInstruction();
                }
            }
            catch (TargetInvocationException TargetInvocationException)
            {
                if (TargetInvocationException.InnerException is PspDebugBreakException)
                {
                }
                else if (TargetInvocationException.InnerException is PspExitThreadException)
                {
                }
                else
                {
                    //DumpAsm();
                    Console.WriteLine(TargetInvocationException.InnerException.Message);
                    DumpAsm(PC - 8);
                    DumpAsm(PC - 4);
                    DumpAsm(PC + 0);
                    throw (new Exception("Unexpected Exception: " + TargetInvocationException.InnerException.Message, TargetInvocationException));
                }
            }
            finally
            {
                ThreadInfo.Running = false;
                Console.WriteLine("## End Execute: (" + ThreadInfo.ID + ")");
            }
        }

        internal void AdvancePC(int Offset)
        {
            RegistersCpu.PC = RegistersCpu.nPC;
            RegistersCpu.nPC = (uint)(RegistersCpu.nPC + Offset);
        }

        public uint GetArgument(int Index)
        {
            return RegistersCpu[(uint)(4 + Index)];
        }

        public void SetReturnValue(uint Value)
        {
            RegistersCpu["v0"] = Value;
        }

        public void Reset()
        {
		    InstructionData.Value = 0;
		    RegistersCpu.Reset();
            //RegistersFpu.Reset();
            //RegistersVFpu.Reset();
            //ThreadInfo.Reset();
		    //CpuBase.Reset();

            //Memory.Reset();

            //throw new NotImplementedException();
        }
    }
}
