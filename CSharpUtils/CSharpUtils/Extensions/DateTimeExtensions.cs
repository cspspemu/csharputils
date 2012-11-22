using System;

public static class DateTimeExtensions
{
	public static long GetTotalNanoseconds(this DateTime DateTime)
	{
		return DateTime.Ticks * 10;
	}

	public static long GetTotalMicroseconds(this DateTime DateTime)
	{
		return DateTime.GetTotalNanoseconds() * 1000;
	}
}
