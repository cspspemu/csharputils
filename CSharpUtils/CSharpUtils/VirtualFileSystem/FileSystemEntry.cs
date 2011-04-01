using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CSharpUtils.VirtualFileSystem
{
	public class FileSystemEntry
	{
		public enum EntryType
		{
			Unknown   = 0,
			File      = 1,
			Directory = 2,
			Link      = 3,
		}

		public struct FileTime
		{
			public DateTimeRange CreationTime, LastAccessTime, LastWriteTime;
		}

		public FileSystem FileSystem;
		public String Path;
		public FileTime Time;
		public long Size;
		public int UserId;
		public int GroupId;
		public EntryType Type = EntryType.Unknown;

		public String FullName { get {
			return Path;
		} }

		public String Name { get {
			return Path.Substring(Path.LastIndexOf('/') + 1);
		} }

		public FileSystemEntry(FileSystem FileSystem, String Path)
		{
			this.FileSystem = FileSystem;
			this.Path = Path;
		}

		virtual public Stream Open(FileMode FileMode)
		{
			return FileSystem.OpenFile(Path, FileMode);
		}

		static public Dictionary<String, FileSystemEntry> LinkedListToDictionary(LinkedList<FileSystemEntry> Entries)
		{
			var Dictionary = new Dictionary<String, FileSystemEntry>();
			foreach (var Entry in Entries)
			{
				Dictionary.Add(Entry.Name, Entry);
			}
			return Dictionary;
		}
		

		public override string ToString()
		{
			return "FileSystemEntry(FullName=" + FullName + ", Name=" + Name + ", Size=" + Size + ", Type=" + Type + ")";
		}
	}
}
