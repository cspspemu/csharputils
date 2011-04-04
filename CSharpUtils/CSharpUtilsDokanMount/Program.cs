using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpUtils.VirtualFileSystem;
using CSharpUtils.VirtualFileSystem.Local;
using Dokan;
using System.Threading;
using CSharpUtils.VirtualFileSystem.Ssh;

namespace CSharpUtilsDokanMount
{
	class Program
	{
		static void Main(string[] args)
		{
			DokanOptions opt = new DokanOptions();
			var FileSystem = new LocalFileSystem(@"C:\temp\temp2");
			var FileSystem2 = new SftpFileSystem("192.168.1.36", 22, "ubuntu", "ubuntu", 1000);
			FileSystem.Mount("/MountedFolder/sftp", FileSystem2);
			opt.MountPoint = @"m:\";
			opt.DebugMode = true;
			opt.UseStdErr = true;
			opt.NetworkDrive = true;
			opt.RemovableDrive = false;
			opt.VolumeLabel = "Test";

			Thread.GetDomain().ProcessExit += new EventHandler(delegate(object sender, EventArgs e)
			{
				DokanNet.DokanUnmount('m');
			});

			int status = DokanNet.DokanMain(opt, new FileSystemProxyDokanOperations(FileSystem));
			switch (status)
			{
				case DokanNet.DOKAN_DRIVE_LETTER_ERROR:
					Console.WriteLine("Drvie letter error");
					break;
				case DokanNet.DOKAN_DRIVER_INSTALL_ERROR:
					Console.WriteLine("Driver install error");
					break;
				case DokanNet.DOKAN_MOUNT_ERROR:
					Console.WriteLine("Mount error");
					break;
				case DokanNet.DOKAN_START_ERROR:
					Console.WriteLine("Start error");
					break;
				case DokanNet.DOKAN_ERROR:
					Console.WriteLine("Unknown error");
					break;
				case DokanNet.DOKAN_SUCCESS:
					Console.WriteLine("Success");
					break;
				default:
					Console.WriteLine("Unknown status: %d", status);
					break;
			}
			Console.ReadKey();
		}
	}
}
