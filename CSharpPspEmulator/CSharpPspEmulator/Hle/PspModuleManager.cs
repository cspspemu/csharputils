using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace CSharpPspEmulator.Hle
{
    public class PspModuleManager
    {
        public PspModule Get(String Name)
        {
            return (PspModule)Assembly.GetExecutingAssembly().CreateInstance("CSharpPspEmulator.Hle.Modules." + Name);
        }

        public T Get<T>()
        {
            return (T)Assembly.GetExecutingAssembly().CreateInstance("CSharpPspEmulator.Hle.Modules." + typeof(T).Name);
        }
    }
}
