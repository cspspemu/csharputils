using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CSharpUtils.Streams
{
    public class MapStream : Stream
    {
        protected class StreamEntry
        {
            public long Position;
            public Stream Stream;

            public long Length { get { return Stream.Length; } }
        }

        protected List<StreamEntry> StreamEntries;
        protected long _Position;
        protected Stream _CurrentStream;
        protected long _CurrentPositionInStream;

        public MapStream()
        {
            StreamEntries = new List<StreamEntry>();
        }

        public MapStream Map(long Offset, Stream Stream)
        {
            StreamEntries.Add(new StreamEntry() { Position = Offset, Stream = Stream });
            return this;
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void Flush()
        {
            foreach (var StreamEntry in StreamEntries) StreamEntry.Stream.Flush();
        }

        public override long Length
        {
            get
            {
                return StreamEntries.Max(StreamEntry => StreamEntry.Position + StreamEntry.Length);
            }
        }

        public override long Position
        {
            get
            {
                return _Position;
            }
            set
            {
                _Position = value;
                _PrepareCurrentStream();
            }
        }

        protected void _PrepareCurrentStream()
        {
            foreach (var StreamEntry in StreamEntries)
            {
                if (_Position >= StreamEntry.Position && _Position < StreamEntry.Position + StreamEntry.Length)
                {
                    _CurrentStream = StreamEntry.Stream;
                    _CurrentPositionInStream = _Position - StreamEntry.Position;
                    return;
                }
            }

            _CurrentStream = null;
            _CurrentPositionInStream = 0;

            //_Position
        }

        protected long GetAvailableBytesOnCurrentStream()
        {
            return _CurrentStream.Length - _CurrentPositionInStream;
        }

        public override int Read(byte[] Buffer, int Offset, int Count)
        {
            if (_CurrentStream == null) throw (new InvalidOperationException(String.Format("Invalid/Unmapped MapStream position {0}", Position)));

            var AvailableCount = (int)GetAvailableBytesOnCurrentStream();

            if (Count > AvailableCount)
            {
                // Read from current Stream.
                var Readed1 = Read(Buffer, Offset, AvailableCount);

                if (Readed1 == 0)
                {
                    return 0;
                }
                else
                {
                    // Try to read from the next Stream.
                    var Readed2 = Read(Buffer, Offset + AvailableCount, Count - AvailableCount);

                    return Readed1 + Readed2;
                }
            }
            else
            {
                var ActualReaded = _CurrentStream.Read(Buffer, Offset, Count);
                Position += ActualReaded;

                return ActualReaded;
            }
        }

        public override void Write(byte[] Buffer, int Offset, int Count)
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin: Position = offset; break;
                case SeekOrigin.Current: Position = Position + offset; break;
                case SeekOrigin.End: Position = Length + offset; break;
            }
            return offset;
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }
    }
}
