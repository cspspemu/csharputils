using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpUtils.Extensions
{
	static public class DateTimeExtensions
	{
		static public long GetTotalNanoseconds(this DateTime DateTime)
		{
			return DateTime.Ticks * 10;
		}
	}
}
