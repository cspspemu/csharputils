using CSharpUtils.Compression.Lz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpUtils.Ext.Compression.Lz
{
	public class Matcher
	{
		static public void HandleLz(byte[] Input, int StartPosition, int MinSearchSize, int MaxSearchSize, int MaxDistance, bool AllowOverlapping, Action<int, byte> ByteCallback, Action<int, int, int> LzCallback)
		{
			HandleLzRle(Input, StartPosition, MinSearchSize, MaxSearchSize, MaxDistance, 0, 0, AllowOverlapping, ByteCallback, LzCallback, null);
		}

		static public void HandleLzRle(byte[] Input, int StartPosition, int MinSearchSize, int MaxSearchSize, int MaxDistance, int MinRle, int MaxRle, bool AllowOverlapping, Action<int, byte> ByteCallback, Action<int, int, int> LzCallback, Action<int, byte, int> RleCallback)
		{
			var LzBuffer = new LzBuffer(MinSearchSize);
			LzBuffer.AddBytes(Input);
			RleMatcher RleMatcher = null;
			bool UseRle = (RleCallback != null);

			if (UseRle)
			{
				RleMatcher = new RleMatcher(Input);
				RleMatcher.Offset = StartPosition;
			}

			for (int n = StartPosition; n < Input.Length; )
			{
				var Result = LzBuffer.FindMaxSequence(n, n, MaxDistance, MinSearchSize, MaxSearchSize, AllowOverlapping);
				
				int RleLength = -1;

				if (UseRle)
				{
					RleLength = RleMatcher.Length;
					if (RleLength < MinRle) RleLength = 0;
					if (RleLength > MaxRle) RleLength = MaxRle;
				}

				if (Result.Found && (!UseRle || (Result.Size > RleLength)))
				{
					//Console.WriteLine("RLE: {0}", RleLength);
					LzCallback(n, Result.Offset - n, Result.Size);
					n += Result.Size;
					if (UseRle) RleMatcher.Skip(Result.Size);
					continue;
				}

				if (UseRle && (RleLength >= MinRle))
				{
					RleCallback(n, Input[n], RleLength);
					n += RleLength;
					RleMatcher.Skip(RleLength);
					continue;
				}

				ByteCallback(n, Input[n]);
				n += 1;
				if (UseRle) RleMatcher.Skip(1);
			}
		}
	}
}
