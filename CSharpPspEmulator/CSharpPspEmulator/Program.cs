using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpPspEmulator.Core;
using CSharpPspEmulator.Core.Cpu;
using CSharpPspEmulator.Core.Cpu.Assembler;

namespace CSharpPspEmulator
{
	class Program
	{
		static void Main(string[] args)
		{
            /*
            var Disassembler = new Disassembler();
            Console.WriteLine(Disassembler.Disassemble(new Disassembler.State(0x00000020, 0, false)));
            Console.WriteLine(Disassembler.Disassemble(new Disassembler.State(0x00000020, 0, true)));
            */

            var Assembler = new Assembler();
            Console.WriteLine(Assembler.Assemble("addi r1, r0, 1000")[0]);

            /*
			var Registers = new RegistersCpu();
			var Memory = new Memory();
			var Cpu = new Cpu();
			var CpuState = new CpuState(Registers, Cpu, Memory);
			//var Cpu = new Cpu();
			CpuState.InstructionData = 0x20;
			Cpu.Execute(CpuState);
            */
            Console.ReadKey();
        }
	}
}
