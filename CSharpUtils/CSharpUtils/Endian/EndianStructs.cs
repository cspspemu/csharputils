using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpUtils.Endian
{
	public struct uint_be
	{
		private uint _InternalValue;

		public uint NativeValue
		{
			set
			{
				_InternalValue = MathUtils.ByteSwap(value);
			}
			get
			{
				return MathUtils.ByteSwap(_InternalValue);
			}
		}

		public static implicit operator uint(uint_be that)
		{
			return that.NativeValue;
		}

		public static implicit operator uint_be(uint that)
		{
			return new uint_be()
			{
				NativeValue = that,
			};
		}
	}

	public struct uint_le
	{
		private uint _InternalValue;

		public uint NativeValue
		{
			set
			{
				//_InternalValue = MathUtils.ByteSwap(value);
				_InternalValue = value;
			}
			get
			{
				//return MathUtils.ByteSwap(_InternalValue);
				return _InternalValue;
			}
		}

		public static implicit operator uint(uint_le that)
		{
			return that.NativeValue;
		}

		public static implicit operator uint_le(uint that)
		{
			return new uint_le()
			{
				NativeValue = that,
			};
		}
	}
}
