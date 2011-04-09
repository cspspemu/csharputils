using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpPspEmulator.Utils.Tables;

namespace CSharpPspEmulator.Core.Cpu.Assembler
{
    public class Disassembler
    {
        public delegate String DisassembleDelegate(State DisassembleState);

        public class State
        {
            public uint PC;
            public InstructionData InstructionData;

            public State(InstructionData InstructionData, uint PC = 0)
            {
                this.PC = PC;
                this.InstructionData = InstructionData;
            }
        }

        static public DisassembleDelegate CreateDisassembleDelegate(InstructionAttribute InstructionAttribute)
        {
            return delegate(State State)
            {
                return InstructionAttribute.AssemblerFormat
                    .Replace("{$d}", "r" + State.InstructionData.RD)
                    .Replace("{$s}", "r" + State.InstructionData.RS)
                    .Replace("{$t}", "r" + State.InstructionData.RT)
                    .Replace("{$imm}", "" + State.InstructionData.IMM)
                    .Replace(",", ", ")
                ;
            };
        }

        public DisassembleDelegate Disassemble;

        public Disassembler()
        {
            var Table = new List<TableExecutionDelegate<DisassembleDelegate>.TableExecutionItem>();

            foreach (var Attribute in InstructionAttribute.GetInstructionAttributeMethods(typeof(Cpu)))
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
