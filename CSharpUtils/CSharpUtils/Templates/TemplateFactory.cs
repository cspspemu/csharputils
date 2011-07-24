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
		public Encoding Encoding;
		public ITemplateProvider TemplateProvider;
		public Dictionary<String, TemplateCode> CachedTemplatesByFile = new Dictionary<string, TemplateCode>();

		public TemplateFactory(ITemplateProvider TemplateProvider = null, Encoding Encoding = null)
		{
			this.Encoding = Encoding;
			this.TemplateProvider = TemplateProvider;
		}

		public TemplateCode GetTemplateByString(String TemplateString)
		{
			return new TemplateCodeGen(TemplateString, this).GetTemplate();
		}

		public TemplateCode GetTemplateByFile(String Name)
		{
			if (TemplateProvider == null) throw(new Exception("No specified TemplateProvider"));
			if (!CachedTemplatesByFile.ContainsKey(Name))
			{
				return CachedTemplatesByFile[Name] = GetTemplateByString(TemplateProvider.GetTemplate(Name).ReadAllContentsAsString(Encoding));
			}

			return CachedTemplatesByFile[Name];
		}
	}
}
