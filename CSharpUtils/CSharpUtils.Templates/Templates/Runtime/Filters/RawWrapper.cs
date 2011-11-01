using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpUtils.Templates.Runtime.Filters
{
	public class RawWrapper
	{
		protected Object Object;

		static public RawWrapper Get(Object Object)
		{
			if (Object is RawWrapper)
			{
				return (RawWrapper)Object;
			}
			return new RawWrapper(Object);
		}

		protected RawWrapper(Object Object)
		{
			this.Object = Object;
		}

		public override bool Equals(object obj)
		{
			return Object.Equals(obj);
		}

		public override int GetHashCode()
		{
			return Object.GetHashCode();
		}

		public override string ToString()
		{
			return Object.ToString();
		}
	}
}
