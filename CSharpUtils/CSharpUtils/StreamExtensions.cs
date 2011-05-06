using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Threading;
using CSharpUtils.Streams;

namespace CSharpUtils
{
	static public class StreamExtensions
	{
        static public bool Eof(this Stream Stream)
        {
            return Stream.Available() <= 0;
        }

        static public long Available(this Stream Stream)
        {
            return Stream.Length - Stream.Position;
        }

		static public byte[] ReadUntil(this Stream Stream, byte ExpectedByte, bool IncludeExpectedByte)
		{
			bool Found = false;
			var Buffer = new MemoryStream();
			while (!Found)
			{
				int b = Stream.ReadByte();
				if (b == -1) throw (new Exception("End Of Stream"));

				if (b == ExpectedByte)
				{
					Found = true;
					if (!IncludeExpectedByte) break;
				}

				Buffer.WriteByte((byte)b);
			}
			return Buffer.ToArray();
		}

		static public String ReadUntilString(this Stream Stream, byte ExpectedByte, Encoding Encoding, bool IncludeExpectedByte)
		{
			return Encoding.GetString(Stream.ReadUntil(ExpectedByte, IncludeExpectedByte));
		}

		static public String ReadAllContentsAsString(this Stream Stream, Encoding Encoding = null)
		{
			if (Encoding == null) Encoding = Encoding.UTF8;
			return Encoding.GetString(Stream.ReadAll());
		}

		static public byte[] ReadAll(this Stream Stream)
		{
            lock (Stream)
            {
				/*
				if (Stream.CanSeek)
				{
					var OldPosition = Stream.Position;
					var Data = new byte[Stream.Length];
					Stream.Position = 0;
					Stream.Read(Data, 0, Data.Length);
					Stream.Position = OldPosition;
				}
				else
				{
				}
                return Data;
				*/
				var MemoryStream = new MemoryStream();
				Stream.CopyTo(MemoryStream);
				return MemoryStream.ToArray();
			}
		}

		static public Stream ReadStream(this Stream Stream, long ToRead)
		{
			var ReadedStream = SliceStream.CreateWithLength(Stream, Stream.Position, ToRead);
			Stream.Skip(ToRead);
			return ReadedStream;
		}

        static public byte[] ReadBytes(this Stream Stream, int ToRead)
        {
            var Buffer = new byte[ToRead];
            var Readed = Stream.Read(Buffer, 0, ToRead);
            if (Readed != ToRead) throw(new Exception("Unable to read " + ToRead + " bytes, readed " + Readed + "."));
            return Buffer;
        }

        static public void WriteBytes(this Stream Stream, byte[] Bytes)
        {
            Stream.Write(Bytes, 0, Bytes.Length);
        }

		static public String ReadString(this Stream Stream, int ToRead, Encoding Encoding = null)
		{
			return Stream.ReadBytes(ToRead).GetString(Encoding);
		}

        static public String ReadStringz(this Stream Stream, int ToRead = -1, Encoding Encoding = null)
        {
            if (Encoding == null) Encoding = Encoding.ASCII;
            if (ToRead == -1)
            {
                var Temp = new MemoryStream();
                byte Byte;
                while ((Byte = (byte)Stream.ReadByte()) != 0)
                {
                    Temp.WriteByte(Byte);
                }
                return Encoding.GetString(Temp.ToArray());
            }
            else
            {
                return Encoding.GetString(Stream.ReadBytes(ToRead)).TrimEnd('\0');
            }
        }

        static public void WriteStringz(this Stream Stream, String Value, int ToWrite = -1, Encoding Encoding = null)
        {
            if (Encoding == null) Encoding = Encoding.ASCII;
            if (ToWrite == -1)
            {
                Stream.WriteBytes(Value.GetStringzBytes(Encoding));
            }
            else
            {
                byte[] Bytes = Encoding.GetBytes(Value);
                if (Bytes.Length > ToWrite) throw(new Exception("String too long"));
                Stream.WriteBytes(Bytes);
                Stream.WriteZeroBytes(ToWrite - Bytes.Length);
            }
        }

        static public void WriteZeroBytes(this Stream Stream, int Count)
        {
            if (Count < 0)
            {
                Console.Error.WriteLine("Can't Write Negative Zero Bytes '" + Count + "'.");
                //throw (new Exception("Can't Write Negative Zero Bytes '" + Count + "'."));
            }
            else if (Count > 0)
            {
                var Bytes = new byte[Count];
                Stream.WriteBytes(Bytes);
            }
        }

        static public void WriteZeroToAlign(this Stream Stream, int Align)
        {
            Stream.WriteZeroBytes((int)(MathUtils.Align(Stream.Position, Align) - Stream.Position));
        }

        static public void WriteZeroToOffset(this Stream Stream, long Offset)
        {
            Stream.WriteZeroBytes((int)(Offset - Stream.Position));
        }

        public static T ReadStruct<T>(this Stream Stream) where T : struct
        {
            var Size = Marshal.SizeOf(typeof(T));
            var Buffer = new byte[Size];
            Stream.Read(Buffer, 0, Size);
            return StructUtils.BytesToStruct<T>(Buffer);
        }

        public static void WriteStruct<T>(this Stream Stream, T Struct) where T : struct
        {
            byte[] Bytes = StructUtils.StructToBytes(Struct);
            Stream.Write(Bytes, 0, Bytes.Length);
        }

        public static void Align(this Stream Stream, int Align)
        {
            Stream.Position = MathUtils.Align(Stream.Position, Align);
        }

		public static void Skip(this Stream Stream, long Count)
		{
			Stream.Seek(Count, SeekOrigin.Current);
		}

#if false
		static ThreadLocal<byte[]> PerThreadBuffer = new ThreadLocal<byte[]>(() =>
		{
			return new byte[2 * 1024 * 1024];
		});

		public static void CopyToFast(this Stream FromStream, Stream ToStream)
		{
			//var SliceFromStream = new SliceStream(FromStream);
			var SliceFromStream = FromStream;
			while (true)
			{
				int ReadedBytesCount = SliceFromStream.Read(PerThreadBuffer.Value, 0, PerThreadBuffer.Value.Length);
				Console.WriteLine(ReadedBytesCount);
				ToStream.Write(PerThreadBuffer.Value, 0, ReadedBytesCount);
				if (ReadedBytesCount < PerThreadBuffer.Value.Length) break;
			}
			//SliceFromStream.Dispose();
		}
#else
        public static void CopyToFast(this Stream FromStream, Stream ToStream)
        {
            /// TODO: Create a buffer and reuse it once for each thread.
            FromStream.CopyTo(ToStream, 2 * 1024 * 1024);
        }
#endif

        public static void CopyToFile(this Stream Stream, String FileName)
        {
            using (var OutputFile = File.OpenWrite(FileName))
            {
                Stream.CopyToFast(OutputFile);
            }
        }

        public static Stream SetPosition(this Stream Stream, long Position)
        {
            Stream.Position = Position;
            return Stream;
        }
	}
}
