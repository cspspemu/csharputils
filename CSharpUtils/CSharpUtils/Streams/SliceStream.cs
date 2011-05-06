using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CSharpUtils.Streams
{
    public class SliceStream : ProxyStream
    {
        protected long ThisPosition;
        protected long ThisStart, ThisLength;

        static public SliceStream CreateWithLength(Stream ParentStream, long ThisStart, long ThisLength = -1, bool? CanWrite = null)
        {
            return new SliceStream(ParentStream, ThisStart, ThisLength, CanWrite);
        }

        static public SliceStream CreateWithBounds(Stream ParentStream, long LowerBound, long UpperBound, bool? CanWrite = null)
        {
            return new SliceStream(ParentStream, LowerBound, UpperBound - LowerBound, CanWrite);
        }

        public SliceStream(Stream ParentStream, long ThisStart = 0, long ThisLength = -1, bool? CanWrite = null)
            : base(ParentStream)
        {
			if (!ParentStream.CanSeek) throw(new NotImplementedException("ParentStream must be seekable"));

            this.ThisPosition = 0;
            this.ThisStart = ThisStart;
            if (ThisLength == -1)
            {
                this.ThisLength = ParentStream.Length - ThisStart;
            }
            else
            {
                this.ThisLength = ThisLength;
            }
        }

        public override long Length
        {
            get
            {
                return ThisLength;
            }
        }

        public override long Position
        {
            get
            {
                return ThisPosition;
            }
            set
            {
                if (value < 0) value = 0;
                if (value > Length) value = Length;
                ThisPosition = value;
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            //Console.WriteLine("Seek(offset: {0}, origin: {1})", offset, origin);
            switch (origin)
            {
                case SeekOrigin.Begin:
                    Position = offset;
                    break;
                case SeekOrigin.Current:
                    Position = Position + offset;
                    break;
                case SeekOrigin.End:
                    Position = Length + offset;
                    break;
            }
            //Console.WriteLine("   {0}", Position);
            return Position;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            lock (ParentStream)
            {
                var ParentStreamPositionToRestore = ParentStream.Position;
                ParentStream.Position = ThisStart + Position;
                if (Position + count > Length)
                {
                    count = (int)(Length - Position);
                }
                try
                {
                    //Console.WriteLine("Read(position: {0}, count: {1})", Position, count);
                    return base.Read(buffer, offset, count);
                }
                finally
                {
                    Seek(count, SeekOrigin.Current);
                    ParentStream.Position = ParentStreamPositionToRestore;
                }
            }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            lock (ParentStream)
            {
                var ParentStreamPositionToRestore = ParentStream.Position;
                ParentStream.Position = ThisStart + Position;
                if (Position + count > Length)
                {
                    count = (int)(Length - Position);
                }
                try
                {
                    base.Write(buffer, offset, count);
                }
                finally
                {
                    Seek(count, SeekOrigin.Current);
                    ParentStream.Position = ParentStreamPositionToRestore;
                }
            }
        }

        public override void SetLength(long value)
        {
            throw (new NotImplementedException());
        }
    }

}
