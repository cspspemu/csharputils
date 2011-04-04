using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dokan;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace CSharpUtils.VirtualFileSystem
{
	public class FileSystemProxyDokanOperations : DokanOperations
	{
		FileSystem FileSystem;

		public FileSystemProxyDokanOperations(FileSystem FileSystem)
		{
			this.FileSystem = FileSystem;
		}

		void NotImplemented()
		{
			StackTrace stackTrace = new StackTrace();           // get call stack
			StackFrame[] stackFrames = stackTrace.GetFrames();  // get method calls (frames)
			Console.WriteLine("Not Implemented : " + stackFrames[1].GetMethod().Name);
		}

		public static implicit operator FileSystemProxyDokanOperations(FileSystem FileSystem)
		{
			return new FileSystemProxyDokanOperations(FileSystem);
		}

		public int CreateFile(string filename, System.IO.FileAccess access, System.IO.FileShare share, System.IO.FileMode mode, System.IO.FileOptions options, Dokan.DokanFileInfo info)
		{
			//info.Context = FileSystem.OpenFile(filename, mode);
			NotImplemented();
			return 0;
		}

		public int OpenDirectory(string filename, Dokan.DokanFileInfo info)
		{
			NotImplemented();
			return 0;
		}

		public int CreateDirectory(string filename, Dokan.DokanFileInfo info)
		{
			NotImplemented();
			return -1;
		}

		public int Cleanup(string filename, Dokan.DokanFileInfo info)
		{
			NotImplemented();
			return 0;
		}

		public int CloseFile(string filename, Dokan.DokanFileInfo info)
		{
			NotImplemented();
			//((FileSystemFileStream)info.Context).Close();
			return 0;
			//NotImplemented();
		}

		public int ReadFile(string filename, byte[] buffer, ref uint readBytes, long offset, Dokan.DokanFileInfo info)
		{
			NotImplemented();
			return -1;
		}

		public int WriteFile(string filename, byte[] buffer, ref uint writtenBytes, long offset, Dokan.DokanFileInfo info)
		{
			NotImplemented();
			return -1;
		}

		public int FlushFileBuffers(string filename, Dokan.DokanFileInfo info)
		{
			NotImplemented();
			return -1;
		}

		public int GetFileInformation(string filename, Dokan.FileInformation fileinfo, Dokan.DokanFileInfo info)
		{
			/*
			fileinfo.Length = 1000;
			fileinfo.Attributes = FileAttributes.Normal;
			fileinfo.CreationTime = DateTime.Now;
			fileinfo.LastWriteTime = DateTime.Now;
			fileinfo.LastAccessTime = DateTime.Now;
			NotImplemented();
			return 0;
			*/
			return -1;
		}

		public int FindFiles(string filename, LinkedList<FileInformation> files, Dokan.DokanFileInfo info)
		{
			Console.WriteLine("FindFiles: " + filename + ":" + info);
			//foreach (var Item in FileSystem.FindFiles(filename.Replace(@"\", "/")))
			foreach (var Item in FileSystem.FindFiles(filename))
			{
				var FileInformation = new FileInformation();
				FileInformation.Attributes = FileAttributes.Normal;
				if (Item.Type.HasFlag(FileSystemEntry.EntryType.Directory))
				{
					FileInformation.Attributes |= FileAttributes.Directory;
				}
				//FileInformation.CreationTime = Item.Time.CreationTime;
				//FileInformation.LastAccessTime = Item.Time.LastAccessTime;
				//FileInformation.LastWriteTime = Item.Time.LastWriteTime;
				FileInformation.CreationTime = DateTime.Now;
				FileInformation.LastAccessTime = DateTime.Now;
				FileInformation.LastWriteTime = DateTime.Now;
				FileInformation.FileName = Item.Name;
				FileInformation.Length = Item.Size;
				//FileInformation.Length = 10001;
				//Console.WriteLine(Item);
				files.AddLast(FileInformation);
			}
			//Thread.Sleep(400);
			return 0;
			//NotImplemented();
		}

		public int SetFileAttributes(string filename, System.IO.FileAttributes attr, Dokan.DokanFileInfo info)
		{
			NotImplemented();
			return -1;
		}

		public int SetFileTime(string filename, DateTime ctime, DateTime atime, DateTime mtime, Dokan.DokanFileInfo info)
		{
			NotImplemented();
			return -1;
		}

		public int DeleteFile(string filename, Dokan.DokanFileInfo info)
		{
			NotImplemented();
			return -1;
		}

		public int DeleteDirectory(string filename, Dokan.DokanFileInfo info)
		{
			NotImplemented();
			return -1;
		}

		public int MoveFile(string filename, string newname, bool replace, Dokan.DokanFileInfo info)
		{
			NotImplemented();
			return -1;
		}

		public int SetEndOfFile(string filename, long length, Dokan.DokanFileInfo info)
		{
			NotImplemented();
			return -1;
		}

		public int SetAllocationSize(string filename, long length, Dokan.DokanFileInfo info)
		{
			NotImplemented();
			return -1;
		}

		public int LockFile(string filename, long offset, long length, Dokan.DokanFileInfo info)
		{
			NotImplemented();
			return 0;
		}

		public int UnlockFile(string filename, long offset, long length, Dokan.DokanFileInfo info)
		{
			NotImplemented();
			return 0;
		}

		public int GetDiskFreeSpace(ref ulong freeBytesAvailable, ref ulong totalBytes, ref ulong totalFreeBytes, Dokan.DokanFileInfo info)
		{
			freeBytesAvailable = 512 * 1024 * 1024;
			totalBytes = 1024 * 1024 * 1024;
			totalFreeBytes = 512 * 1024 * 1024;
			return 0;
		}

		public int Unmount(Dokan.DokanFileInfo info)
		{
			NotImplemented();
			return 0;
		}
	}
}
