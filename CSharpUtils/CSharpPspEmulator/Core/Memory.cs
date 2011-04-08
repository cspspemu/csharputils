using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpPspEmulator.Core
{
	unsafe public class Memory
	{
		/*
		fixed byte ScratchPad[0x00004000];
		fixed byte FrameBuffe[0x00200000];
		fixed byte Main      [0x02000000];
		*/

		/*
		const scratchPad  = Segment(0x00_010000, 0x00004000);
		const frameBuffer = Segment(0x04_000000, 0x00200000);
		const mainMemory  = Segment(0x08_000000, 0x02000000);
		*/

		class Segment
		{
			uint Start;
			uint End;

			public Segment(uint Start, uint Length)
			{
				this.Start = Start;
				this.End = Start + Length;
			}

			public bool IsAddressInside(uint Address)
			{
				return (Address >= this.Start && Address < this.End);
			}
		}

		/*static struct Segments {
			static public Segment ScratchPad = new Segment(0x00010000, 0x00004000);
			static public Segment FrameBuffer = new Segment(0x04000000, 0x00200000);
			static public Segment MainMemory = new Segment(0x08000000, 0x02000000);
		}*/

		unsafe byte *Address(uint Address)
		{
			//if (Segments.ScratchPad.IsAddressInside(Address)) return &ScratchPad;
			return null;
		}

		uint GetUint(uint Address)
		{
			return 0;
		}
	}
}
