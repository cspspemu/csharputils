using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;

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

			//FieldInfo fi;
			//PropertyInfo pi;
			foreach (var MemberInfo in typeof(T).GetMembers())
			{
				bool ValueSet = false;
				object Value = null;

				if (MemberInfo is FieldInfo)
				{
					ValueSet = true;
					Value = (MemberInfo as FieldInfo).GetValue(Struct);
				}
				else if (MemberInfo is PropertyInfo)
				{
					ValueSet = true;
					Value = (MemberInfo as PropertyInfo).GetValue(Struct, null);
				}

				if (ValueSet)
				{
					if (MemberCount > 0)
					{
						Ret += ",";
					}
					Ret += MemberInfo.Name;
					Ret += "=";
					if (Value.GetType() == typeof(uint))
					{
						Ret += String.Format("0x{0:X}", Value);
					}
					else
					{
						Ret += Value;
					}
					MemberCount++;
				}
			}
			Ret += ")";
			return Ret;
		}
	}
}
