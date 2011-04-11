using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpPspEmulator.Hle
{
    class PspFunctionAttribute : Attribute
    {
        uint UID;

        public PspFunctionAttribute(uint UID)
        {
            this.UID = UID;
        }
    }
}
