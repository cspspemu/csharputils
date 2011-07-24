using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

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
