using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpUtils.Templates;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;

namespace CSharpUtilsTemplateTest
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            CSharpCodeProvider myCodeProvider = new CSharpCodeProvider();
            Console.WriteLine(Assembly.GetExecutingAssembly().FullName);
            CompilerResults compilerResults = myCodeProvider.CompileAssemblyFromSource(
                new CompilerParameters(new string[] { "System.dll", "CSharpUtils.dll" }),
                @" 
                using System;
                using CSharpUtils;

                namespace TestNamespace {
                    class Test {
                        static public void Main(string[] args) {
                            Console.WriteLine(1234567);
                            Console.WriteLine(Endianness.LittleEndian);
                        }
                    }
                }
                "
            );

            //compilerResults.
            Assembly assembly = compilerResults.CompiledAssembly;
            Type Type = assembly.GetType("TestNamespace.Test");
            Type.InvokeMember("Main", BindingFlags.InvokeMethod, null, null, new object[] { new string[] { } });
            */



            //Console.WriteLine(compilerResults.Errors.Count);
            //Console.WriteLine(compilerResults.Errors[0]);

            //compilerResults.CompiledAssembly

            Console.WriteLine(Template.ParseFromString(@"
                {% extends 'Base' %}
                Hello {{ User }} {{ ((1) + 2) * 2 + 3 * 4 + 1 + Value }}
                {% if Value %}true{% else %}false{% endif %}
                {% block MyBlock %}MyBlock Text{% if 1 %}Test{% endif %}{% endblock %}
            ").RenderToString(new Dictionary<String, object>() {
                { "User", "Test" },
                { "Value", 1 },
            }));
            Console.ReadKey();
        }
    }
}
