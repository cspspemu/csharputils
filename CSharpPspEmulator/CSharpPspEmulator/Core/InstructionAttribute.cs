//#define DEBUG_INSTRUCTION_GENERATE_TABLE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;

namespace CSharpPspEmulator.Core
{
	public delegate void ExecutionDelegate(CpuState CpuState);

	public sealed class InstructionAttribute : Attribute
	{
        public enum AddressType
        {
            None,
            S16,
            S26,
            Register,
        }

        public enum InstructionType
        {
            Normal,
            PSP,
            Branch,
            JumpAndLink,
            Jump
        }

		public MethodInfo MethodInfo;
		public ExecutionDelegate Execute;
		public String Name
		{
			get
			{
				return MethodInfo.Name;
			}
		}
		readonly public String Format;
        readonly public InstructionType _InstructionType;
		readonly public AddressType _AddressType;
		public uint FormatValue;
		public uint FormatMask;

        public struct TableExecutionDelegate
        {
            IEnumerable<InstructionAttribute> InstructionList;
            public int Offset;
            public uint BaseMask;
            public uint Mask;
            public uint Level;
            public ExecutionDelegate[] Delegates;

            public TableExecutionDelegate(IEnumerable<InstructionAttribute> InstructionList, uint BaseMask = 0xFFFFFFFF, uint Level = 0)
            {
                this.InstructionList = InstructionList;
                Offset = 0;
                Mask = 0;
                this.BaseMask = BaseMask;
                this.Level = Level;
                if (Level > 6) throw(new Exception("Too much recursion generating tables"));
                Delegates = null;
                GetOffsetMask();
                #if DEBUG_INSTRUCTION_GENERATE_TABLE
                    Console.WriteLine("TableExecutionDelegate Offset:{0,8:X} Mask:{1,8:X} BaseMask:{2,8:X} Level:{3}", Offset, Mask, BaseMask, Level);
                #endif
                FillDelegates();
            }

            private void FillDelegates()
            {
                var InstructionsPerOffset = new Dictionary<uint, List<InstructionAttribute>>();

                foreach (var Instruction in InstructionList)
                {
                    var InstructionOffset = (Instruction.FormatValue >> Offset) & Mask;
                    if (!InstructionsPerOffset.ContainsKey(InstructionOffset))
                    {
                        InstructionsPerOffset[InstructionOffset] = new List<InstructionAttribute>();
                    }
                    InstructionsPerOffset[InstructionOffset].Add(Instruction);
                }

                foreach (var InstructionPerOffset in InstructionsPerOffset)
                {
                    var InstructionOffset = InstructionPerOffset.Key;
                    var InstructionListForThisOffset = InstructionPerOffset.Value;
                    if (InstructionOffset < 0) throw (new Exception("Invalid Instruction Offset (I)"));
                    if (InstructionOffset >= Delegates.Length) throw (new Exception("Invalid Instruction Offset (II)"));
                    if (Delegates[InstructionOffset] != null) throw (new Exception("Repeated Instruction"));

                    if (InstructionListForThisOffset.Count == 0)
                    {
                        throw(new Exception("No Items in list"));
                    }
                    else if (InstructionListForThisOffset.Count > 1)
                    {
                        /*foreach (var Instruction in InstructionList)
                        {

                            Console.WriteLine(Instruction.Name + ": " + InstructionOffset + ": " + Delegates[InstructionOffset]);
                        }*/
                        Delegates[InstructionOffset] = new TableExecutionDelegate(InstructionListForThisOffset, BaseMask & ~(Mask << Offset), Level + 1);
                        //throw(new NotImplementedException());
                    }
                    // One in list.
                    else
                    {
                        #if DEBUG_INSTRUCTION_GENERATE_TABLE
                            Console.WriteLine("{0}", InstructionListForThisOffset[0]);
                        #endif
                        Delegates[InstructionOffset] = InstructionListForThisOffset[0].Execute;
                    }
                }
            }

            private void GetOffsetMask()
            {
                Mask = GetCommonMask(InstructionList);
                Mask &= BaseMask;
                if (Mask == 0) throw (new Exception("Empty CommonMask"));
                Offset = 0;
                while ((Mask & 1) == 0)
                {
                    Offset++;
                    Mask >>= 1;
                }
                Delegates = new ExecutionDelegate[1 << SizeBitcount(Mask)];
                Mask &= (uint)(Delegates.Length - 1);
            }

            static int SizeBitcount(uint n)
            {
                int count = 0;
                while ((n & 1) != 0)
                {
                    count++;
                    n >>= 1;
                }
                return count;
            }

            /*static int SparseBitcount(int n)
            {
                int count = 0;
                while (n != 0)
                {
                    count++;
                    n &= (n - 1);
                }
                return count;
            }*/

            static public implicit operator ExecutionDelegate(TableExecutionDelegate TableExecutionDelegate)
            {
                return TableExecutionDelegate.Execute;
            }

            static private uint GetCommonMask(IEnumerable<InstructionAttribute> InstructionList)
            {
                return InstructionList.Aggregate((uint)0xFFFFFFFF, (Previous, Item) => Previous & Item.FormatMask);
            }

            public void Execute(CpuState CpuState)
            {
                Delegates[(CpuState.InstructionData.Value >> Offset) & Mask](CpuState);
            }
        }

        static public implicit operator ExecutionDelegate(InstructionAttribute Attribute)
        {
            return delegate(CpuState CpuState)
            {
                Attribute.MethodInfo.Invoke(CpuState.CpuBase, new object[] { CpuState });
            };
        }

		static public ExecutionDelegate ProcessClass(Type Type)
		{
			var InstructionList = new List<InstructionAttribute>();
			ExecutionDelegate ExecutionDelegate;
			foreach (var MethodInfo in Type.GetMethods())
			{
				InstructionAttribute[] Attributes = (InstructionAttribute[])MethodInfo.GetCustomAttributes(typeof(InstructionAttribute), true);
				if (Attributes.Length > 0)
				{
					//Console.WriteLine("{0} {1,08:X} {2,08:X}", Attributes[0].Name, Attributes[0].FormatValue, Attributes[0].FormatMask);
					Attributes[0].MethodInfo = MethodInfo;
                    Attributes[0].Execute = Attributes[0];
					InstructionList.Add(Attributes[0]);
				}
				//Type.InvokeMember();
				//Console.WriteLine([0]);
			}

            TableExecutionDelegate TableExecutionDelegate = new TableExecutionDelegate(InstructionList);

			ExecutionDelegate = delegate(CpuState CpuState)
			{
				foreach (var Instruction in InstructionList)
				{
					if (Instruction.FormatMatches(CpuState.InstructionData.Value))
					{
						Instruction.MethodInfo.Invoke(CpuState.CpuBase, new object[] { CpuState });
					}
				}
				//Console.WriteLine("ExecutionDelegate");
			};
			return ExecutionDelegate;
		}

		public bool FormatMatches(uint Value)
		{
			return (Value & FormatMask) == FormatValue;
		}

		private void FormatInsert(uint Value, uint Mask, int Count = 1)
		{
			uint MMask = (uint)((1 << Count) - 1);
			FormatValue <<= Count; FormatValue |= Value & MMask;
			FormatMask  <<= Count; FormatMask |= Mask & MMask;
		}

		private void ParseFormat()
		{

			foreach (var Part in Format.Split(':'))
			{
				switch (Part)
				{
                    case "cstw": case "cstz": case "csty": case "cstx":
                    case "absw": case "absz": case "absy": case "absx":
                    case "mskw": case "mskz": case "msky": case "mskx":
                    case "negw": case "negz": case "negy": case "negx":
                    case "one": case "two":
                    case "vt1":
                        FormatInsert(0, 0, 1);
                    break;
                    case "vt2":
                    case "satw": case "satz": case "saty": case "satx":
                    case "swzw": case "swzz": case "swzy": case "swzx":
                       FormatInsert(0, 0, 2);
                    break;
                    case "imm3":
                        FormatInsert(0, 0, 3);
                    break;
                    case "fcond":
                        FormatInsert(0, 0, 4);
                    break;
                    case "c0dr": case "c0cr": case "c1dr": case "c1cr": case "imm5": case "vt5":
                    case "rs": case "rd": case "rt": case "sa": case "lsb": case "msb": case "fs": case "fd": case "ft":
                        FormatInsert(0, 0, 5);
                    break;
                    case "vs": case "vt": case "vd": case "imm7":
                        FormatInsert(0, 0, 7);
                    break;
                    case "imm8" : FormatInsert(0, 0, 8); break;
                    case "imm14": FormatInsert(0, 0, 14); break;
                    case "imm16": FormatInsert(0, 0, 16); break;
                    case "imm20": FormatInsert(0, 0, 20); break;
                    case "imm26": FormatInsert(0, 0, 26); break;

					default:
						foreach (var Bit in Part) {
							switch (Bit)
							{
								case '0': FormatInsert(0, 1); break;
								case '1': FormatInsert(1, 1); break;
								case '-': FormatInsert(0, 0); break;
								default: throw(new Exception("Unknwon format : '" + Bit + "' in '" + Part + "'"));
							}
						}
					break;
				}
			}
		}

        public InstructionAttribute(String Format, AddressType _AddressType = InstructionAttribute.AddressType.None, InstructionType _InstructionType = InstructionType.Normal)
		{
			//this.Name = Name;
			this.Format = Format;
			this._AddressType = _AddressType;
            this._InstructionType = _InstructionType;
			ParseFormat();
		}

        public override string ToString()
        {
            return String.Format("InstructionAttribute({0}:{1,8:X}:{2,8:X})", Name, FormatValue, FormatMask);
        }
	}
}
