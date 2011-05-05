using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace CSharpUtils.Templates
{
    public class Template
    {
        class TemplateTokenizer
        {
        }

        static public Template ParseFromString(String TemplateString)
        {
            var Template = new Template();
            var StartTagRegex = new Regex(@"\{[\{%]", RegexOptions.Compiled);
            var Matches = StartTagRegex.Match(TemplateString, 0);
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
