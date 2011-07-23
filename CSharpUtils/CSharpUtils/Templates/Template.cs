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
                    String Operator = CurrentToken.Text;
                    switch (Operator)
                    {
                        // Unary Operators
                        /*
                        case "+": case "-":
                            Tokens.MoveNext();
                            ParserNode = new ParserNodeUnaryOperation()
                            {
                                Parent = HandleLevel_Expression(),
                                Operator = Operator,
                            };
                            Tokens.MoveNext();
                            
                            break;
                        */
                        case "(":
                            Tokens.MoveNext();
                            ParserNode = HandleLevel_Expression();
                            Tokens.ExpectValue(")");
                            //Tokens.ExpectValueAndNext(")");
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
                case "CSharpUtils.Templates.StringLiteralTemplateToken":
                    ParserNode = new ParserNodeStringLiteral()
                    {
                        Value = CurrentToken.Text,
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
                                Tokens.ExpectValue("%}");
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
                case "else":
                case "endif":
                case "endblock":
                    throw (new Finalize_HandlingLevel_Root());
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
                case "extends":
                    Tokens.MoveNext();
                    ParserNodeExtends ParserNodeExtends = new ParserNodeExtends() { Parent = HandleLevel_Expression() };
                    Tokens.ExpectValueAndNext("%}");
                    return ParserNodeExtends;
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

        public void RenderTo(TextWriter TextWriter, dynamic Parameters = null)
        {
            var TemplateHandler = new TemplateHandler(Tokens, TextWriter, Parameters);
            var Context = new ParserNodeContext()
            {
                TextWriter = TextWriter,
                Parameters = Parameters,
            };
            var ParserNode = TemplateHandler.HandleLevel_Root();
            var OptimizedParserNode = ParserNode.Optimize(Context);
            OptimizedParserNode.Dump();
            OptimizedParserNode.WriteTo(Context);
        }

        public String RenderToString(dynamic Parameters = null)
        {
            var StringWriter = new StringWriter();
            RenderTo(StringWriter, Parameters);
            return StringWriter.ToString();
        }
    }
}
