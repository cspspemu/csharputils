using System;
using Tamir.SharpSsh.java.net;
using System.Net;

namespace Tamir.SharpSsh.jsch
{
	/// <summary>
	/// Summary description for ServerSocketFactory.
	/// </summary>
	public interface ServerSocketFactory
	{
		ServerSocket createServerSocket(int port, int backlog, IPAddress bindAddr);
	}
}
