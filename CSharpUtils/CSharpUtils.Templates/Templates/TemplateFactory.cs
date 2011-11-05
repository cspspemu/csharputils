using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpUtils.Templates.TemplateProvider;
using CSharpUtils.Templates.Runtime;
using CSharpUtils.Templates.Tokenizers;
using CSharpUtils.Extensions;
using CSharpUtils.Templates.Templates;

namespace CSharpUtils.Templates
{
	public class TemplateFactory
	{
		public Encoding Encoding;
		public ITemplateProvider TemplateProvider;
		public Dictionary<String, Type> CachedTemplatesByFile = new Dictionary<string, Type>();

		public TemplateFactory(ITemplateProvider TemplateProvider = null, Encoding Encoding = null)
		{
			this.Encoding = Encoding;
			this.TemplateProvider = TemplateProvider;
		}

		protected Type GetTemplateCodeTypeByString(String TemplateString)
		{
			//return new TemplateCodeGen(TemplateString, this).GetTemplateCodeType();
			return new TemplateCodeGenRoslyn(TemplateString, this).GetTemplateCodeType();
		}

		protected Type GetTemplateCodeTypeByFile(String Name)
		{
			if (TemplateProvider == null) throw(new Exception("No specified TemplateProvider"));
			lock (CachedTemplatesByFile)
			{
				if (!CachedTemplatesByFile.ContainsKey(Name))
				{
					using (var TemplateStream = TemplateProvider.GetTemplate(Name))
					{
						return CachedTemplatesByFile[Name] = GetTemplateCodeTypeByString(TemplateStream.ReadAllContentsAsString(Encoding));
					}
				}

				return CachedTemplatesByFile[Name];
			}
		}

		protected TemplateCode CreateInstanceByType(Type Type)
		{
			return (TemplateCode)Activator.CreateInstance(Type, this);
		}

		public TemplateCode GetTemplateCodeByString(String TemplateString)
		{
			return CreateInstanceByType(GetTemplateCodeTypeByString(TemplateString));
		}

		public TemplateCode GetTemplateCodeByFile(String Name)
		{
			return CreateInstanceByType(GetTemplateCodeTypeByFile(Name));
		}
	}
}
