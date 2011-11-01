using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CSharpUtils.Http
{
	public class HttpHeader
	{
		readonly public String NormalizedName;
		readonly public String Name;
		readonly public String Value;

		public HttpHeader(String Name, String Value)
		{
			this.Name = Name.Trim();
			this.Value = Value.Trim();
			this.NormalizedName = GetNormalizedName(Name);
		}

		static public String GetNormalizedName(String Name)
		{
			return Name.Trim().ToLower();
		}

		public Dictionary<String, String> ParseValue(String FirstKeyName = "Type")
		{
			var Values = new Dictionary<String, String>();
			var TwoParts=  Value.Split(new char[] { ';' }, 2);
			Values[FirstKeyName] = TwoParts[0];
			//Console.WriteLine(TwoParts[0]);
			if (TwoParts.Length == 2)
			{
				var ParseLeft = new Regex("\\s*(\\w+)=(\"|)([^\"]*)\\2(;|$)", RegexOptions.Compiled);
				foreach (Match Match in ParseLeft.Matches(TwoParts[1]))
				{
					string MatchKey = Match.Groups[1].Value;
					string MatchValue = Match.Groups[3].Value;
					Values[MatchKey.ToLower()] = MatchValue;
					//Console.WriteLine(Match.Groups[1]);
				}
			}

			return Values;
		}
	}
}
