using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NVorbis.jorbis
{
	// psychoacoustic setup
	internal class PsyInfo
	{
		internal int athp;
		internal int decayp;
		internal int smoothp;
		internal int noisefitp;
		internal int noisefit_subblock;
		internal float noisefit_threshdB;

		internal float ath_att;

		internal int tonemaskp;
		internal float[] toneatt_125Hz = new float[5];
		internal float[] toneatt_250Hz = new float[5];
		internal float[] toneatt_500Hz = new float[5];
		internal float[] toneatt_1000Hz = new float[5];
		internal float[] toneatt_2000Hz = new float[5];
		internal float[] toneatt_4000Hz = new float[5];
		internal float[] toneatt_8000Hz = new float[5];

		internal int peakattp;
		internal float[] peakatt_125Hz = new float[5];
		internal float[] peakatt_250Hz = new float[5];
		internal float[] peakatt_500Hz = new float[5];
		internal float[] peakatt_1000Hz = new float[5];
		internal float[] peakatt_2000Hz = new float[5];
		internal float[] peakatt_4000Hz = new float[5];
		internal float[] peakatt_8000Hz = new float[5];

		internal int noisemaskp;
		internal float[] noiseatt_125Hz = new float[5];
		internal float[] noiseatt_250Hz = new float[5];
		internal float[] noiseatt_500Hz = new float[5];
		internal float[] noiseatt_1000Hz = new float[5];
		internal float[] noiseatt_2000Hz = new float[5];
		internal float[] noiseatt_4000Hz = new float[5];
		internal float[] noiseatt_8000Hz = new float[5];

		internal float max_curve_dB;

		internal float attack_coeff;
		internal float decay_coeff;

		internal void free()
		{
		}
	}

}
