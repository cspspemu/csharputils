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
					InstructionList.Add(Attributes[0]);
				}
				//Type.InvokeMember();
				//Console.WriteLine([0]);
			}
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

        public InstructionAttribute(String Format, AddressType _AddressType, InstructionType _InstructionType = InstructionType.Normal)
		{
			//this.Name = Name;
			this.Format = Format;
			this._AddressType = _AddressType;
            this._InstructionType = _InstructionType;
			ParseFormat();
		}
	}
}
