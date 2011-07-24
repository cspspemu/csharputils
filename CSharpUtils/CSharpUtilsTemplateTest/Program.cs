using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpUtils.Templates;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using System.IO;
using CSharpUtils;
using CSharpUtils.VirtualFileSystem;
using CSharpUtils.VirtualFileSystem.Local;

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

            //Assembly.GetExecutingAssembly().GetManifestResourceStream();
            /*
            foreach (var Name in Assembly.GetExecutingAssembly().GetManifestResourceNames())
            {
                Console.WriteLine(Name);
            }
            */

            /*
            var FileSystem = new LocalFileSystem(FileUtils.GetExecutableDirectoryPath(), false);
            var TemplateString = FileSystem.OpenFile("Templates/Test.html", FileMode.Open).ReadAllContentsAsString();

            //Stream TemplateStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("CSharpUtilsTemplateTest.Templates.Test.html");
            //String TemplateString = TemplateStream.ReadAllContentsAsString();

            var Result = Template.ParseFromString(TemplateString).RenderToString(new Dictionary<String, object>() {
                { "User", "Test" },
                { "Value", 1 },
            });
            //Console.WriteLine(Result);
            */

            Console.WriteLine(Template.ParseFromString("{% for Item in List %}{{ Item }}{% endfor %}").RenderToString(new Dictionary<String, object>() {
                { "List", new int[] { 1, 2, 3, 4 } },
            }));

            Console.ReadKey();
        }
    }
}
