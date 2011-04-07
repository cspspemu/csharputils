using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using CSharpUtils.Process.Impl;

namespace CSharpUtils.Process
{
	abstract public class ProcessBase
	{
		IProcessBaseImpl Impl;
		private bool Removed;

		/*static {
			OperatingSystem os = Environment.OSVersion;
			PlatformID pid = os.Platform;
		}*/

		static Type ProcessBaseImplType;

		static ProcessBase()
		{
			OperatingSystem os = Environment.OSVersion;
			PlatformID pid = os.Platform;
			//Console.WriteLine(pid);
			switch (pid)
			{
				case PlatformID.Unix:
				case PlatformID.MacOSX:
					//GetType
					ProcessBaseImplType = typeof(ProcessBaseImplUcontext);
				break;
				case PlatformID.Win32Windows:
				case PlatformID.Win32S:
				case PlatformID.Win32NT:
					ProcessBaseImplType = typeof(ProcessBaseImplFibers);
				break;
				case PlatformID.Xbox:
				default:
					ProcessBaseImplType = typeof(ProcessBaseImplThreads);
				break;
			}
		}

		public ProcessBase()
		{
			//Console.WriteLine(this + " : " + parent);

			Impl = (IProcessBaseImpl)Activator.CreateInstance(ProcessBaseImplType);
			State = State.Started;

			Impl.Init(() =>
			{
				State = State.Running;
				main();
				State = State.Ended;
				Yield();
			});
		}

		public State State { private set; get; }

		public void SwitchTo()
		{
			if (State != State.Ended)
			{
				Impl.SwitchTo();
			}
		}

		virtual protected void _Remove()
		{
			if (!Removed)
			{
				Removed = true;
				State = State.Ended;
				Impl.Remove();
			}
		}

		protected void Yield(int count = 1)
		{
			for (int n = 0; n < count; n++)
			{
				Impl.Yield();
			}
		}

		abstract protected void main();
	}
}
