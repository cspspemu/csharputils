using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NVorbis.jorbis
{
	unsafe internal class Util
	{
		static internal int ilog(int _v)
		{
			uint v = (uint)_v;
			uint ret = 0;
			while (v != 0)
			{
				ret++;
				v >>= 1;
			}
			return (int)(ret);
		}

		static internal int ilog2(int _v)
		{
			uint v = (uint)_v;
			uint ret = 0;
			while (v > 1)
			{
				ret++;
				v >>= 1;
			}
			return (int)(ret);
		}

		static internal int icount(int _v)
		{
			uint v = (uint)_v;

			uint ret = 0;
			while (v != 0)
			{
				ret += (v & 1);
				v >>= 1;
			}
			return (int)(ret);
		}

		static internal int floatToIntBits(float v)
		{
			float[] vv = new float[1];
			vv[0] = v;
			fixed (float* ptr = vv)
			{
				return *(int*)ptr;
			}
		}

		static internal float intBitsToFloat(int v)
		{
			int[] vv = new int[1];
			vv[0] = v;
			fixed (int* ptr = vv)
			{
				return *(float*)ptr;
			}
		}

		static internal Encoding InternalEncoding
		{
			get
			{
				return Encoding.ASCII;
			}
		}
	}

}
