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
			Context.RenderingTemplate = this;

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

		public String RenderToString(TemplateScope Scope = null)
		{
			if (Scope == null) Scope = new TemplateScope();
			var StringWriter = new StringWriter();
			Render(new TemplateContext(StringWriter, Scope, TemplateFactory));
			return StringWriter.ToString();
		}

		protected void SetAndRenderParentTemplate(String ParentTemplateFileName, TemplateContext Context)
		{
			this.ParentTemplate = Context.TemplateFactory.GetTemplateCodeByFile(ParentTemplateFileName);
			this.ParentTemplate.ChildTemplate = this;
			this.ParentTemplate.LocalRender(Context);

			throw (new FinalizeRenderException());
		}

		protected void CallBlock(String BlockName, TemplateContext Context)
		{
			Context.RenderingTemplate.GetFirstAscendingBlock(BlockName)(Context);
		}

		protected RenderDelegate GetFirstAscendingBlock(String BlockName)
		{
			if (this.Blocks.ContainsKey(BlockName))
			{
				return this.Blocks[BlockName];
			}

			if (this.ParentTemplate != null)
			{
				return this.ParentTemplate.GetFirstAscendingBlock(BlockName);
			}
			
			throw(new Exception(String.Format("Can't find ascending parent block '{0}'", BlockName)));
		}

		protected void CallParentBlock(String BlockName, TemplateContext Context)
		{
			this.ParentTemplate.GetFirstAscendingBlock(BlockName)(Context);
		}

		public delegate void EmptyDelegate();

		protected void Autoescape(TemplateContext Context, dynamic Expression, EmptyDelegate Block)
		{
			bool OldAutoescape = Context.Autoescape;
			Context.Autoescape = Expression;
			{
				Block();
			}
			Context.Autoescape = OldAutoescape;
		}

		protected void Foreach(TemplateContext Context, String VarName, dynamic Expression, EmptyDelegate Iteration, EmptyDelegate Else = null)
		{
			int Index = 0;
			foreach (var Item in DynamicUtils.ConvertToIEnumerable(Expression))
			{
				Context.SetVar("loop", new Dictionary<String, dynamic> {
					{ "index", Index + 1 },
					{ "index0", Index },
				});
				Context.SetVar(VarName, Item);
				Iteration();
				Index++;
			}

			if (Index == 0)
			{
				if (Else != null) Else();
			}
		}
	}
}
