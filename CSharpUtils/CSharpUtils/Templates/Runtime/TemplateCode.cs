using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Dynamic;
using System.Reflection;
using CSharpUtils.Templates.TemplateProvider;

namespace CSharpUtils.Templates.Runtime
{
	public class TemplateCode
	{
		TemplateFactory TemplateFactory;
		public delegate void RenderDelegate(TemplateContext Context);
		Dictionary<String, RenderDelegate> Blocks = new Dictionary<string, RenderDelegate>();
		TemplateCode ChildTemplate;
		TemplateCode ParentTemplate;

		public TemplateCode(TemplateFactory TemplateFactory = null)
		{
			this.TemplateFactory = TemplateFactory;
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
			catch (FinalizeRenderException)
			{
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
			Render(new TemplateContext(StringWriter, Parameters, TemplateFactory));
			return StringWriter.ToString();
		}

		protected void SetAndRenderParentTemplate(String ParentTemplateFileName, TemplateContext Context)
		{
			this.ParentTemplate = Context.TemplateFactory.GetTemplateByFile(ParentTemplateFileName);
			this.SetBlocks(this.ParentTemplate.Blocks);
			this.ParentTemplate.ChildTemplate = this;
			this.ParentTemplate.LocalRender(Context);
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
