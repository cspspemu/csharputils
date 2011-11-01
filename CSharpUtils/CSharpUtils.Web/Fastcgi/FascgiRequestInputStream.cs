using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CSharpUtils.Fastcgi
{
    public class FascgiRequestInputStream : MemoryStream
    {
        public bool Finalized = false;
    }
}
