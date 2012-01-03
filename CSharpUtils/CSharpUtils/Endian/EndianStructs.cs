using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpUtils.Endian
{
	public struct ulong_be
	{
		private ulong _InternalValue;

		public ulong NativeValue
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

		public static implicit operator ulong(ulong_be that)
		{
			return that.NativeValue;
		}

		public static implicit operator ulong_be(ulong that)
		{
			return new ulong_be()
			{
				NativeValue = that,
			};
		}

		public override string ToString()
		{
			return NativeValue.ToString();
		}
	}

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

		public override string ToString()
		{
			return NativeValue.ToString();
		}
	}

	public struct ushort_be
	{
		private ushort _InternalValue;

		public ushort NativeValue
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

		public static implicit operator ushort(ushort_be that)
		{
			return that.NativeValue;
		}

		public static implicit operator ushort_be(ushort that)
		{
			return new ushort_be()
			{
				NativeValue = that,
			};
		}

		public override string ToString()
		{
			return NativeValue.ToString();
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

		public override string ToString()
		{
			return NativeValue.ToString();
		}
	}
}
