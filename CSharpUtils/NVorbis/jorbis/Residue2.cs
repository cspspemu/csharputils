using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NVorbis.jorbis
{
	class Residue2 : Residue0
	{
		override internal int inverse(Block vb, Object vl, float[][] In, int[] nonzero, int ch)
		{
		int i=0;
		for(i=0; i<ch; i++)
		  if(nonzero[i]!=0)
			break;
		if(i==ch)
		  return (0); /* no nonzero vectors */

		return (_2inverse(vb, vl, In, ch));
	  }
	}

}
