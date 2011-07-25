using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpUtils.Html
{
	public class HtmlUtils
	{
		static public String EscapeHtmlCharacters(String Input)
		{
			String Output = "";
			for (int n = 0; n < Input.Length; n++)
			{
				switch (Input[n])
				{
					case '<' : Output += "&lt;"; break;
					case '>' : Output += "&gt;"; break;
					case '&' : Output += "&amp;"; break;
					case '"' : Output += "&quot;"; break;
					case '\'': Output += "&#039;"; break;
					default: Output += Input[n]; break;
				}
			}
			return Output;
		}
	}
}
