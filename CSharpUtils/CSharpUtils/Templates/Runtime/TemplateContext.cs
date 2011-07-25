using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CSharpUtils.Templates.TemplateProvider;
using CSharpUtils.Html;

namespace CSharpUtils.Templates.Runtime
{
	sealed public class TemplateScope
	{
		public TemplateScope ParentScope;
		public Dictionary<String, dynamic> Items;

		public TemplateScope(Dictionary<String, dynamic> Items, TemplateScope ParentScope = null)
		{
			this.ParentScope = ParentScope;
			this.Items = Items;
		}

		public TemplateScope(TemplateScope ParentScope = null)
		{
			this.ParentScope = ParentScope;
			this.Items = new Dictionary<String, dynamic>();
		}

		public dynamic this[String Index]
		{
			set
			{
				Items[Index] = value;
			}
			get
			{
				if (Items.ContainsKey(Index))
				{
					return Items[Index];
				}

				if (ParentScope != null)
				{
					return ParentScope[Index];
				}
				return null;
			}
		}
	}

	sealed public class TemplateContext
	{
		public TemplateCode RenderingTemplate;
		public TemplateFactory TemplateFactory;
		public TextWriter Output;
		public TemplateScope Scope;
		public dynamic Parameters;

		public TemplateContext(TextWriter Output, TemplateScope Scope = null, TemplateFactory TemplateFactory = null)
		{
			if (Scope == null) Scope = new TemplateScope();

			this.Output = Output;
			this.Scope = Scope;
			this.TemplateFactory = TemplateFactory;
		}

		public void OutputWriteAutoFiltered(dynamic Value)
		{
			if (Value != null)
			{
				Output.Write(AutoFilter(Value));
			}
		}

		public void NewScope(Action Action)
		{
			this.Scope = new TemplateScope(this.Scope);
			Action();
			this.Scope = this.Scope.ParentScope;
		}

		public dynamic GetVar(String Name)
		{
			try
			{
				return Scope[Name];
			}
			catch (Exception)
			{
				return null;
			}
		}

		public void SetVar(String Name, dynamic Value)
		{
			Scope[Name] = Value;
		}

		public dynamic AutoFilter(dynamic Value)
		{
			Value = HtmlUtils.EscapeHtmlCharacters(DynamicUtils.ConvertToString(Value));
			return Value;
		}
	}
}
