using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpUtils.Web._45.Fastcgi
{
	public class FastcgiRequestAsync
	{
		public Dictionary<String, String> Params;
		public FastcgiInputStream Stdin;
		public FastcgiOutputStream Stdout;
		public FastcgiOutputStream Stderr;
	}
}
