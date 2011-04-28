using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpUtils.VirtualFileSystem
{
	abstract public partial class FileSystem : IDisposable
	{
		public struct MountStruct
		{
			public FileSystem FileSystem;
			public String Path;
		}

		internal SortedDictionary<String, MountStruct> MountedFileSystems = new SortedDictionary<string, MountStruct>();

		String CurrentWorkingPath = "";
		virtual protected bool CaseInsensitiveFileSystem { get { return false; } }

		~FileSystem()
		{
			Shutdown();
		}

		virtual public void Shutdown()
		{
		}

		static public String AbsoluteNormalizePath(String Path, String CurrentWorkingPath = "")
		{
			var Components = new LinkedList<String>();

			// Normalize slashes.
			Path = Path.Replace('\\', '/');

			// Relative Path
			if (Path.StartsWith("/"))
			{
				Path = CurrentWorkingPath + "/" + Path;
			}

			// Normalize Components
			foreach (var Component in Path.Split('/'))
			{
				switch (Component)
				{
					case "": case ".": break;
					case "..": Components.RemoveLast(); break;
					default: Components.AddLast(Component); break;
				}
			}

			return String.Join("/", Components);
		}

		private void Access(String Path, out FileSystem NewFileSystem, out String NewPath)
		{
			String ComparePath;

			// Normalize Components
			Path = AbsoluteNormalizePath(Path, CurrentWorkingPath);

			ComparePath = Path;
			if (CaseInsensitiveFileSystem)
			{
				ComparePath = ComparePath.ToLower();
			}

			// Check MountedFileSystems.
			foreach (var Item in MountedFileSystems)
			{
				var CheckMountedPath = Item.Key;
				var MountInfo = Item.Value;

				if (CaseInsensitiveFileSystem)
				{
					CheckMountedPath = CheckMountedPath.ToLower();
				}

				if (
					ComparePath.StartsWith(CheckMountedPath) &&
					(
						(CheckMountedPath.Length == ComparePath.Length) ||
						(ComparePath.Substring(CheckMountedPath.Length, 1) == "/")
					)
				) {
					// Use Mounted File System.
					MountInfo.FileSystem.Access(
						ComparePath.Substring(CheckMountedPath.Length),
						out NewFileSystem,
						out NewPath
					);
					return;
				}
			}
			NewFileSystem = this;
			NewPath = Path;
		}

		public void Mount(String Path, FileSystem FileSystemToMount, String FileSystemToMountPath = "/")
		{
			String FinalPath = AbsoluteNormalizePath(Path, CurrentWorkingPath);
			MountedFileSystems[FinalPath] = new MountStruct()
			{
				FileSystem = FileSystemToMount,
				Path = FileSystemToMountPath,
			};
		}

		public void UnMount(String Path)
		{
			String FinalPath = AbsoluteNormalizePath(Path, CurrentWorkingPath);
			MountedFileSystems.Remove(FinalPath);
		}

		public void Dispose()
		{
			Shutdown();
		}
	}
}
