using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace CSharpUtils.Streams
{
	/// <summary>
	/// 
	/// </summary>
	sealed public class MapStream : Stream
	{
		/// <summary>
		/// 
		/// </summary>
		public class StreamEntry
		{
			/// <summary>
			/// 
			/// </summary>
			readonly public long Position;

			/// <summary>
			/// 
			/// </summary>
			readonly public Stream Stream;

			/// <summary>
			/// 
			/// </summary>
			/// <param name="Position"></param>
			/// <param name="Stream"></param>
			public StreamEntry(long Position, Stream Stream)
			{
				this.Position = Position;
				this.Stream = Stream;
			}

			/// <summary>
			/// 
			/// </summary>
			public long Length { get { return Stream.Length; } }

			/// <summary>
			/// 
			/// </summary>
			/// <returns></returns>
			public override string ToString()
			{
				return String.Format("StreamEntry({0}, {1}, {2})", Position, Length, Stream);
			}
		}

		private List<StreamEntry> _StreamEntries;
		private long _Position;
		private Stream _CurrentStream;
		private long _CurrentStreamPosition;

		/// <summary>
		/// 
		/// </summary>
		public IEnumerable<StreamEntry> StreamEntries
		{
			get
			{
				return _StreamEntries.AsReadOnly();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public MapStream()
		{
			_StreamEntries = new List<StreamEntry>();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Offset"></param>
		/// <param name="Stream"></param>
		/// <returns></returns>
		public MapStream Map(long Offset, Stream Stream)
		{
			_StreamEntries.Add(new StreamEntry(Offset, Stream));
			return this;
		}

		/// <summary>
		/// Function that writtes all the mappings into another stream.
		/// Useful for patching a file or memory.
		/// </summary>
		/// <param name="TargetStream">Stream to write the mapped contents to</param>
		public void WriteSegmentsToStream(Stream TargetStream)
		{
			foreach (var StreamEntry in _StreamEntries)
			{
				var SourceSliceStream = StreamEntry.Stream.SliceWithLength(0, StreamEntry.Length);
				var TargetSliceStream = TargetStream.SliceWithLength(StreamEntry.Position, StreamEntry.Length);
				SourceSliceStream.CopyToFast(TargetSliceStream);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="TargetStream"></param>
		public void Serialize(Stream TargetStream)
		{
			// Magic
			TargetStream.WriteString("MAPS");

			// Version
			TargetStream.WriteVariableUlongBit8Extends(1);

			// ListEntryCount
			TargetStream.WriteVariableUlongBit8Extends((ulong)_StreamEntries.Count);

			//ulong FileOffset = 0;

			// EntryHeaders
			foreach (var StreamEntry in _StreamEntries)
			{
				// EntryFileOffset
				//TargetStream.WriteVariableUlongBit8Extends(FileOffset);

				// EntryMapOffset
				TargetStream.WriteVariableUlongBit8Extends((ulong)StreamEntry.Position);

				// EntryLength
				TargetStream.WriteVariableUlongBit8Extends((ulong)StreamEntry.Length);
			}

			// EntryContents
			foreach (var StreamEntry in _StreamEntries)
			{
				StreamEntry.Stream.SliceWithLength(0, StreamEntry.Length).CopyToFast(TargetStream);
			}

			TargetStream.Flush();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="TargetStream"></param>
		/// <returns></returns>
		static public MapStream Unserialize(Stream TargetStream)
		{
			if (TargetStream.ReadString(4) != "MAPS") throw(new InvalidDataException("Not a MapStream serialized stream"));
			
			var Version = TargetStream.ReadVariableUlongBit8Extends();
			var MapStream = new MapStream();

			switch (Version)
			{
				case 1:
					var EntryCount = (int)TargetStream.ReadVariableUlongBit8Extends();
					var Entries = new Tuple<ulong, ulong>[EntryCount];
					for (int n = 0; n < EntryCount; n++)
					{
						//var EntryFileOffset = TargetStream.ReadVariableUlongBit8Extends();
						var EntryMapOffset = TargetStream.ReadVariableUlongBit8Extends();
						var EntryLength = TargetStream.ReadVariableUlongBit8Extends();
						Entries[n] = new Tuple<ulong, ulong>(EntryMapOffset, EntryLength);
					}

					foreach (var Entry in Entries)
					{
						var EntryMapOffset = Entry.Item1;
						var EntryLength = Entry.Item2;
						var EntryStream = TargetStream.ReadStream((long)EntryLength);
						MapStream.Map((long)EntryMapOffset, EntryStream);
					}

					break;
				default:
					throw (new InvalidDataException("Unsupported MapStream serialized stream version " + Version));
			}

			return MapStream;
		}

		/// <summary>
		/// 
		/// </summary>
		public override bool CanRead
		{
			get { return true; }
		}

		/// <summary>
		/// 
		/// </summary>
		public override bool CanSeek
		{
			get { return true; }
		}

		/// <summary>
		/// 
		/// </summary>
		public override bool CanWrite
		{
			get { return false; }
		}

		/// <summary>
		/// 
		/// </summary>
		public override void Flush()
		{
			foreach (var StreamEntry in _StreamEntries) StreamEntry.Stream.Flush();
		}

		/// <summary>
		/// 
		/// </summary>
		public override long Length
		{
			get
			{
				return _StreamEntries.Max(StreamEntry => StreamEntry.Position + StreamEntry.Length);
			}
		}

		/// <summary>
		/// 
		/// </summary>
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

		/// <summary>
		/// 
		/// </summary>
		protected void _PrepareCurrentStream()
		{
			foreach (var StreamEntry in _StreamEntries)
			{
				if (_Position >= StreamEntry.Position && _Position < StreamEntry.Position + StreamEntry.Length)
				{
					_CurrentStream = StreamEntry.Stream;
					_CurrentStreamPosition = _Position - StreamEntry.Position;
					return;
				}
			}

			_CurrentStream = null;
			_CurrentStreamPosition = 0;

			//_Position
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected long GetAvailableBytesOnCurrentStream()
		{
			return _CurrentStream.Length - _CurrentStreamPosition;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Buffer"></param>
		/// <param name="Offset"></param>
		/// <param name="Count"></param>
		/// <returns></returns>
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

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Buffer"></param>
		/// <param name="Offset"></param>
		/// <param name="Count"></param>
		public override void Write(byte[] Buffer, int Offset, int Count)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="offset"></param>
		/// <param name="origin"></param>
		/// <returns></returns>
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

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}
	}
}
