using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NVorbis.jorbis
{
	abstract class FuncFloor{

	  public static FuncFloor[] floor_P= {new Floor0(), new Floor1()};

	  abstract void pack(Object i, Buffer opb);

	  abstract Object unpack(Info vi, Buffer opb);

	  abstract Object look(DspState vd, InfoMode mi, Object i);

	  abstract void free_info(Object i);

	  abstract void free_look(Object i);

	  abstract void free_state(Object vs);

	  abstract int forward(Block vb, Object i, float[] in, float[] out, Object vs);

	  abstract Object inverse1(Block vb, Object i, Object memo);

	  abstract int inverse2(Block vb, Object i, Object memo, float[] out);
	}

}
