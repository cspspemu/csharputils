using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace CSharpUtils.Templates
{
    public class TemplateToken
    {
        public String Text;
    }

    public class RawTemplateToken : TemplateToken
    {
    }

    public class OpenTagTemplateToken : TemplateToken
    {
    }

    public class CloseTagTemplateToken : TemplateToken
    {
    }

    public class StringLiteralTemplateToken : TemplateToken
    {
    }

    public class NumericLiteralTemplateToken : TemplateToken
    {
    }

    public class OperatorTemplateToken : TemplateToken
    {
    }

    public class IdentifierTemplateToken : TemplateToken
    {
    }

    public class StringReaderMatch
    {
        public bool Success;
        public int Index;
        public int Length;
        public String Value;
    }

    public class StringReader
    {
        public String Text;
        public int Offset;

        public StringReader(String Text, int Offset = 0)
        {
            this.Text = Text;
            this.Offset = Offset;
        }

        public int IndexOf(String Str)
        {
            int Index = Text.IndexOf(Str, Offset);
            if (Index != -1) Index -= Offset;
            return Index;
        }

        public StringReaderMatch Match(Regex Regex)
        {
            var Return = Regex.Match(Text, Offset);
            return new StringReaderMatch()
            {
                Success = Return.Success,
                Index = Return.Success ? Return.Index - Offset : -1,
                Length = Return.Success ? Return.Length : 0,
                Value = Return.Success ? Return.Value : null,
            };
        }

        public char ReadChar()
        {
            return Text[Offset++];
        }

        public void Unread(int Count = 1)
        {
            Offset -= Count;
        }

        public void Skip(int Count = 1)
        {
            Offset += Count;
        }

        public String ReadString(int Length)
        {
            var Return = Text.Substring(Offset, Length);
            Offset += Length;
            return Return;
        }

        public void SkipSpaces()
        {
            var Match = this.Match(new Regex(@"\s+", RegexOptions.Compiled));
            if (Match.Index == 0)
            {
                Skip(Match.Length);
            }
        }

        public String Peek(int Count)
        {
            return Text.Substring(Offset, Count);
        }

        public Char PeekChar()
        {
            return Text[Offset];
        }

        public String GetSlice(int Start, int End)
        {
            return Text.Substring(Start, End - Start);
        }

        int SegmentStart;
        int SegmentEnd;

        public void SegmentSetStart(int Offset = 0)
        {
            SegmentStart = this.Offset + Offset;
        }

        public void SegmentSetEnd(int Offset = 0)
        {
            SegmentEnd = this.Offset + Offset;
        }

        public String SegmentGetSlice()
        {
            return GetSlice(SegmentStart, SegmentEnd);
        }

        public String ReadString()
        {
            return ReadString(Available);
        }

        public int Available
        {
            get
            {
                return Text.Length - Offset;
            }
        }
    }

    public class ExpressionTokenizer
    {
        static String[] Operators2 = new String[]
        {
            "++", "--", "&&", "||",
        };
        static char[] Operators1 = new char[]
        {
            '+', '-', '*', '/', '%', '|', '(', ')', '{', '}', '[', ']', '.', ':', ',',
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

        static public void Tokenize(List<TemplateToken> Tokens, StringReader StringReader)
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
                    case ' ': case '\t': case '\r': case '\n': case '\v':
                        StringReader.Skip();
                        break;
                    // String
                    case '\'': case '"':
                        {
                            StringReader.SegmentSetStart();
                            StringReader.Skip();
                            while (true)
                            {
                                var Char = StringReader.ReadChar();
                                if (Char == '\\')
                                {
                                    StringReader.ReadChar();
                                } else if (Char == CharBase)
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

    public class TemplateTokenizer
    {
        static Regex StartTagRegex = new Regex(@"\{[\{%]", RegexOptions.Compiled);

        static public void Tokenize(List<TemplateToken> Tokens, StringReader StringReader)
        {
            while (true)
            {
                var Match = StringReader.Match(StartTagRegex);

                if (Match.Success)
                {
                    var RawText = StringReader.ReadString(Match.Index);
                    if (RawText.Length > 0) Tokens.Add(new RawTemplateToken() { Text = RawText });
                    var OpenTagTokenString = StringReader.ReadString(2);
                    Tokens.Add(new OpenTagTemplateToken() { Text = OpenTagTokenString });
                    {
                        ExpressionTokenizer.Tokenize(Tokens, StringReader);
                    }
                    var CloseTagTokenString = StringReader.ReadString(2);
                    var ExpectedCloseTagTokenString = (OpenTagTokenString == "{{") ? "}}" : "%}";
                    if (CloseTagTokenString != ExpectedCloseTagTokenString)
                    {
                        throw (new Exception("Expected '" + ExpectedCloseTagTokenString + "' but got '" + CloseTagTokenString + "'"));
                    }
                    Tokens.Add(new CloseTagTemplateToken() { Text = CloseTagTokenString });
                    continue;
                }
                else
                {
                    var RawText = StringReader.ReadString();
                    if (RawText.Length > 0) Tokens.Add(new RawTemplateToken() { Text = RawText });
                    return;
                }
            }
        }
    }

    public class Template
    {
        static public List<TemplateToken> Tokenize(String TemplateString)
        {
            var Tokens = new List<TemplateToken>();
            TemplateTokenizer.Tokenize(Tokens, new StringReader(TemplateString));
            return Tokens;
        }

        static public Template ParseFromString(String TemplateString)
        {
            var Template = new Template();
            //Matches.l
            //Console.WriteLine(Matches.Index);

            return Template;
        }

        public void RenderTo(TextWriter TextWriter, dynamic Parameters = null)
        {
        }

        public String RenderToString(dynamic Parameters = null)
        {
            var StringWriter = new StringWriter();
            RenderTo(Parameters, StringWriter);
            return StringWriter.ToString();
        }
    }
}
