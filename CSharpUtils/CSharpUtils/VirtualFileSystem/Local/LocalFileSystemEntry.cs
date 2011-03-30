using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CSharpUtils.VirtualFileSystem.Local
{
	class LocalFileSystemEntry : FileSystemEntry
	{
		LocalFileSystem LocalFileSystem;
		FileSystemInfo FileSystemInfo;

		public LocalFileSystemEntry(LocalFileSystem LocalFileSystem, FileSystemInfo FileSystemInfo, FileSystemEntry Parent)
		{
			this.LocalFileSystem = LocalFileSystem;
			this.FileSystemInfo = FileSystemInfo;
			this.Parent = Parent;
		}

		override public EntryType Type
		{
			get
			{
				if ((FileSystemInfo.Attributes & FileAttributes.Directory) != 0) return EntryType.Directory;
				return EntryType.File;
			}
		}

		override public long Size
		{
			get
			{
				if (FileSystemInfo is FileInfo)
				{
					return ((FileInfo)FileSystemInfo).Length;
				}
				else
				{
					return 0;
				}
			}
		}

		override protected SortedDictionary<String, FileSystemEntry> ImplList()
		{
			var List = new SortedDictionary<String, FileSystemEntry>();
			var DirectoryInfo = new DirectoryInfo(FileSystemInfo.FullName);
			foreach (var Item in DirectoryInfo.EnumerateFileSystemInfos())
			{
				var FSItem = new LocalFileSystemEntry(this.LocalFileSystem, Item, this);
				List.Add(FSItem.Name, FSItem);
			}
			return List;
		}

		override public String Name
		{
			get
			{
				if (Parent == null) return "";
				return FileSystemInfo.Name;
			}
		}
	}
}
