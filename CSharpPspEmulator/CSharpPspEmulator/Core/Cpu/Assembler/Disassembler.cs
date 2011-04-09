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
            public bool UseMemonic;

            public State(InstructionData InstructionData, uint PC = 0, bool UseMemonic = false)
            {
                this.PC = PC;
                this.InstructionData = InstructionData;
                this.UseMemonic = UseMemonic;
            }
        }

        static String[] Memonics = new String[] {
            "zr", "at", "v0", "v1", "a0", "a1", "a2", "a3",
            "t0", "t1", "t2", "t3", "t4", "t5", "t6", "t7",
            "s0", "s1", "s2", "s3", "s4", "s5", "s6", "s7",
            "t8", "t9", "k0", "k1", "gp", "sp", "fp", "ra",
        };

        static String[] NoMemonics = new String[] {
            "r0", "r1", "r2", "r3", "r4", "r5", "r6", "r7",
            "r8", "r9", "r10", "r11", "r12", "r13", "r14", "r15",
            "r16", "r17", "r18", "r19", "r20", "r21", "r22", "r23",
            "r24", "r25", "r26", "r27", "r28", "r29", "r30", "r31",
        };

        static public DisassembleDelegate CreateDisassembleDelegate(InstructionAttribute InstructionAttribute)
        {
            return delegate(State State)
            {
                String Result = InstructionAttribute.AssemblerFormat;

                String[] RegisterTable;

                if (State.UseMemonic)
                {
                    RegisterTable = Memonics;
                }
                else
                {
                    RegisterTable = NoMemonics;
                }

                Result = Result
                    .Replace("{$d}", RegisterTable[State.InstructionData.RD])
                    .Replace("{$s}", RegisterTable[State.InstructionData.RS])
                    .Replace("{$t}", RegisterTable[State.InstructionData.RT])
                ;

                return Result
                    .Replace("{$imm}", "" + State.InstructionData.IMM)
                    .Replace("{$immu}", "" + State.InstructionData.IMMU)
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
