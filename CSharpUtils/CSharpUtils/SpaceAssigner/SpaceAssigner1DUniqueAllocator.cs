using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpUtils.SpaceAssigner
{
	public class SpaceAssigner1DUniqueAllocator
	{
		SpaceAssigner1D SpaceAssigner;
		Dictionary<byte[], SpaceAssigner1D.Space> AllocatedSpaces;

		public SpaceAssigner1DUniqueAllocator(SpaceAssigner1D SpaceAssigner)
		{
			this.SpaceAssigner = SpaceAssigner;
			this.AllocatedSpaces = new Dictionary<byte[], SpaceAssigner1D.Space>();
		}

		public SpaceAssigner1D.Space AllocateUnique(byte[] data)
		{
			if (!AllocatedSpaces.ContainsKey(data))
			{
				AllocatedSpaces[data] = SpaceAssigner.Allocate(data.Length);
			}
			return AllocatedSpaces[data];
		}
	}
}
