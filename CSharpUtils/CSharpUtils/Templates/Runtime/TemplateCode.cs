using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Dynamic;

namespace CSharpUtils.Templates.Runtime
{
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
