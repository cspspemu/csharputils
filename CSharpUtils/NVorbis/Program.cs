using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NVorbis.jorbis;
using NVorbis.jorbis.Examples;

namespace NVorbis
{
	public class Program
	{
		static public void Main(string[] Args)
		{
			/*
			Console.WriteLine("Hello!");
			var VorbisFile = new VorbisFile(@"C:\projects\csharp\csharputils\CSharpUtils\TestInput\match0.ogg");
			Console.WriteLine(VorbisFile.bitrate(0));
			Console.WriteLine(VorbisFile.pcm_total(0));
			Console.WriteLine(VorbisFile.time_total(0));
			Console.WriteLine(VorbisFile.getInfo(0));

			byte[] buffer = new byte[10240];

			//VorbisFile.open_seekable();
			VorbisFile.open_nonseekable();
			Console.WriteLine(VorbisFile.read(buffer, buffer.Length, 0, 2, 1, null));

			File.WriteAllBytes("temp.bin", buffer);
			
			Console.ReadKey();
			*/
			DecodeExample.main(new string[] { @"C:\projects\csharp\csharputils\CSharpUtils\TestInput\match0.ogg" });
			Console.ReadKey();
		}
	}
}
