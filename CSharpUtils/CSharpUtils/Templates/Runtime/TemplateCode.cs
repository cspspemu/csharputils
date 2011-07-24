using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Dynamic;

namespace CSharpUtils.Templates.Runtime
{
    public class TemplateContext
    {
        public TextWriter Output;
        public dynamic Parameters;

        public dynamic GetVar(String Name)
        {
            return Parameters[Name];
        }

        public void SetVar(String Name, dynamic Value)
        {
            Parameters[Name] = Value;
        }

        public bool ToBool(dynamic Value)
        {
            try
            {
                if (Value == null) return false;
                if (Value is bool) return Value;
                if (Value is int) return Value != 0;
                if (Value is Int64) return Value != 0;
                if (Value is object) return true;
                return (bool)Value;
            }
            catch (Exception Exception)
            {
                Console.WriteLine("VALUE: {0}", Value);
                Console.Error.WriteLine(Exception);
                return false;
            }
        }
    }

    public class TemplateCode
    {
        virtual public void Init()
        {
        }

        virtual public void Render(TemplateContext Context)
        {
        }

        protected void SetParentClass(String ClassName)
        {
        }

        protected void CallBlock(String Block)
        {
        }

        protected void CallParentBlock(String Block)
        {
        }
    }
}
