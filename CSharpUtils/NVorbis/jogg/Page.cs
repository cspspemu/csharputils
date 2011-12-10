using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NVorbis.jogg
{
	public class Page{
		private static uint[] crc_lookup = new uint[256];

		static Page()
		{
			for (uint i = 0; i < crc_lookup.Length; i++)
			{
				crc_lookup[i] = crc_entry(i);
			}
		}

	  private static uint crc_entry(uint index)
	  {
		uint r=index<<24;
		for(int i=0; i<8; i++){
		  if((r&0x80000000)!=0){
			r=(r<<1)^0x04c11db7; /* The same as the ethernet generator
               					  polynomial, although we use an
               				  unreflected alg and an init/final
               				  of 0, not 0xffffffff */
		  }
		  else{
			r<<=1;
		  }
		}
		return r;
	  }

	  public byte[] header_base;
	  public int header;
	  public int header_len;
	  public byte[] body_base;
	  public int body;
	  public int body_len;

	  internal int version(){
		return header_base[header+4]&0xff;
	  }

	  internal int continued()
	  {
		return (header_base[header+5]&0x01);
	  }

	  public int bos(){
		return (header_base[header+5]&0x02);
	  }

	  public int eos(){
		return (header_base[header+5]&0x04);
	  }

	  public long granulepos(){
		long foo=header_base[header+13]&0xff;
		foo=(foo<<8)|(header_base[header+12]&0xff);
		foo=(foo<<8)|(header_base[header+11]&0xff);
		foo=(foo<<8)|(header_base[header+10]&0xff);
		foo=(foo<<8)|(header_base[header+9]&0xff);
		foo=(foo<<8)|(header_base[header+8]&0xff);
		foo=(foo<<8)|(header_base[header+7]&0xff);
		foo=(foo<<8)|(header_base[header+6]&0xff);
		return (foo);
	  }

	  public int serialno(){
		return (header_base[header+14]&0xff)|((header_base[header+15]&0xff)<<8)
			|((header_base[header+16]&0xff)<<16)
			|((header_base[header+17]&0xff)<<24);
	  }

	  internal int pageno(){
		return (header_base[header+18]&0xff)|((header_base[header+19]&0xff)<<8)
			|((header_base[header+20]&0xff)<<16)
			|((header_base[header+21]&0xff)<<24);
	  }

	  internal void checksum()
	  {
		uint crc_reg=0;

		for(int i=0; i<header_len; i++){
		  crc_reg=(crc_reg<<8)
			  ^crc_lookup[((crc_reg>>24)&0xff)^(header_base[header+i]&0xff)];
		}
		for(int i=0; i<body_len; i++){
		  crc_reg=(crc_reg<<8)
			  ^crc_lookup[((crc_reg>>24)&0xff)^(body_base[body+i]&0xff)];
		}
		header_base[header+22]=(byte)(crc_reg>>0);
		header_base[header+23]=(byte)(crc_reg>>8);
		header_base[header+24]=(byte)(crc_reg>>16);
		header_base[header+25]=(byte)(crc_reg>>24);
	  }

	  public Page copy(){
		return copy(new Page());
	  }

	  public Page copy(Page p){
		byte[] tmp=new byte[header_len];
		Array.Copy(header_base, header, tmp, 0, header_len);
		p.header_len=header_len;
		p.header_base=tmp;
		p.header=0;
		tmp=new byte[body_len];
		Array.Copy(body_base, body, tmp, 0, body_len);
		p.body_len=body_len;
		p.body_base=tmp;
		p.body=0;
		return p;
	  }

	}

}
