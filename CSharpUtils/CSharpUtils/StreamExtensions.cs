using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

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

        public static T ReadStruct<T>(this Stream stream) where T : struct
        {
            var Size = Marshal.SizeOf(typeof(T));
            var Buffer = new byte[Size];
            stream.Read(Buffer, 0, Size);
            return StructUtils.BytesToStruct<T>(Buffer);
        }
	}
}
