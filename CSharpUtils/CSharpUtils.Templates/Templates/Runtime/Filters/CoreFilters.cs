using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpUtils.Html;

namespace CSharpUtils.Templates.Runtime.Filters
{
	public class CoreFilters
	{
		[TemplateFilter(Name="format")]
		static public string Format(String Format, params Object[] Params)
		{
			return String.Format(Format, Params);
		}

		[TemplateFilter(Name = "raw")]
		static public RawWrapper Raw(dynamic Object)
		{
			return RawWrapper.Get(Object);
		}

		[TemplateFilter(Name = "escape")]
		static public RawWrapper Escape(dynamic Object)
		{
			return RawWrapper.Get(HtmlUtils.EscapeHtmlCharacters(Object));
		}

		[TemplateFilter(Name = "e")]
		static public RawWrapper E(dynamic Object)
		{
			return Escape(Object);
		}
	}
}
