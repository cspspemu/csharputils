using System.IO;
using CSharpUtils.Streams;

namespace CSharpUtils.SpaceAssigner
{
	public class SpaceAssigner1DUniqueAllocatorStream : SpaceAssigner1DUniqueAllocator
	{
		public Stream Stream;

		public SpaceAssigner1DUniqueAllocatorStream(SpaceAssigner1D SpaceAssigner, Stream Stream)
			: base(SpaceAssigner)
		{
			this.Stream = Stream;
			OnAllocate += SpaceAssigner1DUniqueAllocatorStream_OnAllocate;
		}

		void SpaceAssigner1DUniqueAllocatorStream_OnAllocate(byte[] Bytes, SpaceAssigner1D.Space Space)
		{
			SliceStream.CreateWithBounds(this.Stream, Space.Min, Space.Max).WriteBytes(Bytes);
		}
	}
}
