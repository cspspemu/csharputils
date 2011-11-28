using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CSharpUtils
{
	unsafe public class PointerUtils
	{
		static public String PtrToString(byte* Pointer, Encoding Encoding)
		{
			if (Pointer == null) return null;
			List<byte> Bytes = new List<byte>();
			for (; *Pointer != 0; Pointer++) Bytes.Add(*Pointer);
			return Encoding.GetString(Bytes.ToArray());
		}

		static public String PtrToString(byte* Pointer, int Length, Encoding Encoding)
		{
			if (Pointer == null) return null;
			List<byte> Bytes = new List<byte>();
			for (int n = 0; n < Length; n++) Bytes.Add(Pointer[n]);
			return Encoding.GetString(Bytes.ToArray());
		}

		static public void StoreStringOnPtr(string String, Encoding Encoding, byte* Pointer, int PointerMaxLength = 0x10000)
		{
			var Bytes = Encoding.GetBytes(String);
			foreach (var Byte in Bytes)
			{
				*Pointer++ = Byte;
			}
			*Pointer++ = 0;
		}

		static public void Memset(byte* Pointer, byte Value, int Count)
		{
			for (int n = 0; n < Count; n++)
			{
				Pointer[n] = Value;
			}
		}

		public static void Memcpy(byte* Destination, byte* Source, int Size)
		{
			//Marshal.Copy(new IntPtr(Source), new IntPtr(Destination), 0, Size);
			for (int n = 0; n < Size; n++) Destination[n] = Source[n];
		}
	}
}
