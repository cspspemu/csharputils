using System;
using System.Timers;

namespace CSharpUtils
{
	public static class TimeSpanUtils
	{
		public static TimeSpan FromMicroseconds(long Microseconds)
		{
			return TimeSpan.FromMilliseconds((double)Microseconds / (double)1000.0);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Action"></param>
		public static void InfiniteLoopDetector(string Description, Action Action)
		{
			using (var Timer = new Timer(4.0 * 1000))
			{
				bool Cancel = false;
				Timer.Elapsed += (sender, e) =>
				{
					if (!Cancel)
					{
						Console.WriteLine("InfiniteLoop Detected! : {0} : {1}", Description, e.SignalTime);
					}
				};
				Timer.AutoReset = false;
				Timer.Start();
				try
				{
					Action();
				}
				finally
				{
					Cancel = true;
					Timer.Enabled = false;
					Timer.Stop();
				}
			}
		}
	}
}
