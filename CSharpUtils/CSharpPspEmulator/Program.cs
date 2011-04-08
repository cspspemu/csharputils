using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpPspEmulator.Core;

namespace CSharpPspEmulator
{
	class Program
	{
		static void Main(string[] args)
		{
			var Registers = new Registers();
			var Memory = new Memory();
			var Cpu = new Cpu();
			var CpuState = new CpuState(Registers, Cpu, Memory);
			//var Cpu = new Cpu();
			CpuState.InstructionData = 0x20;
			Cpu.Execute(CpuState);
			Console.ReadKey();
		}
	}
}
