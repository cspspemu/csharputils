using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NVorbis.jorbis
{
	internal class PsyLook
	{
		internal int n;
		internal PsyInfo vi;

		internal float[][][] tonecurves;
		internal float[][] peakatt;
		internal float[][][] noisecurves;

		internal float[] ath;
		internal int[] octave;

		internal void init(PsyInfo vi, int n, int rate)
		{
		}
	}

}
