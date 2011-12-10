using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NVorbis.jorbis
{
	class PsyLook
	{
		int n;
		PsyInfo vi;

		float[][][] tonecurves;
		float[][] peakatt;
		float[][][] noisecurves;

		float[] ath;
		int[] octave;

		void init(PsyInfo vi, int n, int rate)
		{
		}
	}

}
