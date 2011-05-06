using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpUtils
{
    static public class StringExtensions
    {
        static public String Substr(this String String, int StartIndex)
        {
            if (StartIndex < 0) StartIndex = String.Length + StartIndex;
            StartIndex = MathUtils.Clamp(StartIndex, 0, String.Length);
            return String.Substring(StartIndex);
        }

        static public String Substr(this String String, int StartIndex, int Length)
        {
            if (StartIndex < 0) StartIndex = String.Length + StartIndex;
            StartIndex = MathUtils.Clamp(StartIndex, 0, String.Length);
            var End = StartIndex + Length;
            if (Length < 0) Length = String.Length + Length - StartIndex;
            Length = MathUtils.Clamp(Length, 0, String.Length - StartIndex);
            return String.Substring(StartIndex, Length);
        }

        static public byte[] GetStringzBytes(this String String, Encoding Encoding)
        {
			return String.GetBytes(Encoding).Concat(new byte[] { 0 }).ToArray();
        }

		static public byte[] GetBytes(this String This, Encoding Encoding)
		{
			return Encoding.GetBytes(This);
		}
    }
}
