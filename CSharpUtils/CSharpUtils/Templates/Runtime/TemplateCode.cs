using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Dynamic;
using System.Reflection;

namespace CSharpUtils.Templates.Runtime
{
	public class TemplateCode
	{
		public delegate void RenderDelegate(TemplateContext Context);
		Dictionary<String, RenderDelegate> Blocks = new Dictionary<string, RenderDelegate>();
		String ParentClassName;
		TemplateCode ParentTemplate;

		public TemplateCode()
		{
			this.Init();
		}

		virtual public void Init()
		{
			this.SetBlocks(this.Blocks);
		}

		virtual public void SetBlocks(Dictionary<String, RenderDelegate> Blocks)
		{
		}

		protected void SetBlock(Dictionary<String, RenderDelegate> Blocks, String BlockName, RenderDelegate Callback)
		{
			Blocks[BlockName] = Callback;
		}

		virtual protected void LocalRender(TemplateContext Context)
		{
		}

		public void Render(TemplateContext Context)
		{
			try
			{
				this.LocalRender(Context);
			}
			catch (Exception Exception)
			{
				Context.Output.WriteLine(Exception);
			}
		}

		public String RenderToString(dynamic Parameters = null)
		{
			if (Parameters == null) Parameters = new Dictionary<String, object>();
			var StringWriter = new StringWriter();
			Render(new TemplateContext(StringWriter, Parameters));
			return StringWriter.ToString();
		}

		protected void SetParentClass(String ClassName)
		{
			this.ParentClassName = ClassName;
			this.ParentTemplate = (TemplateCode)Activator.CreateInstance(Assembly.GetExecutingAssembly().GetType(ClassName));
			this.ParentTemplate.SetBlocks(this.Blocks);
		}

		protected void CallBlock(String BlockName, TemplateContext Context)
		{
			this.Blocks[BlockName](Context);
		}

		protected void CallParentBlock(String BlockName, TemplateContext Context)
		{
		}
	}
}
