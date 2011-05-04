using System;

namespace Tamir.SharpSsh.jsch
{
	public class SftpException : Exception
	{
		public int id;
		public String message;
		public SftpException (int id, String message):base() 
		{
			this.id=id;
			this.message=message;
		}
		public override String ToString()
		{
			return message;
		}
	}
}
