using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpUtils.VirtualFileSystem
{
	abstract public class NodeFileSystem : ImplFileSystem
	{
		public class Node
		{
			public NodeFileSystem NodeFileSystem;
			public String Name;
			public Node Parent;
			public Dictionary<String, Node> Childs = new Dictionary<String, Node>();
			public FileSystemFileStream FileSystemFileStream;
			public FileSystemEntry.FileTime Time;

			public long Size
			{
				get
				{
					return (FileSystemFileStream != null) ? FileSystemFileStream.Length : 0;
				}
			}

			public bool IsFile
			{
				get
				{
					return !IsDirectory;
				}
			}

			public bool IsDirectory
			{
				get
				{
					return FileSystemFileStream == null;
				}
			}

			protected String _FullName;
			public String FullName
			{
				get
				{
					if (_FullName == null)
					{
						_FullName = Name;
						if (Parent != null)
						{
							var ParentFullName = Parent.FullName;
							if (ParentFullName.Length > 0)
							{
								_FullName = Parent.FullName + "/" + Name;
							}
						}
					}
					return _FullName;
				}
			}
			public Node Root
			{
				get
				{
					if (Parent != null) return Parent.Root;
					return this;
				}
			}

			public Node(NodeFileSystem NodeFileSystem, Node Parent, String Name)
			{
				this.NodeFileSystem = NodeFileSystem;
				this.Parent = Parent;
				this.Name = Name;
			}


			protected Node AccessChild(String Name, bool CreateNode = false)
			{
				if (!Childs.ContainsKey(Name))
				{
					if (CreateNode)
					{
						Childs[Name] = new Node(this.NodeFileSystem, this, Name);
					}
				}
				return Childs[Name];
			}

			public Node Access(String Path, bool CreateNode = false)
			{
				int Index;
				// Has components
				if ((Index = Path.IndexOf('/')) >= 0)
				{
					return AccessChild(Path.Substring(0, Index), CreateNode).Access(Path.Substring(Index + 1), CreateNode);
				}
				// No more components
				else
				{
					return AccessChild(Path, CreateNode);
				}
			}
		}

		public Node RootNode;

		Dictionary<String, List<String>> Folders = new Dictionary<string, List<String>>();
		Dictionary<String, FileSystemFileStream> Files = new Dictionary<string, FileSystemFileStream>();

		public NodeFileSystem()
		{
			RootNode = new Node(this, null, "");
		}

		internal override IEnumerable<FileSystemEntry> ImplFindFiles(string Path)
		{
			foreach (var Child in RootNode.Access(Path).Childs.Values)
			{
				yield return new FileSystemEntry(this, Child.FullName)
				{
					ExtendedFlags = FileSystemEntry.ExtendedFlagsTypes.None,
					GroupId = 0,
					Size = Child.Size,
					Time = Child.Time,
					Type = Child.IsDirectory ? FileSystemEntry.EntryType.Directory : FileSystemEntry.EntryType.File,
				};
			}
		}

		internal override FileSystemFileStream ImplOpenFile(string FileName, System.IO.FileMode FileMode)
		{
			return RootNode.Access(FileName).FileSystemFileStream;
			//return base.ImplOpenFile(FileName, FileMode);
		}

		public override String Title
		{
			get
			{
				return String.Format("nodefs://");
			}
		}
	}
}
