using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpPspEmulator.Core.Cpu;

namespace CSharpPspEmulator.Hle.Modules
{
    public class sceCtrl : PspModule
    {
        public void sceCtrlPeekBufferPositive(CpuState CpuState)
        {
            Console.WriteLine("sceCtrlPeekBufferPositive");
        }
    }
}
