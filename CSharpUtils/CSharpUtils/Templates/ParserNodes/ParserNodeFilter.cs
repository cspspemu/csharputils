using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpUtils.Templates.Utils;

namespace CSharpUtils.Templates.ParserNodes
{
	public class ParserNodeFilter : ParserNode
	{
		protected ParserNode[] Params;
		protected String FilterName;

		public ParserNodeFilter(String FilterName, params ParserNode[] Params)
		{
			this.FilterName = FilterName;
			this.Params = Params;
		}

		public override void WriteTo(ParserNodeContext Context)
		{
			Context.Write("Context.CallFilter({0}", StringUtils.EscapeString(FilterName));
			foreach (var Param in Params)
			{
				Context.Write(", ");
				Param.WriteTo(Context);
			}
			Context.Write(")");
		}

		public override string ToString()
		{
			return base.ToString() + "(" + FilterName + ")";
		}
	}

	public class ParserNodeAutoescape : ParserNode
	{
		protected ParserNode AutoescapeExpression;
		protected ParserNode Body;

		public ParserNodeAutoescape(ParserNode AutoescapeExpression, ParserNode Body)
		{
			this.AutoescapeExpression = AutoescapeExpression;
			this.Body = Body;
		}

		public override void WriteTo(ParserNodeContext Context)
		{
			Context.Write("Autoescape(Context, ");
			AutoescapeExpression.WriteTo(Context);
			Context.WriteLine(", new EmptyDelegate(delegate() {");
			Body.WriteTo(Context);
			Context.WriteLine("}));");
		}
	}
}
