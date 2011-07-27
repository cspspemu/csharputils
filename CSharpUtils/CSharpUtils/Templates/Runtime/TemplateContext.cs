using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CSharpUtils.Templates.TemplateProvider;
using CSharpUtils.Html;
using CSharpUtils.Templates.Runtime.Filters;
using System.Reflection;

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
		public bool Autoescape = true;
		public Dictionary<String, Tuple<Type, string>> Filters;

		public TemplateContext(TextWriter Output, TemplateScope Scope = null, TemplateFactory TemplateFactory = null)
		{
			if (Scope == null) Scope = new TemplateScope();

			this.Output = Output;
			this.Scope = Scope;
			this.TemplateFactory = TemplateFactory;

			Filters = new Dictionary<string, Tuple<Type, string>>();

			AddFilterLibrary(typeof(CoreFilters));
		}

		public void AddFilterLibrary(Type FilterLibraryType)
		{
			foreach (var Method in FilterLibraryType.GetMethods(BindingFlags.Static | BindingFlags.Public))
			{
				Console.WriteLine(Method);
				foreach (var Attribute in Method.GetCustomAttributes(typeof(TemplateFilterAttribute), true))
				{
					TemplateFilterAttribute TemplateFilterAttribute = (TemplateFilterAttribute)Attribute;
					this.AddFilter(TemplateFilterAttribute.Name, FilterLibraryType, Method.Name);
				}
			}
			/*
			this.AddFilter("format", typeof(CoreFilters), "Format");
			this.AddFilter("raw", typeof(CoreFilters), "Raw");
			this.AddFilter("Escape", typeof(CoreFilters), "Escape");
			*/
		}

		public void AddFilter(String FilterName, Type Type, String FunctionName)
		{
			Filters[FilterName] = new Tuple<Type,string>(Type, FunctionName);
		}

		public dynamic CallFilter(string FilterName, params dynamic[] Params)
		{
			Tuple<Type, string> Info;
			if (Filters.TryGetValue(FilterName, out Info))
			{
				return DynamicUtils.Call(Info.Item1, Info.Item2, Params);
			}
			else
			{
				return null;
			}
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
			if (Value is RawWrapper || !Autoescape)
			{
				return Value;
			}
			return HtmlUtils.EscapeHtmlCharacters(DynamicUtils.ConvertToString(Value));
		}
	}
}
