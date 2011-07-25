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
using CSharpUtils.VirtualFileSystem.Local;
using CSharpUtils;
using CSharpUtils.Fastcgi.Http;

namespace CSharpUtilsFastcgiTest
{
	class MyFastcgiServer : FastcgiHttpServer
	{
		int Count = 0;
		TemplateFactory TemplateFactory;
		TemplateCode TemplateCode;

		public class Post
		{
			public string Title;
			public string Body;
		}
		List<Post> Posts;

		public MyFastcgiServer() : base()
		{
			TemplateFactory = new TemplateFactory(new TemplateProviderVirtualFileSystem(new LocalFileSystem(FileUtils.GetExecutableDirectoryPath() + @"\Templates")));

			TemplateCode = TemplateFactory.GetTemplateCodeByFile("Test.html");

			Posts = new List<Post>();

			Posts.Add(new Post()
			{
				Title = "Sample Title",
				Body = "Sample Body",
			});

			//TemplateProvider.Add("Test.html", "{% block Body %}Ex{% endblock %}");
		}

		protected override void HandleHttpRequest(HttpRequest HttpRequest)
		{
			if (HttpRequest.Post.ContainsKey("Title"))
			{
				Posts.Add(new Post()
				{
					Title = HttpRequest.Post["Title"],
					Body = HttpRequest.Post["Body"],
				});
			}

			HttpRequest.Output.Write(TemplateCode.RenderToString(new TemplateScope(new Dictionary<String, dynamic>()
			{
				{ "Count", Count++ },
				{ "Params", HttpRequest.Enviroment },
				{ "Posts", Posts },
				{ "POST", HttpRequest.Post },
			})));

			HttpRequest.Output.WriteLine("<pre>");
			foreach (var Param in HttpRequest.Enviroment)
			{
				HttpRequest.Output.WriteLine("{0}: {1}", Param.Key, Param.Value);
			}
			HttpRequest.Output.WriteLine("</pre>");

			HttpRequest.OutputDebug = true;
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
