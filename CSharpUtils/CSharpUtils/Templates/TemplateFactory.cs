using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpUtils.Templates.TemplateProvider;
using CSharpUtils.Templates.Runtime;
using CSharpUtils.Templates.Tokenizers;

namespace CSharpUtils.Templates
{
	public class TemplateFactory
	{
		ITemplateProvider TemplateProvider;

		public TemplateFactory(ITemplateProvider TemplateProvider = null)
		{
			this.TemplateProvider = TemplateProvider;
		}

		public TemplateCode GetTemplateByString(String TemplateString)
		{
			return new TemplateCodeGen(TemplateString, TemplateProvider).GetTemplate();
		}

		public TemplateCode GetTemplateByFile(String Name, Encoding Encoding = null)
		{
			if (TemplateProvider == null) throw(new Exception("No specified TemplateProvider"));
			return GetTemplateByString(TemplateProvider.GetTemplate(Name).ReadAllContentsAsString(Encoding));
		}
	}
}
