using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NVorbis.jorbis
{
	class Residue1 : Residue0
	{

	  int inverse(Block vb, Object vl, float[][] In, int[] nonzero, int ch){
		int used=0;
		for(int i=0; i<ch; i++){
		  if(nonzero[i]!=0){
			In[used++]=In[i];
		  }
		}
		if(used!=0){
			return (_01inverse(vb, vl, In, used, 1));
		}
		else{
		  return 0;
		}
	  }
	}

}
