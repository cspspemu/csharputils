using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.IO;

namespace CSharpUtils.VirtualFileSystem
{
	public class FileSystemEntry : System.Collections.IEnumerable
	{
		public FileSystemEntry Parent;
		public FileSystemEntry Root {
			get
			{
				if (Parent == null)
				{
					return this;
				}
				else
				{
					return Parent.Root;
				}
			}
		}
		public SortedDictionary<String, FileSystemEntry> VirtualChildren = new SortedDictionary<String, FileSystemEntry>();

		public enum EntryType
		{
			Unknown,
			Directory,
			File,
		}

		virtual protected SortedDictionary<String, FileSystemEntry> ImplList()
		{
			throw(new NotImplementedException());
		}

		public SortedDictionary<String, FileSystemEntry> List()
		{
			var List = new SortedDictionary<String, FileSystemEntry>();
			foreach (var Item in ImplList()) List.Add(Item.Key, Item.Value);
			foreach (var Item in VirtualChildren) List.Add(Item.Key, Item.Value);
			return List;
		}

		virtual public EntryType Type
		{
			get
			{
				return EntryType.Unknown;
			}
		}

		virtual public long Size
		{
			get
			{
				return -1;
			}
		}

		virtual public String Name
		{
			get
			{
				return "<Unknown>";
			}
		}

		public System.Collections.IEnumerator GetEnumerator()
		{
			foreach (var Item in List()) {
				yield return Item;
			}
		}

		public FileSystemEntry this[String Path]
		{
			get
			{
				return Access(Path);
			}
		}

		// Case sensitive
		public FileSystemEntry Access(String Path, bool Create = false)
		{
			int index = Path.IndexOf("/");
			// Direct
			if (index == -1)
			{
				// Case sensitive?
				return ComponentAccess(Path, Create);
			}
			else if (index == 0)
			{
				return Root.Access(Path.Substring(1), Create);
			}
			else
			{
				return this.ComponentAccess(Path.Substring(0, index), false).Access(Path.Substring(index + 1), Create);
			}
		}

		public Stream Open(String Path)
		{
			return Access(Path, true).ImplOpen();
		}

		virtual protected Stream ImplOpen()
		{
			throw (new NotImplementedException());
		}

		protected FileSystemEntry ComponentAccess(String ChildName, bool Create = false)
		{
			switch (ChildName)
			{
				case ".": return this;
				case "..": return Parent;
				case "/": return Root;
			}
			// Case sensitive?
			return List()[ChildName];
		}

		public String FullName
		{
			get
			{
				if (Parent != null) return Parent.FullName + "/" + Name;
				return Name;
			}
		}

		public override string ToString()
		{
			return "FileSystemEntry(Name='" + FullName + "', Type=" + Type + ", Size=" + Size + ")";
		}
	}
}
