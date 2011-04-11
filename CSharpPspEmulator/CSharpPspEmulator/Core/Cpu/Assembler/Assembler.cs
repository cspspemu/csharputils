using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpPspEmulator.Core.Cpu.Interpreter;
using System.Text.RegularExpressions;

namespace CSharpPspEmulator.Core.Cpu.Assembler
{
    public class Assembler
    {
        Dictionary<String, InstructionAttribute> Instructions;

        public Assembler()
        {
            Instructions = new Dictionary<string, InstructionAttribute>();

            //new Regex();
            foreach (var Attribute in InstructionAttribute.GetInstructionAttributeMethods(typeof(InterpretedCpu)))
            {
                //Console.WriteLine(Attribute.Name);
                Instructions.Add(Attribute.Name, Attribute);
            }
        }

        public List<InstructionData> ProcessInstruction(InstructionAttribute Attribute, String _InstructionParams)
        {
            var List = new List<InstructionData>();

            InstructionData InstructionData = Attribute.TableValue;

            var InstructionName = Attribute.Name;
            var InstructionFormat = Attribute.AssemblerFormat;
            var InstructionParams = _InstructionParams.Trim();

            var TypeNames = new List<String>();
            var InstructionFormatRegexStr = InstructionFormat.Replace(Regex.Escape(","), @"\s*,\s*");
            InstructionFormatRegexStr = Regex.Replace(InstructionFormatRegexStr, @"\{\$(\w+)\}", delegate(Match Match)
            {
                var TypeName = Match.Groups[1].Value;
                TypeNames.Add(TypeName);

                switch (TypeName)
                {
                    // Normal registers.
                    case "s": case "t": case "d":
                        return @"(r\d+|\w{2})";

                    // Immediate value.
                    case "imm": case "h":
                        return @"(\-?\d+)";
                    default:
                        throw(new Exception("Invalid format '" + Match + "'"));
                }
            });
            var InstructionFormatRegex = new Regex(InstructionFormatRegexStr);
            var Groups = InstructionFormatRegex.Match(InstructionParams).Groups;

            /*
            Console.WriteLine(InstructionName);
            Console.WriteLine(InstructionFormat);
            Console.WriteLine(InstructionFormatRegexStr);
            Console.WriteLine(InstructionParams);
            */

            for (int n = 0; n < TypeNames.Count; n++)
            {
                var TypeName = TypeNames[n];
                var Value = Groups[n + 1].Value;
                switch (TypeName)
                {
                    case "s": InstructionData.RS = (uint)RegistersCpu.RegisterNameToIndex(Value); break;
                    case "t": InstructionData.RT = (uint)RegistersCpu.RegisterNameToIndex(Value); break;
                    case "d": InstructionData.RD = (uint)RegistersCpu.RegisterNameToIndex(Value); break;
                    case "h": InstructionData.PS = (uint)Convert.ToInt16(Value); break;
                    case "imm": InstructionData.IMM = (short)Convert.ToInt64(Value); break;
                    case "immu": InstructionData.IMMU = (ushort)Convert.ToInt64(Value); break;
                    default: throw (new Exception("Unknown TypeName '" + TypeName + "'"));
                }
            }

            List.Add(InstructionData);

            return List;
        }

        public List<InstructionData> Assemble(String Line)
        {
            
            var Expr = new Regex(@"^\s*((?<Name>\S+)(?:$|\s+(?<Params>[^#]*))?(?<Comment>#.*)?$)");
            if (!Expr.IsMatch(Line)) throw(new Exception("Invalid line to assemble"));
            var Match = Expr.Match(Line);
            var InstructionName = Match.Groups["Name"].Value;
            var InstructionParams = Match.Groups["Params"].Value;
            var Comment = Match.Groups["Comment"].Value;

            return ProcessInstruction(Instructions[InstructionName], InstructionParams);
        }
    }
}
