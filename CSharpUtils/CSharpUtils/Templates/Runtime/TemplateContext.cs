using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CSharpUtils.Templates.TemplateProvider;

namespace CSharpUtils.Templates.Runtime
{
	public class TemplateContext
	{
		public TemplateCode RenderingTemplate;
		public TemplateFactory TemplateFactory;
		public TextWriter Output;
		public dynamic Parameters;

		public TemplateContext(TextWriter Output, dynamic Parameters, TemplateFactory TemplateFactory)
		{
			if (Parameters == null) Parameters = new Dictionary<String, object>();

			this.Output = Output;
			this.Parameters = Parameters;
			this.TemplateFactory = TemplateFactory;
		}

		public void OutputWriteAutoFiltered(dynamic Value)
		{
			if (Value != null)
			{
				Output.Write(AutoFilter(Value));
			}
		}

		public dynamic GetVar(String Name)
		{
			try
			{
				return Parameters[Name];
			}
			catch (Exception)
			{
				return null;
			}
		}

		public void SetVar(String Name, dynamic Value)
		{
			Parameters[Name] = Value;
		}

		public dynamic AutoFilter(dynamic Value)
		{
			return Value;
		}
	}
}
