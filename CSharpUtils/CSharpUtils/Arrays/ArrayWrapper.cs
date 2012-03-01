using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpUtils.Arrays
{
	public class ArrayWrapper<TType> : IArray<TType>
	{
		TType[] Array;

		public ArrayWrapper()
		{
			this.Array = new TType[0];
		}

		public ArrayWrapper(TType[] Array)
		{
			this.Array = Array;
		}

		public TType this[int Index]
		{
			get
			{
				return Array[Index];
			}
			set
			{
				Array[Index] = value;
			}
		}

		static public implicit operator TType[](ArrayWrapper<TType> ArrayWrapper)
		{
			return ArrayWrapper.Array; 
		}

		static public implicit operator ArrayWrapper<TType>(TType[] Array)
		{
			return new ArrayWrapper<TType>(Array);
		}

		public int Length
		{
			get { return Array.Length; }
		}


		public TType[] GetArray()
		{
			return Array;
		}
	}
}
