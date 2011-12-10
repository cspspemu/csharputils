using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NVorbis.jorbis
{
	abstract class FuncTime
	{
		public static FuncTime[] time_P= {new Time0()};
		abstract internal void pack(Object i, NVorbis.jogg.Buffer opb);
		abstract internal Object unpack(Info vi, NVorbis.jogg.Buffer opb);
		abstract internal Object look(DspState vd, InfoMode vm, Object i);
		abstract internal void free_info(Object i);
		abstract internal void free_look(Object i);
		abstract internal int inverse(Block vb, Object i, float[] In, float[] Out);
	}

}
