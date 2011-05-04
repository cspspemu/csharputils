using System;
using System.IO;

namespace Tamir.SharpSsh.java.io
{
	/// <summary>
	/// Summary description for File.
	/// </summary>
	public class File
	{
		public string file;
		internal FileInfo info;

		public File(string file)
		{
			this.file = file;
			info = new FileInfo(file);
		}

		public long Length()
		{
			return info.Length;
		}
		
		public long length()
		{
			return Length();
		}
	}
}
