using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NVorbis.jogg
{
	public class Buffer{
	  private const int BUFFER_INCREMENT=256;

	  private readonly uint[] mask= {0x00000000, 0x00000001, 0x00000003,
		  0x00000007, 0x0000000f, 0x0000001f, 0x0000003f, 0x0000007f, 0x000000ff,
		  0x000001ff, 0x000003ff, 0x000007ff, 0x00000fff, 0x00001fff, 0x00003fff,
		  0x00007fff, 0x0000ffff, 0x0001ffff, 0x0003ffff, 0x0007ffff, 0x000fffff,
		  0x001fffff, 0x003fffff, 0x007fffff, 0x00ffffff, 0x01ffffff, 0x03ffffff,
		  0x07ffffff, 0x0fffffff, 0x1fffffff, 0x3fffffff, 0x7fffffff, 0xffffffff};

	  int ptr=0;
	  byte[] _buffer=null;
	  int endbit=0;
	  int endbyte=0;
	  int storage=0;

	  public void writeinit(){
		_buffer=new byte[BUFFER_INCREMENT];
		ptr=0;
		_buffer[0]=(byte)'\0';
		storage=BUFFER_INCREMENT;
	  }

	  public void write(byte[] s){
		for(int i=0; i<s.Length; i++){
		  if(s[i]==0)
			break;
		  write(s[i], 8);
		}
	  }

	  public void read(byte[] s, int bytes){
		int i=0;
		while(bytes--!=0){
		  s[i++]=(byte)(read(8));
		}
	  }

	  void reset(){
		ptr=0;
		_buffer[0]=(byte)'\0';
		endbit=endbyte=0;
	  }

	  public void writeclear(){
		_buffer=null;
	  }

	  public void readinit(byte[] buf, int bytes){
		readinit(buf, 0, bytes);
	  }

	  public void readinit(byte[] buf, int start, int bytes){
		ptr=start;
		_buffer=buf;
		endbit=endbyte=0;
		storage=bytes;
	  }

	  public void write(int _value, int bits){
		  uint value = (uint)_value;
		if(endbyte+4>=storage){
		  byte[] foo=new byte[storage+BUFFER_INCREMENT];
		  Array.Copy(_buffer, 0, foo, 0, storage);
		  _buffer=foo;
		  storage+=BUFFER_INCREMENT;
		}

		value&=mask[bits];
		bits+=endbit;
		_buffer[ptr]|=(byte)(value<<endbit);

		if(bits>=8){
		  _buffer[ptr+1]=(byte)(value>>(8-endbit));
		  if(bits>=16){
			_buffer[ptr+2]=(byte)(value>>(16-endbit));
			if(bits>=24){
			  _buffer[ptr+3]=(byte)(value>>(24-endbit));
			  if(bits>=32){
				if(endbit>0)
				  _buffer[ptr+4]=(byte)(value>>(32-endbit));
				else
				  _buffer[ptr+4]=0;
			  }
			}
		  }
		}

		endbyte+=bits/8;
		ptr+=bits/8;
		endbit=bits&7;
	  }

	  public int look(int bits){
		int ret;
		uint m=mask[bits];

		bits+=endbit;

		if(endbyte+4>=storage){
		  if(endbyte+(bits-1)/8>=storage)
			return (-1);
		}

		ret=((_buffer[ptr])&0xff)>>endbit;
		if(bits>8){
		  ret|=((_buffer[ptr+1])&0xff)<<(8-endbit);
		  if(bits>16){
			ret|=((_buffer[ptr+2])&0xff)<<(16-endbit);
			if(bits>24){
			  ret|=((_buffer[ptr+3])&0xff)<<(24-endbit);
			  if(bits>32&&endbit!=0){
				ret|=((_buffer[ptr+4])&0xff)<<(32-endbit);
			  }
			}
		  }
		}
		return (int)(m&ret);
	  }

	  public int look1(){
		if(endbyte>=storage)
		  return (-1);
		return ((_buffer[ptr]>>endbit)&1);
	  }

	  public void adv(int bits){
		bits+=endbit;
		ptr+=bits/8;
		endbyte+=bits/8;
		endbit=bits&7;
	  }

	  public void adv1(){
		++endbit;
		if(endbit>7){
		  endbit=0;
		  ptr++;
		  endbyte++;
		}
	  }

	  public int read(int bits){
		uint ret;
		uint m=mask[bits];

		bits+=endbit;

		if(endbyte+4>=storage){
		  ret=unchecked((uint)-1);
		  if(endbyte+(bits-1)/8>=storage){
			ptr+=bits/8;
			endbyte+=bits/8;
			endbit=bits&7;
			return (int)(ret);
		  }
		}

		ret=(uint)((_buffer[ptr])&0xff)>>endbit;
		if(bits>8){
		  ret|=(uint)((_buffer[ptr+1])&0xff)<<(8-endbit);
		  if(bits>16){
			ret|=(uint)((_buffer[ptr+2])&0xff)<<(16-endbit);
			if(bits>24){
			  ret|=(uint)((_buffer[ptr+3])&0xff)<<(24-endbit);
			  if(bits>32&&endbit!=0){
				ret|=(uint)((_buffer[ptr+4])&0xff)<<(32-endbit);
			  }
			}
		  }
		}

		ret&=m;

		ptr+=bits/8;
		endbyte+=bits/8;
		endbit=bits&7;
		return (int)(ret);
	  }

	  public int readB(int bits){
		int ret;
		int m=32-bits;

		bits+=endbit;

		if(endbyte+4>=storage){
		  /* not the main path */
		  ret=-1;
		  if(endbyte*8+bits>storage*8){
			ptr+=bits/8;
			endbyte+=bits/8;
			endbit=bits&7;
			return (ret);
		  }
		}

		ret=(_buffer[ptr]&0xff)<<(24+endbit);
		if(bits>8){
		  ret|=(_buffer[ptr+1]&0xff)<<(16+endbit);
		  if(bits>16){
			ret|=(_buffer[ptr+2]&0xff)<<(8+endbit);
			if(bits>24){
			  ret|=(_buffer[ptr+3]&0xff)<<(endbit);
			  if(bits>32&&(endbit!=0))
				ret|=(_buffer[ptr+4]&0xff)>>(8-endbit);
			}
		  }
		}
		  // CHECK!
		//ret=(ret>>>(m>>1))>>>((m+1)>>1);

		ret=(int)(uint)((uint)ret>>(m>>1))>>((m+1)>>1);

		ptr+=bits/8;
		endbyte+=bits/8;
		endbit=bits&7;
		return (ret);
	  }

	  public int read1(){
		int ret;
		if(endbyte>=storage){
		  ret=-1;
		  endbit++;
		  if(endbit>7){
			endbit=0;
			ptr++;
			endbyte++;
		  }
		  return (ret);
		}

		ret=(_buffer[ptr]>>endbit)&1;

		endbit++;
		if(endbit>7){
		  endbit=0;
		  ptr++;
		  endbyte++;
		}
		return (ret);
	  }

	  public int bytes(){
		return (endbyte+(endbit+7)/8);
	  }

	  public int bits(){
		return (endbyte*8+endbit);
	  }

	  public byte[] buffer(){
		return (_buffer);
	  }

	  public static int ilog(uint v){
		int ret=0;
		while(v>0){
		  ret++;
		  v>>=1;
		}
		return (ret);
	  }

	  public static void report(String In){
		  Console.Error.WriteLine(In);
		  Environment.Exit(1);
	  }
	}

}
