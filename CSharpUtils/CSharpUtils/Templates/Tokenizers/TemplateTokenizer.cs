using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CSharpUtils.Templates.Tokenizers
{
	public class TemplateTokenizer
	{
		static Regex StartTagRegex = new Regex(@"\{[\{%]", RegexOptions.Compiled);

		static public List<TemplateToken> Tokenize(TokenizerStringReader StringReader)
		{
			List<TemplateToken> Tokens = new List<TemplateToken>();
			Tokenize(Tokens, StringReader);
			return Tokens;
		}

		static public List<TemplateToken> Tokenize(List<TemplateToken> Tokens, TokenizerStringReader StringReader)
		{
			while (true)
			{
				var Match = StringReader.Match(StartTagRegex);

				if (Match.Success)
				{
					var RawText = StringReader.ReadString(Match.Index);
					if (RawText.Length > 0) Tokens.Add(new RawTemplateToken() { Text = RawText });
					var OpenTagTokenString = StringReader.ReadString(2);
					Tokens.Add(new OpenTagTemplateToken() { Text = OpenTagTokenString });
					{
						ExpressionTokenizer.Tokenize(Tokens, StringReader);
					}
					var CloseTagTokenString = StringReader.ReadString(2);
					var ExpectedCloseTagTokenString = (OpenTagTokenString == "{{") ? "}}" : "%}";
					if (CloseTagTokenString != ExpectedCloseTagTokenString)
					{
						throw (new Exception("Expected '" + ExpectedCloseTagTokenString + "' but got '" + CloseTagTokenString + "'"));
					}
					Tokens.Add(new CloseTagTemplateToken() { Text = CloseTagTokenString });
					continue;
				}
				else
				{
					var RawText = StringReader.ReadString();
					if (RawText.Length > 0) Tokens.Add(new RawTemplateToken() { Text = RawText });
					return Tokens;
				}
			}
		}
	}
}
