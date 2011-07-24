using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpUtils.Templates;
using CSharpUtils.Templates.TemplateProvider;
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
			//TemplateProvider TemplateProvider = new TemplateProviderVirtualFileSystem(new LocalFileSystem(FileUtils.GetExecutableDirectoryPath(), false));
			TemplateProviderMemory TemplateProvider = new TemplateProviderMemory();
			TemplateFactory TemplateFactory = new TemplateFactory(TemplateProvider);

			TemplateProvider.Add("Base.html", "Test{% block Body %}Base{% endblock %}Test");
			TemplateProvider.Add("Test.html", "{% extends 'Base.html' %}{% block Body %}Ex{% endblock %}");
			//TemplateProvider.Add("Test.html", "{% block Body %}Ex{% endblock %}");

			Console.WriteLine(TemplateFactory.GetTemplateCodeByFile("Test.html").RenderToString());

			Console.ReadKey();
		}
	}
}
