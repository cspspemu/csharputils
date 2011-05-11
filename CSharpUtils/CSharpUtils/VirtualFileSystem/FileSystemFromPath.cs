using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpUtils.VirtualFileSystem
{
	public class FileSystemFromPath : ProxyFileSystem
	{
		public String AccessPath;
		public bool AllowAccessingParent;

		public FileSystemFromPath(FileSystem ParentFileSystem, String AccessPath, bool AllowAccessingParent = false)
			: base(ParentFileSystem)
		{
			this.AccessPath = AccessPath;
			this.AllowAccessingParent = AllowAccessingParent;
		}

		override protected void Access(String Path, out FileSystem NewFileSystem, out String NewPath)
		{
			if (!AllowAccessingParent)
			{
				Path = AbsoluteNormalizePath(Path);
			}
			base.Access(CombinePath(AccessPath, Path), out NewFileSystem, out NewPath);
		}

		protected override FileSystemEntry FilterFileSystemEntry(FileSystemEntry FileSystemEntry)
		{
			var NewFileSystemEntry = FileSystemEntry.Clone();
			{
				NewFileSystemEntry.Path = NewFileSystemEntry.Path.Substring(AccessPath.Length).PadLeft('/');
			}
			return NewFileSystemEntry;
		}

		public override String Title
		{
			get
			{
				return ParentFileSystem.Title.TrimEnd('/') + "/" + AccessPath.TrimStart('/');
			}
		}

	}
}
