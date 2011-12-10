using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NVorbis.jorbis
{
	abstract class FuncTime{
	  public static FuncTime[] time_P= {new Time0()};

	  abstract void pack(Object i, Buffer opb);

	  abstract Object unpack(Info vi, Buffer opb);

	  abstract Object look(DspState vd, InfoMode vm, Object i);

	  abstract void free_info(Object i);

	  abstract void free_look(Object i);

	  abstract int inverse(Block vb, Object i, float[] in, float[] out);
	}

}
