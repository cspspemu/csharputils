using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpUtils.SpaceAssigner
{
	// http://www.yoda.arachsys.com/csharp/genericoperators.html
	// http://www.lambda-computing.com/publications/articles/generics2/
	public class SpaceAssigner1D
	{
		public class Space
		{
			public long From;
			public long To;

			public long Length
			{
				get
				{
					return To - From;
				}
			}

			public Space(long From, long To)
			{
				this.From = From;
				this.To = To;
			}
		}

		protected LinkedList<Space> AvailableChunks;

		public SpaceAssigner1D()
		{
			AvailableChunks = new LinkedList<Space>();
		}

		public Space Intersection(Space Space)
		{
			throw(new NotImplementedException());
		}

		public void AddAvailable(Space Space)
		{
			//Space.
			throw (new NotImplementedException());
			//AvailableChunks.AddLast(Space);
		}

		/**
		 * Finds an Available Space Chunk that has a length greater or equals to
		 * the specified one.
		 */
		public Space Allocate(long Length)
		{
			throw(new NotImplementedException());
		}
	}
}
