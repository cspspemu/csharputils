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
		public enum AddressType {
			None,
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
					case "rs": case "rt": case "rd":
						FormatInsert(0, 0, 5);
					break;
					case "imm16":
						FormatInsert(0, 0, 16);
					break;
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

		public InstructionAttribute(String Format, AddressType _AddressType)
		{
			//this.Name = Name;
			this.Format = Format;
			this._AddressType = _AddressType;
			ParseFormat();
		}
	}
}
