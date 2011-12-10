using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NVorbis.jorbis
{
	abstract class FuncResidue
	{
	  public static FuncResidue[] residue_P= {new Residue0(), new Residue1(), new Residue2()};
	  abstract internal void pack(Object vr, NVorbis.jogg.Buffer opb);
	  abstract internal Object unpack(Info vi, NVorbis.jogg.Buffer opb);
	  abstract internal Object look(DspState vd, InfoMode vm, Object vr);
	  abstract internal void free_info(Object i);
	  abstract internal void free_look(Object i);
	  abstract internal int inverse(Block vb, Object vl, float[][] In, int[] nonzero, int ch);
	}

}
