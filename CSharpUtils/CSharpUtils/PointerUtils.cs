using System;
using System.Collections.Generic;
using System.Linq;
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

		static public void Memset(byte* Pointer, byte Value, int Count)
		{
			for (int n = 0; n < Count; n++)
			{
				Pointer[n] = Value;
			}
		}
	}
}
