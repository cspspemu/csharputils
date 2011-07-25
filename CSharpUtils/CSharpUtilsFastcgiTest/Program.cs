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

namespace CSharpUtilsFastcgiTest
{
	class MyFastcgiServer : FastcgiServer
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

		protected override void HandleFascgiRequest(FastcgiRequest FastcgiRequest)
		{
			using (var TextWriter = new StreamWriter(FastcgiRequest.StdoutStream, Encoding.UTF8))
			{
				TextWriter.WriteLine("X-Dynamic: C#");
				TextWriter.WriteLine("Content-Type: text/html; charset=utf-8");
				TextWriter.WriteLine("");
				//TextWriter.WriteLine(Count++);
				var Start = DateTime.Now;

				if (FastcgiRequest.PostParams.ContainsKey("Title"))
				{
					Posts.Add(new Post()
					{
						Title = FastcgiRequest.PostParams["Title"],
						Body = FastcgiRequest.PostParams["Body"],
					});
				}

				/*
				foreach (var Param in FastcgiRequest.Params)
				{
					Console.WriteLine("{0} : {1}", Param.Key, Param.Value);
				}
				*/

				TextWriter.WriteLine(TemplateCode.RenderToString(new TemplateScope(new Dictionary<String, dynamic>()
				{
					{ "Count", Count++ },
					{ "Params", FastcgiRequest.Params },
					{ "Posts", Posts },
					{ "POST", FastcgiRequest.PostParams },
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
