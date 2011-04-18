using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace CSharpUtils
{
	static public class StreamExtensions
	{
		static public String ReadAllContentsAsString(this Stream Stream, Encoding Encoding = null)
		{
			if (Encoding == null) Encoding = Encoding.UTF8;
			return Encoding.GetString(Stream.ReadAll());
		}

		static public byte[] ReadAll(this Stream Stream)
		{
			var Data = new byte[Stream.Length];
			Stream.Read(Data, 0, Data.Length);
			return Data;
		}

        static public byte[] ReadBytes(this Stream Stream, int ToRead)
        {
            var Buffer = new byte[ToRead];
            var Readed = Stream.Read(Buffer, 0, ToRead);
            if (Readed != ToRead) throw(new Exception("Unable to read " + ToRead + " bytes, readed " + Readed + "."));
            return Buffer;
        }

        static public void WriteBytes(this Stream Stream, byte[] Bytes)
        {
            Stream.Write(Bytes, 0, Bytes.Length);
        }

        static public String ReadStringz(this Stream Stream, int ToRead = -1, Encoding Encoding = null)
        {
            if (Encoding == null) Encoding = Encoding.ASCII;
            if (ToRead == -1)
            {
                var Temp = new MemoryStream();
                byte Byte;
                while ((Byte = (byte)Stream.ReadByte()) != 0)
                {
                    Temp.WriteByte(Byte);
                }
                return Encoding.GetString(Temp.ToArray());
            }
            else
            {
                return Encoding.GetString(Stream.ReadBytes(ToRead)).TrimEnd('\0');
            }
        }

        public static T ReadStruct<T>(this Stream Stream) where T : struct
        {
            var Size = Marshal.SizeOf(typeof(T));
            var Buffer = new byte[Size];
            Stream.Read(Buffer, 0, Size);
            return StructUtils.BytesToStruct<T>(Buffer);
        }

        public static void WriteStruct<T>(this Stream Stream, T Struct) where T : struct
        {
            byte[] Bytes = StructUtils.StructToBytes(Struct);
            Stream.Write(Bytes, 0, Bytes.Length);
        }
	}
}
