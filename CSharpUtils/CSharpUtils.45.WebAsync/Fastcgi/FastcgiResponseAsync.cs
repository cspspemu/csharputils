using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpUtils.Web._45.Fastcgi
{
	public class FastcgiResponseAsync
	{
		public FastcgiOutputStream StdoutStream;
		public FastcgiOutputStream StderrStream;
		public FastcgiHeaders Headers;

		public StreamWriter StdoutWriter;
		public StreamWriter StderrWriter;
	}
}
