using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using CSharpUtils.Process.Impl;

namespace CSharpUtils.Process
{
	abstract public class ProcessBaseCore
	{
		IProcessBaseImpl Impl;
		private bool Removed;

		/*static {
			OperatingSystem os = Environment.OSVersion;
			PlatformID pid = os.Platform;
		}*/

		static Func<IProcessBaseImpl> ProcessBaseImplFactory;

		static ProcessBaseCore()
		{
			OperatingSystem os = Environment.OSVersion;
			PlatformID pid = os.Platform;
			//Console.WriteLine(pid);
			switch (pid)
			{
				/*
				case PlatformID.Unix:
				case PlatformID.MacOSX:
					//GetType
					ProcessBaseImplFactory = () => { return new ProcessBaseImplUcontext(); };
				break;
				case PlatformID.Xbox:
				break;
				*/
				case PlatformID.Win32Windows:
				case PlatformID.Win32S:
				case PlatformID.Win32NT:
					ProcessBaseImplFactory = () => { return new ProcessBaseImplFibers(); };
				break;
				default:
					ProcessBaseImplFactory = () => { return new ProcessBaseImplThreads(); };
				break;
			}
		}

		public ProcessBaseCore()
		{
			//Console.WriteLine(this + " : " + parent);

			Impl = ProcessBaseImplFactory();
			State = State.Started;

			Impl.Init(() =>
			{
				Action method = Main;

			restart:
				try
				{
					State = State.Running;
					method();
					State = State.Ended;
					Yield();
				}
				catch (ChangeExecutionMethodException ChangeExecutionMethodException)
				{
					method = ChangeExecutionMethodException.method;
					goto restart;
				}
			});
		}

		class ChangeExecutionMethodException : Exception
		{
			internal Action method;

			public ChangeExecutionMethodException(Action method)
			{
				this.method = method; ;
			}
		}

		public void SetAction(Action method)
		{
			throw(new ChangeExecutionMethodException(method));
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

		abstract protected void Main();
	}
}
