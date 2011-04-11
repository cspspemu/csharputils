using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpPspEmulator.Core
{
    public class PspException : Exception
    {
    }

    public class PspExitThreadException : PspException
    {
    }

    public class PspDebugBreakException : PspException
    {
    }
}
