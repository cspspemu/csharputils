using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpPspEmulator.Core.Cpu;
using System.Runtime.InteropServices;
using System.Threading;
using CSharpPspEmulator.Core;

namespace CSharpPspEmulator.Hle.Modules
{
    unsafe public class ThreadManForUser : PspModule
    {
        [PspFunction(UID: 0x446D8DE6)]
        public void sceKernelCreateThread(CpuState CpuState)
        {
            // SceUID
            // String Name, uint /*SceKernelThreadEntry*/ entry, int initPriority, int stackSize, uint /*SceUInt*/ attr, uint /*SceKernelThreadOptParamPtr*/ option

            String Name = Marshal.PtrToStringAnsi(CpuState.Memory.GetIntPtr(CpuState.GetArgument(0)));
            uint EntryPtr = CpuState.GetArgument(1);
            uint Priority = CpuState.GetArgument(2);
            uint StackSize = CpuState.GetArgument(3);
            uint OldGP = CpuState.RegistersCpu[28];
            uint Attr = CpuState.GetArgument(4);
            uint SceKernelThreadOptParamPtr = CpuState.GetArgument(5);
            // @TODO: Alloc Stack.

            CpuState NewCpuState = null;

            Thread Thread = new Thread(delegate()
            {
                NewCpuState.RegistersCpu[28] = OldGP;
                NewCpuState.RegistersCpu[29] = NewCpuState.SystemHle.MemoryManager.AllocStack(StackSize);
                NewCpuState.Execute();
            });
            NewCpuState = new CpuState(CpuState.SystemHle, CpuState.RegistersCpu, CpuState.CpuBase, CpuState.Memory, new ThreadInfo("PSP: " + Name, Thread));

            CpuState.SetReturnValue(CpuState.SystemHle.PspUID.Alloc(NewCpuState));
        }

        [PspFunction(UID: 0xF475845D)]
        public void sceKernelStartThread(CpuState CpuState)
        {
            var NewCpuState = CpuState.SystemHle.PspUID.Get<CpuState>(CpuState.GetArgument(0));
            NewCpuState.ThreadInfo.Thread.Start();

            // @TODO: Wait X Instructions executed
            while (NewCpuState.ThreadInfo.Running && NewCpuState.ThreadInfo.ExecutedInstructionCount < 1000)
            {
                Thread.Yield();
            }

            //callLibrary("ThreadManForUser", "sceKernelStartThread"); 
            //CpuState.PC = CpuState.GetArgument(0);
        }

        [PspFunction(UID: 0xAA73C935)]
        public void sceKernelExitThread(CpuState CpuState)
        {
            throw (new PspExitThreadException());
        }
    }
}
