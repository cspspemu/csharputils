using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CSharpUtils.Fastcgi
{
	public class FastcgiHandler
	{
		public FastcgiPacketReader Reader;
		public FastcgiPacketWriter Writer;

		protected Dictionary<int, FastcgiRequest> Requests;

		public FastcgiHandler(Stream InputStream, Stream OutputStream)
		{
			this.Reader = new FastcgiPacketReader(InputStream);
			this.Writer = new FastcgiPacketWriter(OutputStream);
		}

		public FastcgiRequest GetOrCreateFastcgiRequest(ushort RequestId)
		{
			if (!Requests.ContainsKey(RequestId))
			{
				return Requests[RequestId] = new FastcgiRequest(this, RequestId);
			}
			return Requests[RequestId];
		}
	}
}
