using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

static public class TimeSpanExtensions
{
	static public long GetTotalMicroseconds(this TimeSpan TimeSpan)
	{
		return (long)TimeSpan.TotalMilliseconds * 1000;
	}
		
}
