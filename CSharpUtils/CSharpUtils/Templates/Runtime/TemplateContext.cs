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

		public dynamic GetVar(String Name)
		{
			return Parameters[Name];
		}

		public void SetVar(String Name, dynamic Value)
		{
			Parameters[Name] = Value;
		}

		public dynamic AutoFilter(dynamic Value)
		{
			return Value;
		}

		public bool ToBool(dynamic Value)
		{
			try
			{
				if (Value == null) return false;
				if (Value is bool) return Value;
				if (Value is int) return Value != 0;
				if (Value is Int64) return Value != 0;
				if (Value is object) return true;
				return (bool)Value;
			}
			catch (Exception Exception)
			{
				Console.WriteLine("VALUE: {0}", Value);
				Console.Error.WriteLine(Exception);
				return false;
			}
		}

		public dynamic GenRange(dynamic Start, dynamic End)
		{
			var List = new List<dynamic>();
			for (dynamic N = Start; N <= End; N++)
			{
				List.Add(N);
			}
			//return List.ToArray();
			return List;
		}
	}
}
