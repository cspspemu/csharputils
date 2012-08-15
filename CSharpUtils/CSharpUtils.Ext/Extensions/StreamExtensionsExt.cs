using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CSharpUtils.SpaceAssigner;
using CSharpUtils.Streams;

static public class StreamExtensionsExt
{
	static public MapStream ConvertSpacesToMapStream(this Stream Stream, SpaceAssigner1D.Space[] Spaces)
	{
		var MapStream = new MapStream();

		foreach (var Space in Spaces)
		{
			MapStream.Map(Space.Min, Stream.SliceWithBounds(Space.Min, Space.Max));
		}

		return MapStream;
	}
}
