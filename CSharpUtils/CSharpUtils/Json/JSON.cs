using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Globalization;

namespace CSharpUtils.Json
{
	public class JSON
	{
		static public object Decode(string Format)
		{
			return Parse(Format);
		}

		static public string Encode(object ObjectToEncode)
		{
			return Stringify(ObjectToEncode);
		}

		static public object Parse(string Format)
		{
			throw (new NotImplementedException());
		}

		static public string Stringify(object ObjectToEncode) {
			if (ObjectToEncode == null)
			{
				return "null";
			}

			if (ObjectToEncode is string)
			{
				return '"' + Escape(ObjectToEncode as string) + '"';
			}

			if (ObjectToEncode is bool)
			{
				return (((dynamic)ObjectToEncode) == true) ? "true" : "false";
			}

			if (ObjectToEncode is IDictionary) {
				var Dict = ObjectToEncode as IDictionary;
				var Str = "";
				foreach (var Key in Dict.Keys)
				{
					var Value = Dict[Key];
					if (Str.Length > 0) Str += ",";
					Str += Stringify(Key.ToString()) + ":" + Stringify(Value);

				}
				return "{" + Str + "}";
			}

			if (ObjectToEncode is IEnumerable)
			{
				var List = ObjectToEncode as IEnumerable;
				var Str = "";
				foreach (var Item in List)
				{
					if (Str.Length > 0) Str += ",";
					Str += Stringify(Item);
				}
				return "[" + Str + "]";
			}

			double NumericResult;
			string NumericStr = Convert.ToString(ObjectToEncode, CultureInfo.InvariantCulture.NumberFormat);
			if (Double.TryParse(NumericStr, out NumericResult))
			{
				return NumericStr;
			}
			else
			{
				//throw (new NotImplementedException("Don't know how to encode '" + ObjectToEncode + "'."));
				return Stringify(ObjectToEncode.ToString());
			}
		}

		static protected string Escape(string StringToEscape)
		{
			var Ret = "";
			foreach (var C in StringToEscape)
			{
				switch (C)
				{
					case '/':
					case '\"':
					case '\'':
					case '\b':
					case '\f':
					case '\n':
					case '\r':
					case '\t':
						Ret += '\\' + C;
						break;
					default:
						if (C > 255)
						{
							Ret += "\\u" + Convert.ToString(C, 16).PadLeft(4, '0');
						} else {
							Ret += C;
						}
						break;
				}
			}
			return Ret;
		}
	}
}
