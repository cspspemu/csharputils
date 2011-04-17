using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace CSharpUtils
{
	static public class StreamExtensions
	{
		static public String ReadAllContentsAsString(this Stream Stream, Encoding Encoding = null)
		{
			if (Encoding == null) Encoding = Encoding.UTF8;
			return Encoding.GetString(Stream.ReadAll());
		}

		static public byte[] ReadAll(this Stream Stream)
		{
			var Data = new byte[Stream.Length];
			Stream.Read(Data, 0, Data.Length);
			return Data;
		}

        static public byte[] ReadBytes(this Stream Stream, int ToRead)
        {
            var Buffer = new byte[ToRead];
            var Readed = Stream.Read(Buffer, 0, ToRead);
            if (Readed != ToRead) throw(new Exception("Unable to read " + ToRead + " bytes, readed " + Readed + "."));
            return Buffer;

        }

        static public String ReadStringz(this Stream Stream, int ToRead, Encoding Encoding = null)
        {
            if (Encoding == null) Encoding = Encoding.ASCII;
            return Encoding.GetString(Stream.ReadBytes(ToRead)).TrimEnd('\0');
        }

        public static T ReadStruct<T>(this Stream Stream) where T : struct
        {
            var Size = Marshal.SizeOf(typeof(T));
            var Buffer = new byte[Size];
            Stream.Read(Buffer, 0, Size);
            return StructUtils.BytesToStruct<T>(Buffer);
        }
	}

    public class ProxyStream : Stream
    {
        protected Stream ParentStream;

        public ProxyStream(Stream ParentStream)
        {
            this.ParentStream = ParentStream;
        }

        public override bool CanRead { get { return ParentStream.CanRead; } }
        public override bool CanSeek { get { return ParentStream.CanSeek; } }
        public override bool CanWrite { get { return ParentStream.CanWrite; } }

        public override void Flush()
        {
            ParentStream.Flush();
        }

        public override long Length
        {
            get { return ParentStream.Length; }
        }

        public override long Position
        {
            get
            {
                return ParentStream.Position;
            }
            set
            {
                ParentStream.Position = value;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return ParentStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return ParentStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            ParentStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            ParentStream.Write(buffer, offset, count);
        }
    }

    public class SliceStream : ProxyStream
    {
        protected long ThisPosition;
        protected long ThisStart, ThisLength;

        public SliceStream(Stream ParentStream, long ThisStart, long ThisLength, bool? CanWrite = null)
            : base(ParentStream)
        {
            this.ThisPosition = 0;
            this.ThisStart = ThisStart;
            this.ThisLength = ThisLength;
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
            throw(new NotImplementedException());
        }
    }
}
