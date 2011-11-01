using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CSharpUtils.Streams;
using CSharpUtils.Extensions;

namespace CSharpUtils.VirtualFileSystem
{
	static public class FileSystemUtils
	{
		static public bool ContainsFileName(this IEnumerable<FileSystemEntry> FileSystemEntry, String FileName)
		{
			foreach (var Item in FileSystemEntry)
			{
				if (Item.Name == FileName) return true;
			}
			return false;
		}

		static public bool ExistsFile(this FileSystem FileSystem, String Path)
		{
			try
			{
				FileSystem.GetFileInfo(Path);
				return true;
			}
			catch
			{
				return false;
			}
		}

		static public void CopyFile(this FileSystem FileSystem, String SourcePath, String DestinationPath)
		{
			using (var SourceStream = FileSystem.OpenFile(SourcePath, FileMode.Open))
			{
				using (var DestinationStream = FileSystem.OpenFile(DestinationPath, FileMode.Create))
				{
					SourceStream.CopyToFast(DestinationStream);
				}
			}
		}

		static public void WriteFile(this FileSystem FileSystem, String Path, byte[] Data)
		{
			using (var FileStream = FileSystem.OpenFile(Path, FileMode.Create))
			{
				FileStream.Write(Data, 0, Data.Length);
			}
		}

		static public byte[] ReadFile(this FileSystem FileSystem, String Path)
		{
			using (var FileStream = FileSystem.OpenFile(Path, FileMode.Open))
			{
				return FileStream.ReadAll();
			}
		}
	}
}
