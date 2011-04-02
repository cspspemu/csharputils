using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpUtils.Process
{
    class MainProcess : Process
    {
        public MainProcess()
        {
            currentExecutingProcess = this;
        }

        public bool Alive
        {
            get
            {
                return childs.Count > 0;
            }
        }

        protected override void main()
        {
            while (Alive)
            {
                Yield();
            }
        }
    }
}
