using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NVorbis.jorbis
{
	abstract class FuncMapping
	{
		public static FuncMapping[] mapping_P = { new Mapping0() };
		abstract internal void pack(Info info, Object imap, NVorbis.jogg.Buffer buffer);
		abstract internal Object unpack(Info info, NVorbis.jogg.Buffer buffer);
		abstract internal Object look(DspState vd, InfoMode vm, Object m);
		abstract internal void free_info(Object imap);
		abstract internal void free_look(Object imap);
		abstract internal int inverse(Block vd, Object lm);
	}

}
