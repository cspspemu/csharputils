using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using CSharpUtils.Templates.Tokenizers;
using Microsoft.CSharp;
using System.Reflection;
using System.CodeDom.Compiler;
using CSharpUtils.Templates.Runtime;
using CSharpUtils.Templates.ParserNodes;

namespace CSharpUtils.Templates
{
    public class Finalize_HandlingLevel_Root : Exception
    {
    }

    public class TemplateHandler
    {
        TokenReader Tokens;
        TextWriter TextWriter;
        dynamic Parameters;

        public TemplateHandler(TokenReader Tokens, TextWriter TextWriter, dynamic Parameters)
        {
            this.Tokens = Tokens;
            this.TextWriter = TextWriter;
            this.Parameters = Parameters;
        }

        TemplateToken CurrentToken
        {
            get
            {
                return Tokens.Current;
            }
        }

        string CurrentTokenType
        {
            get
            {
                return CurrentToken.GetType().Name;
            }
        }

        public ParserNode HandleLevel_Identifier()
        {
            ParserNode ParserNode;

            switch (CurrentTokenType)
            {
                case "OperatorTemplateToken":
                    String Operator = CurrentToken.Text;
                    switch (Operator)
                    {
                        // Unary Operators
                        case "+": case "-":
                            Tokens.MoveNext();
                            ParserNode = new ParserNodeUnaryOperation()
                            {
                                Parent = HandleLevel_Expression(),
                                Operator = Operator,
                            };
                            Tokens.MoveNext();
                            
                            break;
                        case "(":
                            Tokens.MoveNext();
                            ParserNode = HandleLevel_Expression();
                            Tokens.ExpectValueAndNext(")");
                            break;
                        default:
                            throw (new Exception(String.Format("Invalid operator '{0}'", CurrentTokenType)));
                    }
                    break;
                case "NumericLiteralTemplateToken":
                    ParserNode = new ParserNodeNumericLiteral()
                    {
                        Value = Int64.Parse(CurrentToken.Text),
                    };
                    Tokens.MoveNext();
                    break;
                case "IdentifierTemplateToken":
                    ParserNode = new ParserNodeIdentifier()
                    {
                        Id = CurrentToken.Text
                    };
                    Tokens.MoveNext();
                    break;
                case "StringLiteralTemplateToken":
                    ParserNode = new ParserNodeStringLiteral()
                    {
                        Value = CurrentToken.Text,
                    };
                    Tokens.MoveNext();
                    break;
                default:
                    throw (new Exception(String.Format("Invalid Identifier '{0}'('{1}')", CurrentTokenType, CurrentToken.Text)));
            }

            return ParserNode;
        }

        public ParserNode HandleLevel_Mul()
        {
            ParserNode ParserNode = HandleLevel_Identifier();

            while (Tokens.HasMore)
            {
                switch (CurrentTokenType)
                {
                    case "OperatorTemplateToken":
                        string Operator = CurrentToken.Text;
                        switch (Operator)
                        {
                            case "*":
                            case "/":
                                Tokens.MoveNext();
                                ParserNode = new ParserNodeBinaryOperation()
                                {
                                    LeftNode = ParserNode,
                                    Operator = Operator,
                                    RightNode = HandleLevel_Identifier(),
                                };
                                break;
                            default: return ParserNode;
                        }
                        break;
                    default: return ParserNode;
                }
            }

            return ParserNode;
        }

        public ParserNode HandleLevel_Sum()
        {
            ParserNode ParserNode = HandleLevel_Mul();

            while (Tokens.HasMore)
            {
                switch (CurrentTokenType)
                {
                    case "OperatorTemplateToken":
                        string Operator = CurrentToken.Text;
                        switch (Operator)
                        {
                            case "+":
                            case "-":
                                Tokens.MoveNext();
                                ParserNode = new ParserNodeBinaryOperation()
                                {
                                    LeftNode = ParserNode,
                                    Operator = Operator,
                                    RightNode = HandleLevel_Mul(),
                                };
                                break;
                            default: return ParserNode;
                        }
                        break;
                    default: return ParserNode;
                }
            }

            return ParserNode;
        }

        public ParserNode HandleLevel_Sli()
        {
            ParserNode ParserNode = HandleLevel_Sum();

            while (Tokens.HasMore)
            {
                switch (CurrentTokenType)
                {
                    case "OperatorTemplateToken":
                        string Operator = CurrentToken.Text;
                        switch (Operator)
                        {
                            case "..":
                                Tokens.MoveNext();
                                ParserNode = new ParserNodeBinaryOperation()
                                {
                                    LeftNode = ParserNode,
                                    Operator = Operator,
                                    RightNode = HandleLevel_Sum(),
                                };
                                break;
                            default: return ParserNode;
                        }
                        break;
                    default: return ParserNode;
                }
            }

            return ParserNode;
        }

        public ParserNode HandleLevel_Expression()
        {
            return HandleLevel_Sli();
        }

        public ParserNode HandleLevel_Tag()
        {
            return HandleLevel_Expression();
        }

        public ParserNode HandleLevel_TagSpecial_For()
        {
            Tokens.MoveNext();

            String VarName = CurrentToken.Text;
            Tokens.MoveNext();
            Tokens.ExpectValueAndNext("in");
            ParserNode LoopIterator = HandleLevel_Expression();
            Tokens.ExpectValueAndNext("%}");

            ParserNode BodyBlock = HandleLevel_Root();

            Tokens.ExpectValueAndNext("endfor");
            Tokens.ExpectValueAndNext("%}");

            return new ForParserNode()
            {
                LoopIterator = LoopIterator,
                VarName = VarName,
                BodyBlock = BodyBlock,
            };
        }

        public ParserNode HandleLevel_TagSpecial_Extends()
        {

            Tokens.MoveNext();
            ParserNodeExtends ParserNodeExtends = new ParserNodeExtends() { Parent = HandleLevel_Expression() };
            Tokens.ExpectValueAndNext("%}");
            return ParserNodeExtends;
        }

        public ParserNode HandleLevel_TagSpecial()
        {
            string SpecialType = CurrentToken.Text;
            switch (SpecialType)
            {
                case "if":
                    bool Alive = true;

                    Tokens.MoveNext();

                    ParserNode ConditionNode = HandleLevel_Expression();
                    Tokens.ExpectValueAndNext("%}");

                    ParserNode BodyIfNode = HandleLevel_Root();
                    ParserNode BodyElseNode = new DummyParserNode();

                    while (Alive)
                    {
                        switch (CurrentToken.Text)
                        {
                            case "endif":
                                Tokens.MoveNext();
                                Tokens.ExpectValueAndNext("%}");
                                Alive = false;
                                break;
                            case "else":
                                Tokens.MoveNext();
                                Tokens.ExpectValueAndNext("%}");

                                BodyElseNode = HandleLevel_Root();

                                break;
                            default:
                                throw (new Exception(String.Format("Unprocessed Token Type '{0}'", CurrentTokenType)));
                        }
                    }

                    return new IfParserNode()
                    {
                        ConditionNode = ConditionNode,
                        BodyIfNode = BodyIfNode,
                        BodyElseNode = BodyElseNode,
                    };
                case "block": {
                    Tokens.MoveNext();

                    String BlockName = CurrentToken.Text;
                    Tokens.MoveNext();
                    Tokens.ExpectValueAndNext("%}");

                    ParserNode BodyBlock = HandleLevel_Root();

                    Tokens.ExpectValueAndNext("endblock");
                    Tokens.ExpectValueAndNext("%}");

                    return new ParserNodeBlock()
                    {
                        Parent = BodyBlock,
                        BlockName = BlockName,
                    };
                }
                case "for": return HandleLevel_TagSpecial_For();
                case "extends": return HandleLevel_TagSpecial_Extends();
                case "else":
                case "endif":
                case "endblock":
                case "endfor":
                    throw (new Finalize_HandlingLevel_Root());
                default:
                    throw (new Exception(String.Format("Unprocessed Tag Type '{0}'('{1}')", CurrentTokenType, CurrentToken.Text)));
            }
            //return HandleLevel_Expression();
        }

        public ParserNode HandleLevel_Root()
        {
            var ParserNodeContainer = new ParserNodeContainer();

            try
            {
                while (Tokens.HasMore)
                {
                    //Console.WriteLine(CurrentToken);
                    switch (CurrentTokenType)
                    {
                        case "RawTemplateToken":
                            ParserNodeContainer.Add(new ParserNodeLiteral()
                            {
                                Text = ((RawTemplateToken)CurrentToken).Text,
                            });
                            Tokens.MoveNext();
                            break;
                        case "OpenTagTemplateToken":
                            string OpenType = CurrentToken.Text;

                            Tokens.MoveNext();

                            switch (OpenType)
                            {
                                case "{{":
                                    ParserNodeContainer.Add(new ParserNodeOutputExpression() { Parent = HandleLevel_Tag() });
                                    Tokens.ExpectValueAndNext("}}");
                                    break;
                                case "{%": {
                                    ParserNode ParserNode = HandleLevel_TagSpecial();
                                    ParserNodeContainer.Add(ParserNode);
                                    //ParserNode.Dump();
                                } break;
                            }
                            break;
                        default:
                            throw (new Exception(String.Format("Unprocessed Token Type '{0}'('{1}')", CurrentTokenType, CurrentToken.Text)));
                    }
                }
            }
            catch (Finalize_HandlingLevel_Root)
            {
            }

            return ParserNodeContainer;
        }
    }

    public class Template
    {
        TokenReader Tokens;

        static public List<TemplateToken> Tokenize(String TemplateString)
        {
            var Tokens = new List<TemplateToken>();
            TemplateTokenizer.Tokenize(Tokens, new TokenizerStringReader(TemplateString));
            return Tokens;
        }

        static public Template ParseFromString(String TemplateString)
        {
            var Template = new Template();

            Template.Tokens = new TokenReader(Template.Tokenize(TemplateString));
            //Matches.l
            //Console.WriteLine(Matches.Index);

            return Template;
        }

        public void RenderCodeTo(TextWriter TextWriter, dynamic Parameters = null)
        {
            var TemplateHandler = new TemplateHandler(Tokens, TextWriter, Parameters);
            var Context = new ParserNodeContext()
            {
                TextWriter = TextWriter,
                Parameters = Parameters,
            };
            var ParserNode = TemplateHandler.HandleLevel_Root();
            var OptimizedParserNode = ParserNode.Optimize(Context);
            //OptimizedParserNode.Dump();
            Context.TextWriter.WriteLine("using System;");
            Context.TextWriter.WriteLine("using CSharpUtils.Templates.Runtime;");
            Context.TextWriter.WriteLine("namespace CSharpUtils.Templates.CompiledTemplates {");
            Context.TextWriter.WriteLine("class CompiledTemplate_TempTemplate : TemplateCode {");
            Context.TextWriter.WriteLine("override public void Render(TemplateContext Context) { try {");
            OptimizedParserNode.WriteTo(Context);
            Context.TextWriter.WriteLine("} catch (Exception Exception) { Console.WriteLine(Exception); } }");
            Context.TextWriter.WriteLine("}");
            Context.TextWriter.WriteLine("}");
        }

        public String ExecuteCode(String Code, dynamic Parameters = null)
        {
            //Console.WriteLine(Code);

            CSharpCodeProvider CSharpCodeProvider = new CSharpCodeProvider();
            //Console.WriteLine(Assembly.GetExecutingAssembly().FullName);

            CompilerResults CompilerResults = CSharpCodeProvider.CompileAssemblyFromSource(
                new CompilerParameters(new string[] {
                    "System.dll",
                    "Microsoft.CSharp.dll",
                    "System.Core.dll",
                    System.Reflection.Assembly.GetAssembly(typeof(Template)).Location
                }),
                Code
            );

            if (CompilerResults.NativeCompilerReturnValue == 0)
            {
                Assembly assembly = CompilerResults.CompiledAssembly;
                Type Type = assembly.GetType("CSharpUtils.Templates.CompiledTemplates.CompiledTemplate_TempTemplate");

                TemplateContext TemplateContext = new TemplateContext(new StringWriter(), Parameters);

                TemplateCode TemplateCode = (TemplateCode)Activator.CreateInstance(Type);
                TemplateCode.Render(TemplateContext);

                //Console.WriteLine(TemplateContext.Output.ToString());
                return TemplateContext.Output.ToString();
            }
            else
            {
                Console.Error.WriteLine(Code);

                foreach (var Error in CompilerResults.Errors)
                {
                    Console.Error.WriteLine("Error: " + Error);
                }

                throw(new Exception("Error Compiling"));
            }
        }

        public String RenderToString(dynamic Parameters = null)
        {
            var StringWriter = new StringWriter();
            RenderCodeTo(StringWriter, Parameters);
            String CodeString = StringWriter.ToString();
            return ExecuteCode(CodeString, Parameters);
        }
    }
}
