using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CSharpUtils.Http;
using System.Diagnostics;
using CSharpUtils.Extensions;

namespace CSharpUtils.Fastcgi.Http
{
	public class HttpFile
	{
		public string FileName;
		public string ContentType;
		public FileInfo TempFile;
	}

	public class HttpRequest
	{
		public bool OutputDebug;
		public Encoding Encoding = Encoding.UTF8;
		public HttpHeaderList Headers;
		public TextWriter Output;

		public Dictionary<String, String> Enviroment;
		public Dictionary<String, String> Post;
		public Dictionary<String, HttpFile> Files;
		public Stream StdinStream;
		public Dictionary<String, String> Get;
		public Dictionary<String, String> Cookies;

		public void SetContentType(string MimeType, Encoding Encoding)
		{
			Headers.Set("Content-Type", MimeType + "; charset=" + Encoding.ToString());
			this.Encoding = Encoding;
		}
	}

	abstract public class FastcgiHttpServer : FastcgiServer
	{
		sealed override protected void HandleFascgiRequest(FastcgiRequest FastcgiRequest)
		{
			FastcgiRequest.StdinStream.SetPosition(0);
			Dictionary<string, string> Post = new Dictionary<string, string>();
			Dictionary<string, HttpFile> Files = new Dictionary<string, HttpFile>();

			HttpHeader ContentType = new HttpHeader("Content-Type", FastcgiRequest.Params["CONTENT_TYPE"]);

			if (FastcgiRequest.StdinStream.Length > 0)
			{
				File.WriteAllBytes("Stdin.bin", FastcgiRequest.StdinStream.ReadAll());
			}

			var ContentTypeParts = ContentType.ParseValue("type");
			if (ContentTypeParts["type"] == "multipart/form-data")
			{
				string Boundary = ContentTypeParts["boundary"];
				File.WriteAllBytes("Boundary.bin", Encoding.ASCII.GetBytes(Boundary));
				MultipartDecoder MultipartDecoder = new MultipartDecoder(FastcgiRequest.StdinStream, "--" + Boundary);
				var Parts = MultipartDecoder.Parse();

				foreach (var Part in Parts)
				{
					if (Part.IsFile)
					{
						Files.Add(Part.Name, new HttpFile() {
							TempFile = new FileInfo(Part.TempFilePath),
							FileName = Part.FileName,
							ContentType = Part.ContentType,
						});
					}
					else
					{
						Post[Part.Name] = Part.Content;
					}
				}
			}
			else
			{
				Post = HttpUtils.ParseUrlEncoded(Encoding.UTF8.GetString(FastcgiRequest.StdinStream.ReadAll()));
			}

			//CONTENT_TYPE: multipart/form-data; boundary=----WebKitFormBoundaryIMw3ByBOPx38V6Bd

			using (var OutputTextWriter = new StringWriter())
			{
				var HttpRequest = new HttpRequest()
				{
					Headers = new HttpHeaderList(),
					Output = OutputTextWriter,
					Enviroment = FastcgiRequest.Params,
					StdinStream = FastcgiRequest.StdinStream,
					Post = Post,
					Files = Files,
					Get = HttpUtils.ParseUrlEncoded(FastcgiRequest.Params["QUERY_STRING"]),
					Cookies = new Dictionary<String, String>(),
				};

				HttpRequest.Headers.Set("X-Dynamic", "C#");
				HttpRequest.SetContentType("text/html", Encoding.UTF8);

				var Stopwatch = new Stopwatch();
				Stopwatch.Start();
				{
					HandleHttpRequest(HttpRequest);
				}
				Stopwatch.Stop();
				double GenerationTime = (double)Stopwatch.ElapsedTicks / (double)Stopwatch.Frequency;

				HttpRequest.Headers.Set("X-GenerationTime", String.Format("{0}", GenerationTime));

				using (var Stdout = new StreamWriter(FastcgiRequest.StdoutStream))
				{
					HttpRequest.Headers.WriteTo(Stdout);
				}

				using (var Stdout = new StreamWriter(FastcgiRequest.StdoutStream, HttpRequest.Encoding))
				{
					Stdout.Write(OutputTextWriter.ToString());
					if (HttpRequest.OutputDebug)
					{
						Stdout.WriteLine("<pre>");
						Stdout.WriteLine("Generation Time: {0}", GenerationTime);
						Stdout.WriteLine("</pre>");
					}
				}
			}
		}

		abstract protected void HandleHttpRequest(HttpRequest HttpRequest);
	}
}
