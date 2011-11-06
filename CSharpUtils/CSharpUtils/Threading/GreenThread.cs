using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CSharpUtils.Threading
{
	public class GreenThread
	{
		protected Action Action;

		protected Thread ParentThread;
		protected Thread CurrentThread;
		protected Semaphore ParentSemaphore;
		protected Semaphore ThisSemaphore;
		static protected ThreadLocal<GreenThread> ThisGreenThreadList = new ThreadLocal<GreenThread>();

		public GreenThread()
		{
		}

		~GreenThread()
		{
		}

		public void InitAndStartStopped(Action Action)
		{
			this.Action = Action;
			this.ParentThread = Thread.CurrentThread;

			Console.WriteLine("InitAndStartStopped");

			ParentSemaphore = new Semaphore(1, 1);
			ParentSemaphore.WaitOne();

			ThisSemaphore = new Semaphore(1, 1);
			ThisSemaphore.WaitOne();

			var This = this;

			this.CurrentThread = new Thread(() =>
			{
				ThisGreenThreadList.Value = This;
				ThisSemaphore.WaitOne();
				try
				{
					Action();
				}
				finally
				{
					ParentSemaphore.Release();
				}
			});

			this.CurrentThread.Start();
		}

		/// <summary>
		/// Called from the caller thread.
		/// This will give the control to the green thread.
		/// </summary>
		public void SwitchTo()
		{
			ThisSemaphore.Release();
			ParentSemaphore.WaitOne();
		}

		/// <summary>
		/// Called from the green thread.
		/// This will return the control to the caller thread.
		/// </summary>
		static public void Yield()
		{
			if (ThisGreenThreadList.IsValueCreated)
			{
				var GreenThread = ThisGreenThreadList.Value;
				GreenThread.ParentSemaphore.Release();
				GreenThread.ThisSemaphore.WaitOne();
			}
		}

		static public void StopAll()
		{
			throw(new NotImplementedException());
		}

		public void Stop()
		{
			CurrentThread.Abort();
		}
	}
}
