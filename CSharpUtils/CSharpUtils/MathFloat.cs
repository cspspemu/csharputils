using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpUtils
{
	unsafe public class MathFloat
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="Value"></param>
		/// <returns></returns>
		public static float Abs(float Value)
		{
			return (Value > 0) ? Value : -Value;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Value"></param>
		/// <returns></returns>
		public static float Floor(float Value)
		{
			return (float)Math.Floor((double)Value);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Value"></param>
		/// <returns></returns>
		public static float Ceil(float Value)
		{
			return (float)Math.Ceiling((double)Value);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Value"></param>
		/// <returns></returns>
		public static float Round(float Value)
		{
			return (float)Math.Round((double)Value);
		}

		/// <summary>
		/// Rounds x to the nearest integer value, using the current rounding mode.
		/// If the return value is not equal to x, the FE_INEXACT exception is raised.
		/// nearbyint performs the same operation, but does not set the FE_INEXACT exception.
		/// </summary>
		/// <param name="Value"></param>
		/// <returns></returns>
		public static float Rint(float Value)
		{
			return MathFloat.Round(Value);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Value"></param>
		/// <returns></returns>
		public static uint ReinterpretFloatAsUInt(float Value)
		{
			//Console.WriteLine("ReinterpretFloatAsUInt:{0}", Value);
			var Values = new float[1];
			Values[0] = Value;
			fixed (float *ValuePtr = &Values[0])
			{
				return *((uint*)ValuePtr);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Value"></param>
		/// <returns></returns>
		public static float ReinterpretUIntAsFloat(uint Value)
		{
			//Console.WriteLine("ReinterpretUIntAsFloat:{0}", Value);
			var Values = new uint[1];
			Values[0] = Value;
			fixed (uint* ValuePtr = &Values[0])
			{
				return *((float*)ValuePtr);
			}
		}
	}
}
