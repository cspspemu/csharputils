using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace CSharpUtils.Fastcgi
{
	public class FastcgiHandler
	{
        public bool Debug = false;
		public FastcgiPacketReader Reader;
		public FastcgiPacketWriter Writer;

        public delegate void HandleFastcgiRequestDelegate(FastcgiRequest FastcgiRequest);

        public event HandleFastcgiRequestDelegate HandleFastcgiRequest;

        protected Dictionary<ushort, FastcgiRequest> Requests = new Dictionary<ushort, FastcgiRequest>();

        public FastcgiHandler(Socket Socket, bool Debug = false)
        {
            Socket.Blocking = true;
            Init(new NetworkStream(Socket), new NetworkStream(Socket), Debug);
            this.Writer.Socket = Socket;
        }

        public FastcgiHandler(Stream InputStream, Stream OutputStream, bool Debug = false)
		{
            Init(InputStream, OutputStream, Debug);
		}

        protected void Init(Stream InputStream, Stream OutputStream, bool Debug = false)
        {
            this.Reader = new FastcgiPacketReader(InputStream, Debug);
            this.Writer = new FastcgiPacketWriter(OutputStream, Debug);

            this.Reader.HandlePacket += new FastcgiPacketHandleDelegate(Reader_HandlePacket);
        }

        bool Reader_HandlePacket(Fastcgi.PacketType Type, ushort RequestId, byte[] Content)
        {
            var Request = GetOrCreateFastcgiRequest(RequestId);

            if (Debug)
            {
                Console.WriteLine("Handling Packet (" + Type + ")");
            }

            switch (Type)
            {
                case Fastcgi.PacketType.FCGI_BEGIN_REQUEST:
                    var Role = (Fastcgi.Role)(Content[0] | (Content[1] << 8));
                    var Flags = (Fastcgi.Flags)(Content[2]);
                    break;
                case Fastcgi.PacketType.FCGI_PARAMS:
                    if (Content.Length == 0)
                    {
                        Request.ParamsStream.Finalized = true;
                    }
                    else
                    {
                        Request.ParamsStream.Write(Content, 0, Content.Length);
                    }
                    break;
                case Fastcgi.PacketType.FCGI_STDIN:
                    if (Content.Length == 0)
                    {
                        Request.StdinStream.Finalized = true;
                        Request.FinalizedRequest = true;
                    }
                    else
                    {
                        Request.StdinStream.Write(Content, 0, Content.Length);
                    }
                    break;

                default:
                    throw (new Exception("Unhandled packet type: '" + Type + "'"));
            }

            if (Debug)
            {
                Console.WriteLine("     : FinalizedRequest(" + Request.FinalizedRequest + ")");
            }

            if (Request.FinalizedRequest)
            {
                if (HandleFastcgiRequest != null)
                {
                    new Thread(() =>
                    {
                        int Result = 0;
                        try
                        {
                            HandleFastcgiRequest(Request);
                            Result = 0;
                        }
                        catch (Exception Exception)
                        {
                            Result = -1;
                            Console.Error.WriteLine(Exception);
                        }
                        Request.StdoutStream.Flush();
                        Request.StderrStream.Flush();
                        Writer.WritePacket(RequestId, Fastcgi.PacketType.FCGI_STDOUT, new byte[0]);
                        Writer.WritePacketEndRequest(RequestId, Result, Fastcgi.ProtocolStatus.FCGI_REQUEST_COMPLETE);
                        Writer.Stream.Flush();
                        if (Debug)
                        {
                            Console.WriteLine("Completed Request(RequestId={0}, Result={1})", RequestId, Result);
                        }

                        lock (Requests)
                        {
                            Requests.Remove(RequestId);
                            if (Requests.Count == 0)
                            {
                                Writer.Socket.Close();
                            }
                        }
                    }).Start();
                }
                return false;
            }
            else
            {
                return true;
            }
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
