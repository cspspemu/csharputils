using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;
using CSharpPspEmulator.Utils.Tables;

namespace CSharpPspEmulator.Core.Cpu
{
    public sealed class InstructionAttribute : Attribute, ITableItem
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
            Normal = 1,
            Branch = 2,
            Jump = 4,
            Link = 8,
            JumpAndLink = Jump | Link,
            PSP = 16,
        }

        //public delegate String GenerateAssemblerDelegate(String Name);

		public MethodInfo MethodInfo;
		public ExecutionDelegate Execute;
        public String Name;
		public String MethodName
		{
			get
			{
				return MethodInfo.Name;
			}
		}
		readonly public String _Format;
        readonly public String AssemblerFormat;
        readonly public InstructionType _InstructionType;
		readonly public AddressType _AddressType;
        private bool FormatParsed;
		private uint FormatValue;
		private uint FormatMask;

        private void CheckFormatParsed()
        {
            lock (this)
            {
                if (!FormatParsed)
                {
                    ParseFormat();
                    FormatParsed = true;
                }
            }
        }

        public uint TableValue
        {
            get {
                CheckFormatParsed();
                return FormatValue;
            }
        }

        public uint TableMask
        {
            get {
                CheckFormatParsed();
                return FormatMask;
            }
        }

        /*static public implicit operator ExecutionDelegate(InstructionAttribute Attribute)
        {
            return delegate(CpuState CpuState)
            {
                Attribute.MethodInfo.Invoke(CpuState.CpuBase, new object[] { CpuState });
            };
        }*/

        public delegate ExecutionDelegate ExecutionDelegateGenerator(InstructionAttribute InstructionAttribute);

        /*static public ExecutionDelegate CreateExecutionDelegate(InstructionAttribute InstructionAttribute)
        {
            return delegate(CpuState CpuState)
            {
                InstructionAttribute.MethodInfo.Invoke(CpuState.CpuBase, new object[] { CpuState });
            };
        }*/

        static public IEnumerable<InstructionAttribute> GetInstructionAttributeMethods(Type Type)
        {
            var List = new List<InstructionAttribute>();
            foreach (var MethodInfo in Type.GetMethods())
            {
                var InstructionAttributes = (InstructionAttribute[])MethodInfo.GetCustomAttributes(typeof(InstructionAttribute), true);
                //Console.WriteLine("InstructionAttributes:" + InstructionAttributes.Length);
                if (InstructionAttributes.Length == 1)
                {
                    InstructionAttributes[0].MethodInfo = MethodInfo;
                    List.Add(InstructionAttributes[0]);
                }
            }
            return List;
        }

        static public ExecutionDelegate GetExecutor(Type Type)
        {
            return GetExecutor(Type, InstructionAttribute => delegate(CpuState CpuState)
            {
                InstructionAttribute.MethodInfo.Invoke(CpuState.CpuBase, new object[] { CpuState });
            }, delegate(CpuState CpuState)
            {
                Type.GetMethod("INVALID").Invoke(CpuState.CpuBase, new object[] { CpuState });
            });
        }

        static public ExecutionDelegate GetExecutor(Type Type, ExecutionDelegateGenerator ExecutionDelegateGenerator, ExecutionDelegate DefaultExecutionDelegate)
		{
            var Table = new List<TableExecutionDelegate<ExecutionDelegate>.TableExecutionItem>();

            foreach (var Attribute in GetInstructionAttributeMethods(Type))
            {
                var Item = new TableExecutionDelegate<ExecutionDelegate>.TableExecutionItem();
                Item.TableItem = Attribute;
                Item.ExecutionDelegate = ExecutionDelegateGenerator(Attribute);
                Table.Add(Item);
			}

            return new TableExecutionDelegate<ExecutionDelegate>(Table, CurrentTable => delegate(CpuState CpuState)
            {
                CurrentTable.GetDelegateByValue(CpuState.InstructionData.Value)(CpuState);
            }, DefaultExecutionDelegate);
		}

		private void FormatInsert(uint Value, uint Mask, int Count = 1)
		{
			uint MMask = (uint)((1 << Count) - 1);
			FormatValue <<= Count; FormatValue |= Value & MMask;
			FormatMask  <<= Count; FormatMask |= Mask & MMask;
		}

		private void ParseFormat()
		{
			foreach (var Part in _Format.Split(':'))
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

        public InstructionAttribute(String Format, String Name = "", String AssemblerFormat = "<NoAssemblerFormat>", AddressType _AddressType = InstructionAttribute.AddressType.None, InstructionType _InstructionType = InstructionType.Normal)
		{
			this.Name = Name;
			this._Format = Format;
            this.AssemblerFormat = AssemblerFormat;
			this._AddressType = _AddressType;
            this._InstructionType = _InstructionType;
			//ParseFormat();
		}

        public override string ToString()
        {
            return String.Format("InstructionAttribute({0}:{1,8:X}:{2,8:X})", Name, FormatValue, FormatMask);
        }
    }
}
