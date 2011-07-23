using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpUtils.Templates;

namespace CSharpUtilsTemplateTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Template.ParseFromString(@"
                Hello {{ User }} {{ ((1) + 2) * 2 + 3 * 4 + 1 + Value }}
                {% if 1 %}Hello World{% endif %}
            ").RenderToString(new Dictionary<String, object>() {
                { "User", "Test" },
                { "Value", 1 },
            }));
            Console.ReadKey();
        }
    }
}
