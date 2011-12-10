using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NVorbis.jorbis
{
	abstract class FuncMapping
	{
		public static FuncMapping[] mapping_P = { new Mapping0() };

		abstract void pack(Info info, Object imap, Buffer buffer);

		abstract Object unpack(Info info, Buffer buffer);

		abstract Object look(DspState vd, InfoMode vm, Object m);

		abstract void free_info(Object imap);

		abstract void free_look(Object imap);

		abstract int inverse(Block vd, Object lm);
	}

}
