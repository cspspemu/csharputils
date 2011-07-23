using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using CSharpUtils.Templates.Tokenizers;

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
                return CurrentToken.GetType().FullName;
            }
        }

        public ParserNode HandleLevel_Identifier()
        {
            ParserNode ParserNode;

            
            switch (CurrentTokenType)
            {
                case "CSharpUtils.Templates.OperatorTemplateToken":
                    switch (CurrentToken.Text)
                    {
                        case "(":
                            Tokens.MoveNext();
                            ParserNode = HandleLevel_Expression();
                            Tokens.ExpectValue(")");
                            break;
                        default:
                            throw (new Exception(String.Format("Invalid operator '{0}'", CurrentTokenType)));
                    }
                    break;
                case "CSharpUtils.Templates.NumericLiteralTemplateToken":
                    ParserNode = new ParserNodeNumericLiteral()
                    {
                        Value = Int64.Parse(CurrentToken.Text),
                    };
                    break;
                case "CSharpUtils.Templates.IdentifierTemplateToken":
                    ParserNode = new ParserNodeIdentifier()
                    {
                        Id = CurrentToken.Text
                    };
                    break;
                default:
                    throw (new Exception(String.Format("Invalid Identifier '{0}'('{1}')", CurrentTokenType, CurrentToken.Text)));
            }
            Tokens.MoveNext();

            return ParserNode;
        }

        public ParserNode HandleLevel_Sum()
        {
            ParserNode ParserNode = HandleLevel_Mul();

            while (Tokens.HasMore)
            {
                switch (CurrentTokenType)
                {
                    case "CSharpUtils.Templates.OperatorTemplateToken":
                        string Operator = CurrentToken.Text;
                        switch (Operator)
                        {
                            case "+": case "-":
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

        public ParserNode HandleLevel_Mul()
        {
            ParserNode ParserNode = HandleLevel_Identifier();

            while (Tokens.HasMore)
            {
                switch (CurrentTokenType)
                {
                    case "CSharpUtils.Templates.OperatorTemplateToken":
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

        public ParserNode HandleLevel_Expression()
        {
            return HandleLevel_Sum();
        }

        public ParserNode HandleLevel_Tag()
        {
            return HandleLevel_Expression();
        }

        public ParserNode HandleLevel_TagSpecial()
        {
            string SpecialType = CurrentToken.Text;
            switch (SpecialType)
            {
                case "if":
                    Tokens.MoveNext();

                    ParserNode ConditionNode = HandleLevel_Expression();
                    Tokens.ExpectValue("%}");
                    Tokens.MoveNext();
                    ParserNode BodyIfNode = HandleLevel_Root();
                    ParserNode BodyElseNode = new DummyParserNode();

                    switch (CurrentToken.Text)
                    {
                        case "endif":
                            Tokens.MoveNext();
                            Tokens.ExpectValue("%}");
                            Tokens.MoveNext();
                            break;
                        case "else":
                            Tokens.MoveNext();
                            Tokens.ExpectValue("%}");
                            Tokens.MoveNext();

                            BodyElseNode = HandleLevel_Root();

                            break;
                        default:
                            throw (new Exception(String.Format("Unprocessed Token Type '{0}'", CurrentTokenType)));
                    }

                    return new IfParserNode()
                    {
                        ConditionNode = ConditionNode,
                        BodyIfNode = BodyIfNode,
                        BodyElseNode = BodyElseNode,
                    };
                case "else":
                case "endif":
                    throw (new Finalize_HandlingLevel_Root());
            }
            return HandleLevel_Expression();
        }

        public ParserNode HandleLevel_Root()
        {
            var ParserNodeContainer = new ParserNodeContainer();

            try
            {
                for (; Tokens.HasMore; Tokens.MoveNext())
                {
                    switch (CurrentTokenType)
                    {
                        case "CSharpUtils.Templates.RawTemplateToken":
                            ParserNodeContainer.Add(new ParserNodeLiteral()
                            {
                                Text = ((RawTemplateToken)CurrentToken).Text,
                            });
                            break;
                        case "CSharpUtils.Templates.OpenTagTemplateToken":
                            string OpenType = CurrentToken.Text;

                            Tokens.MoveNext();

                            switch (OpenType)
                            {
                                case "{{":
                                    ParserNodeContainer.Add(new ParserNodeOutputExpression() { Parent = HandleLevel_Tag() });
                                    Tokens.ExpectValue("}}");
                                    break;
                                case "{%":
                                    ParserNodeContainer.Add(HandleLevel_TagSpecial());
                                    break;
                            }
                            break;
                        default:
                            throw (new Exception(String.Format("Unprocessed Token Type '{0}'", CurrentTokenType)));
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

        public void RenderTo(TextWriter TextWriter, dynamic Parameters = null)
        {
            var TemplateHandler = new TemplateHandler(Tokens, TextWriter, Parameters);
            var Context = new ParserNodeContext()
            {
                TextWriter = TextWriter,
                Parameters = Parameters,
            };
            TemplateHandler.HandleLevel_Root().Optimize(Context).WriteTo(Context);
        }

        public String RenderToString(dynamic Parameters = null)
        {
            var StringWriter = new StringWriter();
            RenderTo(StringWriter, Parameters);
            return StringWriter.ToString();
        }
    }
}
