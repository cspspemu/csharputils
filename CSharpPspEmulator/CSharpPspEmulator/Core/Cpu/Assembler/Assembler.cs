using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpPspEmulator.Core.Cpu.Assembler
{
    public class Assembler
    {
        public Assembler()
        {
            foreach (var Attribute in InstructionAttribute.GetInstructionAttributeMethods(typeof(Cpu)))
            {
                Console.WriteLine(Attribute.Name + ": " + Attribute.AssemblerFormat);
            }
        }
    }
}
