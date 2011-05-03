using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CSharpUtils.Net
{
	public class SocketLineReader
	{
		Socket Socket;

		public SocketLineReader(Socket Socket)
		{
			this.Socket = Socket;
			
		}

		public String ReadLine()
		{
		}
	}
}
