using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace CSharpUtils.Drawing
{
	/// <summary>
	/// 
	/// </summary>
	public struct RGBA
	{
		/// <summary>
		/// 
		/// </summary>
		public byte R, G, B, A;
	}

	/// <summary>
	/// 
	/// </summary>
	public struct ARGB_Rev
	{
		/// <summary>
		/// 
		/// </summary>
		public byte B, G, R, A;

		public static implicit operator ARGB_Rev(Color Col)
		{
			return new ARGB_Rev() { R = Col.R, G = Col.G, B = Col.B, A = Col.A };
		}

		public static implicit operator Color(ARGB_Rev Col)
		{
			return Color.FromArgb(Col.A, Col.R, Col.G, Col.B);
		}

		public override string ToString()
		{
			return String.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", R, G, B, A);
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public struct ARGB
	{
		/// <summary>
		/// 
		/// </summary>
		public byte A, R, G, B;
	}
}
