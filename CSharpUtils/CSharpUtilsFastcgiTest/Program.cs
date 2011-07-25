using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpUtils.Fastcgi;
using System.IO;
using System.Threading;
using CSharpUtils.Templates.TemplateProvider;
using CSharpUtils.Templates;
using CSharpUtils.Templates.Runtime;

namespace CSharpUtilsFastcgiTest
{
	class MyFastcgiServer : FastcgiServer
	{
		int Count = 0;
		TemplateProviderMemory TemplateProvider;
		TemplateFactory TemplateFactory;
		TemplateCode TemplateCode;

		public MyFastcgiServer() : base()
		{
			TemplateProvider = new TemplateProviderMemory();
			TemplateFactory = new TemplateFactory(TemplateProvider);

			TemplateProvider.Add("Base.html", "Test{% block Body %}Base{% endblock %}Test");
			TemplateProvider.Add("Test.html", "{% extends 'Base.html' %}{% block Body %}Ex{{ Count }}{% endblock %}");
			TemplateCode = TemplateFactory.GetTemplateCodeByFile("Test.html");

			//TemplateProvider.Add("Test.html", "{% block Body %}Ex{% endblock %}");
		}

		protected override void HandleFascgiRequest(FastcgiRequest FastcgiRequest)
		{
			using (var TextWriter = new StreamWriter(FastcgiRequest.StdoutStream, Encoding.UTF8))
			{
				TextWriter.WriteLine("X-Dynamic: C#");
				TextWriter.WriteLine("Content-Type: text/html; charset=utf-8");
				TextWriter.WriteLine("");
				//TextWriter.WriteLine(Count++);
				var Start = DateTime.Now;
				TextWriter.WriteLine(TemplateCode.RenderToString(new TemplateScope(new Dictionary<String, dynamic>()
				{
					{ "Count", Count++ }
				})));
				var End = DateTime.Now;
				TextWriter.WriteLine((End - Start).Milliseconds);

			}
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			var FastcgiServer = new MyFastcgiServer();
			FastcgiServer.Listen(9001, "127.0.0.1");
		}
	}
}
