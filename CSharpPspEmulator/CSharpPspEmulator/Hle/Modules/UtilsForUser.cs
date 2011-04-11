using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpPspEmulator.Core.Cpu;

namespace CSharpPspEmulator.Hle.Modules
{
    class UtilsForUser : PspModule
    {
        /// <summary><![CDATA[
        /// Function to initialise a mersenne twister context.
        /// 
        /// @param ctx - Pointer to a context
        /// @param seed - A seed for the random function.
        /// 
        /// @par Example:
        /// @code
        /// SceKernelUtilsMt19937Context ctx;
        /// sceKernelUtilsMt19937Init(&ctx, time(NULL));
        /// u23 rand_val = sceKernelUtilsMt19937UInt(&ctx);
        /// @endcode
        /// 
        /// @return < 0 on error.
        /// ]]></summary>
        /// <param name="CpuState"></param>
        public void sceKernelUtilsMt19937Init(CpuState CpuState)
        {
            //throw(new NotImplementedException());
            Console.WriteLine("sceKernelUtilsMt19937Init");

            CpuState.SetReturnValue(0);
        }

        public void sceKernelUtilsMt19937UInt(CpuState CpuState)
        {
            //throw (new NotImplementedException());
            //Console.WriteLine("sceKernelUtilsMt19937UInt");

            CpuState.SetReturnValue(0);
        }
    }
}
