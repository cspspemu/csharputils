using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpPspEmulator.Utils.Tables;
using CSharpPspEmulator.Core.Cpu.Interpreter;

namespace CSharpPspEmulator.Core.Cpu.Assembler
{
    public class Disassembler
    {
        public delegate String DisassembleDelegate(State DisassembleState);

        public class State
        {
            public uint PC;
            public InstructionData InstructionData;
            public bool UseMemonic;

            public State(InstructionData InstructionData, uint PC = 0, bool UseMemonic = false)
            {
                this.PC = PC;
                this.InstructionData = InstructionData;
                this.UseMemonic = UseMemonic;
            }
        }

        static public DisassembleDelegate CreateDisassembleDelegate(InstructionAttribute InstructionAttribute)
        {
            return delegate(State State)
            {
                String Result = InstructionAttribute.Name + " " + InstructionAttribute.AssemblerFormat;

                String[] RegisterTable;

                if (State.UseMemonic)
                {
                    RegisterTable = RegistersCpu.Memonics;
                }
                else
                {
                    RegisterTable = RegistersCpu.NoMemonics;
                }

                Result = Result
                    .Replace("{$d}", RegisterTable[State.InstructionData.RD])
                    .Replace("{$s}", RegisterTable[State.InstructionData.RS])
                    .Replace("{$t}", RegisterTable[State.InstructionData.RT])
                ;

                return Result
                    .Replace("{$imm}", "" + State.InstructionData.IMM)
                    .Replace("{$immu}", "" + State.InstructionData.IMM)
                    .Replace("{$offset2}", "" + State.InstructionData.OFFSET2)
                    .Replace("{$code}", "0x" + Convert.ToString(State.InstructionData.CODE, 16))
                    .Replace(",", ", ")
                ;
                
            };
        }

        public DisassembleDelegate Disassemble;

        static Disassembler _Instance = null;
        static public Disassembler Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new Disassembler();
                }
                return _Instance;
            }
        }

        protected Disassembler()
        {
            var Table = new List<TableExecutionDelegate<DisassembleDelegate>.TableExecutionItem>();

            foreach (var Attribute in InstructionAttribute.GetInstructionAttributeMethods(typeof(InterpretedCpu)))
            {
                var Item = new TableExecutionDelegate<DisassembleDelegate>.TableExecutionItem();
                Item.TableItem = Attribute;
                Item.ExecutionDelegate = CreateDisassembleDelegate(Attribute);
                Table.Add(Item);
            }

            Disassemble = new TableExecutionDelegate<DisassembleDelegate>(Table, CurrentTable => delegate(State State)
            {
                return CurrentTable.GetDelegateByValue(State.InstructionData.Value)(State);
            }, delegate(State State)
            {
                return "<Invalid>";
            });
        }
    }
}
