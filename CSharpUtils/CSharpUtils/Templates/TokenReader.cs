using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpUtils.Templates
{
    public class TokenReader
    {
        int Index;
        List<TemplateToken> Tokens;

        public TokenReader(List<TemplateToken> Tokens)
        {
            this.Tokens = Tokens;

            foreach (var Token in Tokens) {
                Console.WriteLine(Token);
            }
        }

        public bool HasMore
        {
            get
            {
                return Index < Tokens.Count;
            }
        }

        public TemplateToken Current
        {
            get
            {
                return Tokens[Index];
            }
        }

        public void MoveStart()
        {
            Index = 0;
        }

        public void MovePrev()
        {
            if (Index > 0) Index--;
        }

        public void MoveNext()
        {
            if (HasMore) Index++;
        }

        public bool CurrentIsType(Type Type)
        {
            return (Current.GetType() == Type);
        }

        public void ExpectType(Type Type)
        {
            if (!CurrentIsType(Type)) throw (new Exception(String.Format("Expected token {0} but obtained {1}('{2}')", Type, Current.GetType(), Current.Text)));
        }

        public void ExpectValue(String ExpectedValue)
        {
            if (Current.Text != ExpectedValue) throw (new Exception(String.Format("Expected token '{0}' but obtained '{1}'", ExpectedValue, Current.Text)));
        }
    }
}
