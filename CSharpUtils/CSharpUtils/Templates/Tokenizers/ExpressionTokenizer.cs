using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpUtils.Templates.Tokenizers
{
    public class ExpressionTokenizer
    {
        static String[] Operators2 = new String[]
        {
            "++", "--", "&&", "||",
        };
        static char[] Operators1 = new char[]
        {
            '+', '-', '*', '/', '%', '|', '(', ')', '{', '}', '[', ']', '.', ':', ',',
            '<', '>',
        };

        static public bool IsDecimalDigit(char Char)
        {
            return (Char >= '0') && (Char <= '9');
        }

        static public bool IsAlpha(char Char)
        {
            return (Char >= 'a' && Char <= 'z') || (Char >= 'A' && Char <= 'Z') || (Char == '_');
        }

        static public bool IsAlphaNumeric(char Char)
        {
            return IsAlpha(Char) || IsDecimalDigit(Char);
        }

        static public void Tokenize(List<TemplateToken> Tokens, TokenizerStringReader StringReader)
        {
            //StringReader.SkipSpaces();
            while (StringReader.Available > 0)
            {
                switch (StringReader.Peek(2))
                {
                    case "%}":
                    case "}}":
                        return;
                }

                var CharBase = StringReader.PeekChar();

                switch (CharBase)
                {
                    // Ignore Spaces
                    case ' ':
                    case '\t':
                    case '\r':
                    case '\n':
                    case '\v':
                        StringReader.Skip();
                        break;
                    // String
                    case '\'':
                    case '"':
                        {
                            StringReader.SegmentSetStart();
                            StringReader.Skip();
                            while (true)
                            {
                                var Char = StringReader.ReadChar();
                                if (Char == '\\')
                                {
                                    StringReader.ReadChar();
                                }
                                else if (Char == CharBase)
                                {
                                    break;
                                }
                            }
                            StringReader.SegmentSetEnd();
                            Tokens.Add(new StringLiteralTemplateToken()
                            {
                                Text = StringReader.SegmentGetSlice(),
                            });
                        }
                        break;
                    default:
                        // Numbers
                        if (IsDecimalDigit(CharBase))
                        {
                            StringReader.SegmentSetStart();
                            StringReader.Skip();
                            while (true)
                            {
                                var Char = StringReader.PeekChar();
                                if (IsDecimalDigit(Char))
                                {
                                    StringReader.Skip();
                                }
                                else
                                {
                                    break;
                                }
                            }
                            StringReader.SegmentSetEnd();
                            Tokens.Add(new NumericLiteralTemplateToken()
                            {
                                Text = StringReader.SegmentGetSlice(),
                            });
                        }
                        else
                        {
                            // Operators
                            if (Operators2.Contains(StringReader.Peek(2)))
                            {
                                Tokens.Add(new OperatorTemplateToken()
                                {
                                    Text = StringReader.ReadString(2),
                                });
                                break;
                            }

                            if (Operators1.Contains(CharBase))
                            {
                                Tokens.Add(new OperatorTemplateToken()
                                {
                                    Text = StringReader.ReadChar().ToString(),
                                });
                                break;
                            }

                            if (IsAlpha(CharBase))
                            {
                                StringReader.SegmentSetStart();
                                StringReader.Skip();
                                while (true)
                                {
                                    var Char = StringReader.PeekChar();
                                    if (IsAlphaNumeric(Char))
                                    {
                                        StringReader.Skip();
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                StringReader.SegmentSetEnd();
                                Tokens.Add(new IdentifierTemplateToken()
                                {
                                    Text = StringReader.SegmentGetSlice(),
                                });
                                break;
                            }

                            StringReader.Skip();

                            throw (new Exception("Unknown Token Type : '" + CharBase + "'"));
                        }
                        break;
                }
            }
        }
    }
}
