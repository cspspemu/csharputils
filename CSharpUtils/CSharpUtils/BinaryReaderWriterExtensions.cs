using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CSharpUtils
{
    static public class BinaryReaderWriterExtensions
    {
        static public uint ReadUint32Endian(this BinaryReader BinaryReader, Endianness Endian)
        {
            if (Endian == Endianness.LittleEndian)
            {
                return BitConverter.ToUInt32(BinaryReader.ReadBytes(4), 0);
            }
            else
            {
                // @TODO: Use stackalloc instead.
                byte[] Bytes = new byte[4];
                for (int n = 3; n >= 0; n--) Bytes[n] = BinaryReader.ReadByte();
                return BitConverter.ToUInt32(Bytes, 0);
            }
        }
    }
}
