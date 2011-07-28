using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CSharpUtils.Fastcgi.Http
{
	public class HttpFile
	{
		public string FileName;
		public string ContentType;
		public FileInfo TempFile;
	}
}
