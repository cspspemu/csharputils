using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using CSharpPspEmulator.Core.Cpu;
using CSharpPspEmulator.Core;
using CSharpPspEmulator.Hle.Modules;

namespace CSharpPspEmulator.Hle
{
    public class SyscallHandler
    {
        public ExecutionDelegate[] SyscallHandlers = new ExecutionDelegate[0x10000];

        public SyscallHandler(SystemHle SystemHle)
        {
            SyscallHandlers[0x206D] = (SystemHle.PspModuleManager.Get<ThreadManForUser>()).sceKernelCreateThread;
            SyscallHandlers[0x206F] = (SystemHle.PspModuleManager.Get<ThreadManForUser>()).sceKernelStartThread;
            SyscallHandlers[0x2071] = (SystemHle.PspModuleManager.Get<ThreadManForUser>()).sceKernelExitThread;

            SyscallHandlers[0x20BF] = (SystemHle.PspModuleManager.Get<UtilsForUser>()).sceKernelUtilsMt19937Init;
            SyscallHandlers[0x20C0] = (SystemHle.PspModuleManager.Get<UtilsForUser>()).sceKernelUtilsMt19937UInt;

            SyscallHandlers[0x213A] = (SystemHle.PspModuleManager.Get<sceDisplay>()).sceDisplaySetMode;
            SyscallHandlers[0x2147] = (SystemHle.PspModuleManager.Get<sceDisplay>()).sceDisplayWaitVblankStart;

            SyscallHandlers[0x2150] = (SystemHle.PspModuleManager.Get<sceCtrl>()).sceCtrlPeekBufferPositive;
        }

        public void Handle(CpuState CpuState, uint Code)
        {
            //Console.WriteLine("Syscall(" + Code + ")");
            var Handler = SyscallHandlers[Code];
            if (Handler == null)
            {
                throw (new Exception(String.Format("Unknown syscall '{0:X}'", Code)));
            }
            else
            {
                Handler(CpuState);
            }
        }
    }
}
