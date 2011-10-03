using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;

namespace CSharpUtils.Net
{
	public class NetworkUtilities
	{
		/*
		static public int GetAvailablePort()
		{
			throw(new NotImplementedException());
		}
		*/

		static public ushort GetAvailableTcpPort(ushort StartingPort = 10101)
		{
			var IsPortBusy = new Dictionary<ushort, bool>();

			foreach (var TcpConnectionInfo in IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpConnections())
			{
				IsPortBusy[(ushort)TcpConnectionInfo.LocalEndPoint.Port] = true;
				/*
				Console.WriteLine(
					"{0} -> {1} :: {2}",
					tcpi.LocalEndPoint,
					tcpi.RemoteEndPoint,
					Enum.GetName(typeof(TcpState), tcpi.State)
				);
				*/
			}

			for (int Port = StartingPort; Port < UInt16.MaxValue; Port++)
			{
				if (!IsPortBusy.ContainsKey((ushort)Port))
				{
					return (ushort)Port;
				}
			}

			throw (new KeyNotFoundException("Can't find any free port"));
		}
	}
}
