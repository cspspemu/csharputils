using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NVorbis.jorbis
{
	abstract class FuncFloor{

	  public static FuncFloor[] floor_P= {new Floor0(), new Floor1()};
	  abstract internal void pack(Object i, NVorbis.jogg.Buffer opb);
	  abstract internal Object unpack(Info vi, NVorbis.jogg.Buffer opb);
	  abstract internal Object look(DspState vd, InfoMode mi, Object i);
	  abstract internal void free_info(Object i);
	  abstract internal void free_look(Object i);
	  abstract internal void free_state(Object vs);
	  abstract internal int forward(Block vb, Object i, float[] In, float[] Out, Object vs);
	  abstract internal Object inverse1(Block vb, Object i, Object memo);
	  abstract internal int inverse2(Block vb, Object i, Object memo, float[] Out);
	}

}
