/* -*-mode:java; c-basic-offset:2; indent-tabs-mode:nil -*- */
/* JOrbis
 * Copyright (C) 2000 ymnk, JCraft,Inc.
 *  
 * Written by: 2000 ymnk<ymnk@jcraft.com>
 *   
 * Many thanks to 
 *   Monty <monty@xiph.org> and 
 *   The XIPHOPHORUS Company http://www.xiph.org/ .
 * JOrbis has been based on their awesome works, Vorbis codec.
 *   
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Library General Public License
 * as published by the Free Software Foundation; either version 2 of
 * the License, or (at your option) any later version.
   
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Library General Public License for more details.
 * 
 * You should have received a copy of the GNU Library General Public
 * License along with this program; if not, write to the Free Software
 * Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NVorbis.jorbis
{
	class ChainingExample
	{
		public static void main(String[] arg){
			VorbisFile ov=null;

			try{
			  if(arg.Length>0){
				ov=new VorbisFile(arg[0]);
			  }
			  else{
				  throw(new NotImplementedException());
				//ov=new VorbisFile(Console.In, null, -1);
			  }
			}
			catch(Exception e){
			  Console.Error.WriteLine(e);
			  return;
			}

			if(ov.seekable()){
			 Console.WriteLine("Input bitstream contained "+ov.streams()
				  +" logical bitstream section(s).");
			 Console.WriteLine("Total bitstream playing time: "+ov.time_total(-1)
				  +" seconds\n");
			}
			else{
			 Console.WriteLine("Standard input was not seekable.");
			 Console.WriteLine("First logical bitstream information:\n");
			}

			for(int i=0; i<ov.streams(); i++){
			  Info vi=ov.getInfo(i);
			 Console.WriteLine("\tlogical bitstream section "+(i+1)+" information:");
			 Console.WriteLine("\t\t"+vi.rate+"Hz "+vi.channels+" channels bitrate "
				  +(ov.bitrate(i)/1000)+"kbps serial number="+ov.serialnumber(i));
			  Console.Write("\t\tcompressed length: "+ov.raw_total(i)+" bytes ");
			 Console.WriteLine(" play time: "+ov.time_total(i)+"s");
			  Comment vc=ov.getComment(i);
			 Console.WriteLine(vc);
			}
			//ov.clear();
		}
	}

}
