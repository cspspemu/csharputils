using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NVorbis.jorbis
{
	public class JOrbisException extends Exception{

	  private static final long serialVersionUID=1L;

	  public JOrbisException(){
		super();
	  }

	  public JOrbisException(String s){
		super("JOrbis: "+s);
	  }
	}

}
