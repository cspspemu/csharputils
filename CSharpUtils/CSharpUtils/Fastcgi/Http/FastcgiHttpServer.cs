using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CSharpUtils.Http;

namespace CSharpUtils.Fastcgi.Http
{
	public class HttpHeaderList
	{
		protected Dictionary<string, List<Tuple<string, string>>> Headers = new Dictionary<string, List<Tuple<string, string>>>();

		public void Set(String Name, String Value)
		{
			_Set(Name, Value, false);
		}

		public void Append(String Name, String Value)
		{
			_Set(Name, Value, true);
		}

		public void Remove(String Name)
		{
			Headers.Remove(Name);
		}

		protected void _Set(String Name, String Value, bool Append = false)
		{
			var KeyNormalizedName = Name;
			var TupleValue = new Tuple<string, string>(Name, Value);

			if (Append)
			{
				if (!Headers.ContainsKey(KeyNormalizedName))
				{
					Headers[KeyNormalizedName] = new List<Tuple<string, string>>();
				}
				Headers[KeyNormalizedName].Add(TupleValue);
			}
			else
			{
				Headers[KeyNormalizedName] = new List<Tuple<string, string>>() { TupleValue };
			}
		}

		public void SetCookie(String Name, String Value)
		{
			throw(new NotImplementedException());
			//Add("Set-Cookie", String.Format("{0}={1}", Name, Value));
		}

		public void WriteTo(TextWriter Output)
		{
			foreach (var Header in Headers)
			{
				foreach (var HeaderValue in Header.Value)
				{
					Output.WriteLine("{0}: {1}", HeaderValue.Item1, HeaderValue.Item2);
				}
			}
			Output.WriteLine("");
		}
	}

	public class HttpRequest
	{
		public bool OutputDebug;
		public Encoding Encoding = Encoding.UTF8;
		public HttpHeaderList Headers;
		public TextWriter Output;

		public Dictionary<String, String> Enviroment;
		public Dictionary<String, String> Post;
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
			using (var OutputTextWriter = new StringWriter())
			{
				var HttpRequest = new HttpRequest()
				{
					Headers = new HttpHeaderList(),
					Output = OutputTextWriter,
					Enviroment = FastcgiRequest.Params,
					Post = FastcgiRequest.PostParams,
					Get = HttpUtils.ParseUrlEncoded(FastcgiRequest.Params["QUERY_STRING"]),
					Cookies = new Dictionary<String, String>(),
				};

				HttpRequest.Headers.Set("X-Dynamic", "C#");
				HttpRequest.SetContentType("text/html", Encoding.UTF8);

				var Start = DateTime.Now;
				{
					HandleHttpRequest(HttpRequest);
				}
				var End = DateTime.Now;
				double GenerationTime = (End - Start).TotalSeconds;

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
