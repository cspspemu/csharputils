using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpPspEmulator.Hle.Modules;

namespace CSharpPspEmulator.Hle
{
    public class SystemHle
    {
        public PspUID PspUID;
        public SyscallHandler SyscallHandler;
        public MemoryManager MemoryManager;
        public PspModuleManager PspModuleManager;

        public SystemHle()
        {
            this.PspUID = new PspUID();
            this.MemoryManager = new MemoryManager();
            this.PspModuleManager = new PspModuleManager();
            this.SyscallHandler = new SyscallHandler(this);
        }
    }
}
