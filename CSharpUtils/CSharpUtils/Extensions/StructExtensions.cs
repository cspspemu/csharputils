using System;
using System.Reflection;

public static class StructExtensions
{
	public static string ToStringDefault<T>(this T Struct, bool SimplifyBool = false) //where T : struct
	{
		var Ret = "";
		Ret += typeof(T).Name;
		Ret += "(";
		var MemberCount = 0;
		bool AddedItem = false;

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
				if (AddedItem)
				{
					Ret += ",";
					AddedItem = false;
				}

				if (SimplifyBool && (Value is bool))
				{
					if (((bool)Value) == true)
					{
						Ret += MemberInfo.Name;
						MemberCount++;
						AddedItem = true;
					}
				}
				else
				{
					Ret += MemberInfo.Name;
					Ret += "=";
					if (Value is uint)
					{
						Ret += String.Format("0x{0:X}", Value);
					}
					else
					{
						Ret += Value;
					}
					MemberCount++;
					AddedItem = true;
				}
			}
		}
		Ret += ")";
		return Ret;
	}
}
