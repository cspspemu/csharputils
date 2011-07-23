using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpUtils
{
	public class ProduceConsumeBuffer<T>
	{
		public T[] Items = new T[0];

		public void Produce(T[] NewBytes)
		{
			Items = Items.Concat(NewBytes);
		}

		public void Produce(T[] NewBytes, int Offset, int Length)
		{
			Items = Items.Concat(NewBytes, Offset, Length);
		}

        public int ConsumeRemaining
        {
            get
            {
                return Items.Length;
            }
        }

		public T[] Consume(int Length)
		{
			var Return = Items.Slice(0, Length);
			Items = Items.Slice(Length);
			return Return;
		}

		public int IndexOf(T Item)
		{
			return Array.IndexOf(Items, Item);
		}
	}
}
