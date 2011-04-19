using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq;

namespace CSharpUtils
{
    /// <summary>
    /// http://stackoverflow.com/questions/1440392/use-byte-as-key-in-dictionary
    /// </summary>
    public class ByteArrayEqualityComparer : IEqualityComparer<byte[]>
    {
        public bool Equals(byte[] left, byte[] right)
        {
            if (left == null || right == null) return left == right;
            return left.SequenceEqual(right);
        }

        public int GetHashCode(byte[] key)
        {
            if (key == null) throw new ArgumentNullException("key");
            return key.Sum(Key => Key);
        }
    }
}
