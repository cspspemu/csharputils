using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpPspEmulator.Core;
using CSharpPspEmulator.Core.Cpu;
using CSharpPspEmulator.Core.Cpu.Assembler;
using CSharpPspEmulator.Hle.Elf;
using System.IO;
using CSharpPspEmulator.Core.Cpu.Interpreter;
using CSharpPspEmulator.Hle;

namespace CSharpPspEmulator
{
	class Program
	{
		static void Main(string[] args)
		{
            var InterpretedCpu = new InterpretedCpu();
            var RegistersCpu = new RegistersCpu();
            var Memory = new Memory();
            var SystemHle = new SystemHle();
            var CpuState = new CpuState(SystemHle, RegistersCpu, InterpretedCpu, Memory);

            var ElfFile = new FileStream("../../../Demos/minifire.elf", FileMode.Open);
            var Elf = new ElfLoader();
            Elf.Load(ElfFile);

            byte[] Data = new byte[0x1EF];
            ElfFile.Position = 0xB0;
            ElfFile.Read(Data, 0, Data.Length);
            Memory.WriteBytes(0x08900000, Data, 0, Data.Length);
            CpuState.PC = 0x08900008;
            CpuState.Execute();

            Console.ReadKey();
        }
	}
}
