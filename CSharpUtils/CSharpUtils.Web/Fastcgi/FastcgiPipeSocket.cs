using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace CSharpUtils.Fastcgi
{
    class FastcgiPipeSocket : IFastcgiPipe
    {
        Socket Socket;

        public FastcgiPipeSocket(Socket Socket)
        {
            this.Socket = Socket;
        }

        public void Write(byte[] Data, int Offset, int Length)
        {
            Socket.Send(Data, Offset, Length, SocketFlags.None);
        }

        public int Read(byte[] Data, int Offset, int Length)
        {
            return Socket.Receive(Data, Offset, Length, SocketFlags.None);
        }

        public void Close()
        {
            Socket.Close();
        }
    }
}
