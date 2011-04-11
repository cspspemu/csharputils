using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CSharpPspEmulator.Core.Cpu
{
    public class ThreadInfo
    {
        public Thread Thread;
        public String Name;
        public ulong ExecutedInstructionCount;

        public ThreadInfo(String Name = "Main", Thread Thread = null)
        {
            if (Thread == null)
            {
                Thread = Thread.CurrentThread;
            }
            this.Name = Name;
            this.Thread = Thread;
            this.ExecutedInstructionCount = 0;
        }

        public bool Running = false;

        public int ID
        {
            get
            {
                return Thread.ManagedThreadId;
            }
        }
    }
}
