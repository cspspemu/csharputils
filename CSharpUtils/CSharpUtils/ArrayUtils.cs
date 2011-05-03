using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpUtils
{
	static public class ArrayUtils
	{
		static public T[] Concat<T>(this T[] Left, T[] Right)
		{
			var Return = new T[Left.Length + Right.Length];
			Left.CopyTo(Return, 0);
			Right.CopyTo(Return, Left.Length);
			return Return;
		}

		static public T[] Concat<T>(this T[] Left, T[] Right, int RightOffset, int RightLength)
		{
			var Return = new T[Left.Length + RightLength];
			Array.Copy(Left, 0, Return, 0, Left.Length);
			Array.Copy(Right, RightOffset, Return, Left.Length, RightLength);
			return Return;
		}

		static public T[] Slice<T>(this T[] This, int Start, int Length)
		{
			if (Start < 0) Start = This.Length - Start;
			Start = Math.Min(Math.Max(0, Start), This.Length);
			Length = Math.Min(This.Length - Start, Length);
			var Return = new T[Length];
			Array.Copy(This, Start, Return, 0, Length);
			return Return;
		}

		static public T[] Slice<T>(this T[] This, int Start)
		{
			return This.Slice(Start, This.Length - Start);
		}
	}
}
