using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpUtils.Extensions
{
	static public class StructExtensions
	{
		static public string ToStringDefault<T>(this T Struct) where T : struct
		{
			var Ret = "";
			Ret += typeof(T).Name;
			Ret += "(";
			var MemberCount = 0;
			foreach (var FieldInfo in typeof(T).GetFields())
			{
				if (MemberCount > 0)
				{
					Ret += ",";
				}
				Ret += FieldInfo.Name;
				Ret += "=";
				Ret += FieldInfo.GetValue(Struct);
				MemberCount++;
			}
			Ret += ")";
			return Ret;
		}
	}
}
