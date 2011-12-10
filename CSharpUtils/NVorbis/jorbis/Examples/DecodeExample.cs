using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CSPspEmu.Hle.Formats.audio;
using NVorbis.jogg;

namespace NVorbis.jorbis.Examples
{
	// Takes a vorbis bitstream from stdin and writes raw stereo PCM to
	// stdout.  Decodes simple and chained OggVorbis files from beginning
	// to end.  Vorbisfile.a is somewhat more complex than the code below.

	class DecodeExample
	{
		static int convsize = 4096 * 2;
		static byte[] convbuffer = new byte[convsize]; // take 8k out of the data segment, not the stack

		public static void main(String[] arg)
		{
			//Stream output = File.OpenWrite("output.bin");
			var OutputBuffer = new MemoryStream();
			Stream input = null;
			//java.io.InputStream input=System.In;
			if (arg.Length > 0)
			{
				try{
					input = File.OpenRead(arg[0]);
				} catch (Exception e) {
					Console.Error.WriteLine(e);
				}
			} else {
				throw(new NotImplementedException("aaaa"));
			}

			if (input == null) throw(new NotImplementedException("bbb"));

			var SyncState = new SyncState(); // sync and verify incoming physical bitstream
			var StreamState = new StreamState(); // take physical pages, weld into a logical stream of packets
			var Page = new Page(); // one Ogg bitstream page.  Vorbis packets are inside
			var Packet = new Packet(); // one raw packet of data for decode

			var Info = new Info(); // struct that stores all the static vorbis bitstream settings
			var Comment = new Comment(); // struct that stores all the bitstream user comments
			var DspState = new DspState(); // central working state for the packet->PCM decoder
			var Block=new Block(DspState); // local working space for packet->PCM decode

			byte[] buffer;
			int bytes=0;

			// Decode setup

			SyncState.init(); // Now we can read pages

			// we repeat if the bitstream is chained
    while(true)
	{
      int eos=0;

      // grab some data at the head of the stream.  We want the first page
      // (which is guaranteed to be small and only contain the Vorbis
      // stream initial header) We need the first page to get the stream
      // serialno.

      // submit a 4k block to libvorbis' Ogg layer
      int index=SyncState.buffer(4096);
      buffer=SyncState.data;
      try{
        bytes=input.Read(buffer, index, 4096);
      }
      catch(Exception e){
        Console.Error.WriteLine(e);
        //Environment.Exit(-1);
		return;
      }
      SyncState.wrote(bytes);

      // Get the first page.
	int _result = SyncState.pageout(Page);
	if (_result != 1)
	  {
        // have we simply run out of data?  If so, we're done.
        if (bytes < 4096) break;

		Console.WriteLine(bytes + "; " + _result);
		//File.WriteAllBytes();
        // error case.  Must not be Vorbis data
        Console.Error.WriteLine("Input does not appear to be an Ogg bitstream.");
        //Environment.Exit(1);
		return;
      }

      // Get the serial number and set up the rest of decode.
      // serialno first; use it to set up a logical stream
      StreamState.init(Page.serialno());

      // extract the initial header from the first page and verify that the
      // Ogg bitstream is in fact Vorbis data

      // I handle the initial header first instead of just having the code
      // read all three Vorbis headers at once because reading the initial
      // header is an easy way to identify a Vorbis bitstream and it's
      // useful to see that functionality seperated out.

      Info.init();
      Comment.init();
      if(StreamState.pagein(Page)<0){
        // error; stream version mismatch perhaps
        Console.Error.WriteLine("Error reading first page of Ogg bitstream data.");
        Environment.Exit(1);
      }

      if(StreamState.packetout(Packet)!=1){
        // no page? must not be vorbis
        Console.Error.WriteLine("Error reading initial header packet.");
        Environment.Exit(1);
      }

      if(Info.synthesis_headerin(Comment, Packet)<0){
        // error case; not a vorbis header
        Console.Error.WriteLine("This Ogg bitstream does not contain Vorbis audio data.");
        Environment.Exit(1);
      }

      // At this point, we're sure we're Vorbis.  We've set up the logical
      // (Ogg) bitstream decoder.  Get the comment and codebook headers and
      // set up the Vorbis decoder

      // The next two packets in order are the comment and codebook headers.
      // They're likely large and may span multiple pages.  Thus we reead
      // and submit data until we get our two pacakets, watching that no
      // pages are missing.  If a page is missing, error out; losing a
      // header page is the only place where missing data is fatal. */

      int i=0;
      while(i<2){
        while(i<2){

          int result=SyncState.pageout(Page);
          if(result==0)
            break; // Need more data
          // Don't complain about missing or corrupt data yet.  We'll
          // catch it at the packet output phase

          if(result==1){
            StreamState.pagein(Page); // we can ignore any errors here
            // as they'll also become apparent
            // at packetout
            while(i<2){
              result=StreamState.packetout(Packet);
              if(result==0)
                break;
              if(result==-1){
                // Uh oh; data at some point was corrupted or missing!
                // We can't tolerate that in a header.  Die.
                Console.Error.WriteLine("Corrupt secondary header.  Exiting.");
                Environment.Exit(1);
              }
              Info.synthesis_headerin(Comment, Packet);
              i++;
            }
          }
        }
        // no harm in not checking before adding more
        index=SyncState.buffer(4096);
        buffer=SyncState.data;
        try{
          bytes=input.Read(buffer, index, 4096);
        }
        catch(Exception e){
          Console.Error.WriteLine(e);
			Environment.Exit(1);
        }
        if(bytes==0&&i<2){
          Console.Error.WriteLine("End of file before finding all Vorbis headers!");
			Environment.Exit(1);
        }
        SyncState.wrote(bytes);
      }

      // Throw the comments plus a few lines about the bitstream we're
      // decoding
      {
        byte[][] ptr=Comment.user_comments;
        for(int j=0; j<ptr.Length; j++){
          if(ptr[j]==null)
            break;
		  Console.Error.WriteLine(Util.InternalEncoding.GetString(ptr[j], 0, ptr[j].Length - 1));
        }
        Console.Error.WriteLine("\nBitstream is "+Info.channels+" channel, "+Info.rate
            +"Hz");
        Console.Error.WriteLine("Encoded by: "
			+ Util.InternalEncoding.GetString(Comment.vendor, 0, Comment.vendor.Length - 1) + "\n");
      }

      convsize=4096/Info.channels;

      // OK, got and parsed all three headers. Initialize the Vorbis
      //  packet->PCM decoder.
      DspState.synthesis_init(Info); // central decode state
      Block.init(DspState); // local state for most of the decode
      // so multiple block decodes can
      // proceed in parallel.  We could init
      // multiple vorbis_block structures
      // for vd here

      float[][][] _pcm=new float[1][][];
      int[] _index=new int[Info.channels];
      // The rest is just a straight decode loop until end of stream
      while(eos==0){
        while(eos==0){

          int result=SyncState.pageout(Page);
          if(result==0)
            break; // need more data
          if(result==-1){ // missing or corrupt data at this page position
            Console.Error.WriteLine("Corrupt or missing data in bitstream; continuing...");
          }
          else{
            StreamState.pagein(Page); // can safely ignore errors at
            // this point
            while(true){
              result=StreamState.packetout(Packet);

              if(result==0)
                break; // need more data
              if(result==-1){ // missing or corrupt data at this page position
                // no reason to complain; already complained above
              }
              else{
                // we have a packet.  Decode it
                int samples;
                if(Block.synthesis(Packet)==0){ // test for success!
                  DspState.synthesis_blockin(Block);
                }

                // **pcm is a multichannel float vector.  In stereo, for
                // example, pcm[0] is left, and pcm[1] is right.  samples is
                // the size of each channel.  Convert the float values
                // (-1.<=range<=1.) to whatever PCM format and write it out

                while((samples=DspState.synthesis_pcmout(_pcm, _index))>0){
                  float[][] pcm=_pcm[0];
                  int bout=(samples<convsize ? samples : convsize);

                  // convert floats to 16 bit signed ints (host order) and
                  // interleave
                  for(i=0; i<Info.channels; i++){
                    int ptr=i*2;
                    //int ptr=i;
                    int mono=_index[i];
                    for(int j=0; j<bout; j++){
                      int val=(int)(pcm[i][mono+j]*32767.0);
                      //		      short val=(short)(pcm[i][mono+j]*32767.);
                      //		      int val=(int)Math.round(pcm[i][mono+j]*32767.);
                      // might as well guard against clipping
                      if(val>32767){
                        val=32767;
                      }
                      if(val<-32768){
                        val=-32768;
                      }
                      if(val<0)
                        val=val|0x8000;
                      convbuffer[ptr]=(byte)(val);
                      convbuffer[ptr+1]=(byte)(((uint)val)>>8);
                      ptr+=2*(Info.channels);
                    }
                  }

//                  System.out.write(convbuffer, 0, 2*vi.channels*bout);
					//throw(new NotImplementedException("ccccccccc"));
					OutputBuffer.Write(convbuffer, 0, 2 * Info.channels * bout);

                  // tell libvorbis how
                  // many samples we
                  // actually consumed
                  DspState.synthesis_read(bout);
                }
              }
            }
            if(Page.eos()!=0)
              eos=1;
          }
        }
        if(eos==0){
          index=SyncState.buffer(4096);
          buffer=SyncState.data;
          try{
            bytes=input.Read(buffer, index, 4096);
          }
          catch(Exception e){
            Console.Error.WriteLine(e);
            Environment.Exit(1);
          }
          SyncState.wrote(bytes);
          if(bytes==0)
            eos=1;
        }
      }

      // clean up this logical bitstream; before exit we see if we're
      // followed by another [chained]

      StreamState.clear();

      // ogg_page and ogg_packet structs always point to storage in
      // libvorbis.  They're never freed or manipulated directly

      Block.clear();
      DspState.clear();
      Info.clear(); // must be called last
    }

    // OK, clean up the framer
    SyncState.clear();

	var WaveStream = new WaveStream();
	var WaveOutputStream = File.OpenWrite("output2.wav");
	OutputBuffer.Position = 0;
	WaveStream.WriteWave(WaveOutputStream, () =>
	{
		OutputBuffer.CopyTo(WaveOutputStream);
	}, NumberOfChannels: 1, SampleRate: 44100);

    Console.Error.WriteLine("Done.");
  }
	}

}
