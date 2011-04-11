using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpPspEmulator.Core.Cpu;
using System.Threading;

namespace CSharpPspEmulator.Hle.Modules
{
    class sceDisplay : PspModule
    {
        public void sceDisplaySetMode(CpuState CpuState)
        {
            Console.WriteLine("sceDisplaySetMode");
            //throw new NotImplementedException();
        }

        public void sceDisplayWaitVblankStart(CpuState CpuState)
        {
            Console.WriteLine("sceDisplayWaitVblankStart");
            //Thread.Sleep(10000);
            //throw new NotImplementedException();
        }
    }
}
