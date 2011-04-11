using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpPspEmulator.Hle
{
    public class PspUID
    {
        uint LastUID = 1;
        Dictionary<uint, Object> Map = new Dictionary<uint,object>();

        public uint Alloc(Object Object)
        {
            uint UID = LastUID++;
            Map[UID] = Object;
            return UID;
        }

        public T Get<T>(uint UID)
        {
            return (T)Map[UID];
        }

        public void Free(uint UID)
        {
            Map.Remove(UID);
        }
    }
}
