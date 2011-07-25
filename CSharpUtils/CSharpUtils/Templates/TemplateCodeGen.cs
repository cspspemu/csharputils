using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpUtils.Templates.Tokenizers;
using CSharpUtils.Templates.Runtime;
using System.IO;
using CSharpUtils.Templates.ParserNodes;
using CSharpUtils.Templates.Utils;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;

namespace CSharpUtils.Templates
{
	public class TemplateCodeGen
	{
		TemplateFactory TemplateFactory;
		TokenReader Tokens;

		static public TemplateCode CompileTemplateCodeByString(String TemplateString, TemplateFactory TemplateFactory = null)
		{
			return (TemplateCode)Activator.CreateInstance(CompileTemplateCodeTypeByString(TemplateString, TemplateFactory), TemplateFactory);
		}

		static public Type CompileTemplateCodeTypeByString(String TemplateString, TemplateFactory TemplateFactory = null)
		{
			return (new TemplateCodeGen(TemplateString, TemplateFactory)).GetTemplateCodeType();
		}

		public TemplateCodeGen(String TemplateString, TemplateFactory TemplateFactory = null)
		{
			this.TemplateFactory = TemplateFactory;
			this.Tokens = new TokenReader(TemplateTokenizer.Tokenize(new TokenizerStringReader(TemplateString)));
		}

		protected String GetCode()
		{
			StringWriter CodeWriter = new StringWriter();
			RenderCodeTo(CodeWriter);

			//Console.WriteLine(CodeWriter.ToString());

			return CodeWriter.ToString();
		}

		protected void RenderCodeTo(TextWriter TextWriter)
		{
			var TemplateHandler = new TemplateHandler(Tokens, TextWriter);
			var Context = new ParserNodeContext(TextWriter, TemplateFactory);
			TemplateHandler.Reset();
			var ParserNode = TemplateHandler.HandleLevel_Root();

			//OptimizedParserNode.Dump();
			Context.WriteLine("using System;");
			Context.WriteLine("using System.Collections.Generic;");
			Context.WriteLine("using CSharpUtils.Templates;");
			Context.WriteLine("using CSharpUtils.Templates.Runtime;");
			Context.WriteLine("using CSharpUtils.Templates.TemplateProvider;");
			Context.WriteLine("");
			Context.WriteLine("namespace CSharpUtils.Templates.CompiledTemplates {");
			Context.Indent(delegate()
			{
				Context.WriteLine("class CompiledTemplate_TempTemplate : TemplateCode {");

				Context.Indent(delegate()
				{
					Context.WriteLine("public CompiledTemplate_TempTemplate(TemplateFactory TemplateFactory = null) : base(TemplateFactory) { }");
					Context.WriteLine("");

					Context.WriteLine("override public void SetBlocks(Dictionary<String, RenderDelegate> Blocks) {");
					Context.Indent(delegate()
					{
						foreach (var BlockPair in TemplateHandler.Blocks)
						{
							Context.WriteLine(String.Format("SetBlock(Blocks, {0}, Block_{1});", StringUtils.EscapeString(BlockPair.Key), BlockPair.Key));
						}
					});
					Context.WriteLine("}");
					Context.WriteLine("");

					Context.WriteLine("override protected void LocalRender(TemplateContext Context) {");
					Context.Indent(delegate()
					{
						ParserNode.OptimizeAndWrite(Context);
					});
					Context.WriteLine("}"); // Method

					foreach (var BlockPair in TemplateHandler.Blocks)
					{
						Context.WriteLine("");
						Context.WriteLine("public void Block_" + BlockPair.Key + "(TemplateContext Context) {");
						Context.Indent(delegate()
						{
							BlockPair.Value.OptimizeAndWrite(Context);
						});
						Context.WriteLine("}"); // Method
					}
				});

				Context.WriteLine("}"); // class
			});

			Context.WriteLine("}"); // namespace
		}

		protected Type GetTemplateCodeTypeByCode(String Code)
		{
			//Console.WriteLine(Code);

			CSharpCodeProvider CSharpCodeProvider = new CSharpCodeProvider();
			//Console.WriteLine(Assembly.GetExecutingAssembly().FullName);

			CompilerResults CompilerResults = CSharpCodeProvider.CompileAssemblyFromSource(
				new CompilerParameters(new string[] {
					"System.dll",
					"Microsoft.CSharp.dll",
					"System.Core.dll",
					System.Reflection.Assembly.GetAssembly(typeof(TemplateCodeGen)).Location
				}),
				Code
			);

			if (CompilerResults.NativeCompilerReturnValue == 0)
			{
				Assembly assembly = CompilerResults.CompiledAssembly;
				Type Type = assembly.GetType("CSharpUtils.Templates.CompiledTemplates.CompiledTemplate_TempTemplate");
				return Type;
			}
			else
			{
				Console.Error.WriteLine(Code);

				foreach (var Error in CompilerResults.Errors)
				{
					Console.Error.WriteLine("Error: " + Error);
				}

				throw (new Exception("Error Compiling"));
			}
		}

		public Type GetTemplateCodeType()
		{
			return GetTemplateCodeTypeByCode(GetCode());
		}

		/*
		public String RenderToString(dynamic Parameters = null)
		{
			var OutputWriter = new StringWriter();
			GetTemplateCodeTypeByCode(GetCode()).Render(new TemplateContext(OutputWriter, Parameters, TemplateFactory));
			return OutputWriter.ToString();
		}
		*/
	}

}
