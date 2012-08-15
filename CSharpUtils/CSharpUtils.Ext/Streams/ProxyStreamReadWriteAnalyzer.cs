using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CSharpUtils.SpaceAssigner;

namespace CSharpUtils.Streams
{
    public class ProxyStreamReadWriteAnalyzer : ProxyStream
    {
        protected SpaceAssigner1D _ReadUsage = new SpaceAssigner1D();
        protected SpaceAssigner1D _WriteUsage = new SpaceAssigner1D();

        public ProxyStreamReadWriteAnalyzer(Stream BaseStream)
            : base(BaseStream)
        {
        }

        public SpaceAssigner1D.Space[] ReadUsage
        {
            get
            {
                return _ReadUsage.GetAvailableSpaces();
            }
        }

        public SpaceAssigner1D.Space[] WriteUsage
        {
            get
            {
                return _WriteUsage.GetAvailableSpaces();
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var Start = Position;

            try
            {
                return base.Read(buffer, offset, count);
            }
            finally
            {
                _ReadUsage.AddAvailable(Start, Position);
            }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            var Start = Position;

            try
            {
                base.Write(buffer, offset, count);
            }
            finally
            {
                _WriteUsage.AddAvailable(Start, Position);
            }
        }
    }
}
