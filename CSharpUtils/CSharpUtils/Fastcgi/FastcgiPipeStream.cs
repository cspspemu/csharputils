using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CSharpUtils.Fastcgi
{
    class FastcgiPipeStream : IFastcgiPipe
    {
        Stream Stream;

        public FastcgiPipeStream(Stream Stream)
        {
            this.Stream = Stream;
        }

        public void Write(byte[] Data, int Offset, int Length)
        {
            Stream.Write(Data, Offset, Length);
        }

        public int Read(byte[] Data, int Offset, int Length)
        {
            return Stream.Read(Data, Offset, Length);
        }

        public void Close()
        {
            Stream.Close();
        }
    }
}
