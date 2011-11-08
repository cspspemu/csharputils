using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

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

		static public Thread MonitorThread;

		public GreenThread()
		{
			/*
			if (MonitorThread == null)
			{
				MonitorThread = new Thread(() => {
					//Thread.CurrentThread.IsAlive
					var Process = System.Diagnostics.Process.GetCurrentProcess();
					while (true)
					{
						foreach (var _ProcessThread in Process.Threads)
						{
							var ProcessThread = _ProcessThread as ProcessThread;

							Console.WriteLine(ProcessThread.);
							//Thread.ThreadState
						}
						//Thread
						Thread.Sleep(200);
					}
				});
				MonitorThread.Start();
			}
			*/
		}

		~GreenThread()
		{
		}

		void ThisSemaphoreWaitOrParentThreadStopped()
		{
			while (true)
			{
				// If the parent thread have been stopped. We should not wait any longer.
				if (!ParentThread.IsAlive) Thread.CurrentThread.Abort();

				if (ThisSemaphore.WaitOne(20))
				{
					// Signaled.
					break;
				}
			}
		}

		public void InitAndStartStopped(Action Action)
		{
			this.Action = Action;
			this.ParentThread = Thread.CurrentThread;

			ParentSemaphore = new Semaphore(1, 1);
			ParentSemaphore.WaitOne();

			ThisSemaphore = new Semaphore(1, 1);
			ThisSemaphore.WaitOne();

			var This = this;

			this.CurrentThread = new Thread(() =>
			{
				ThisGreenThreadList.Value = This;
				ThisSemaphoreWaitOrParentThreadStopped();
				try
				{
					Action();
				}
				finally
				{
					ParentSemaphore.Release();
				}
			});

			this.CurrentThread.Name = "GreenThread";

			this.CurrentThread.Start();
		}

		/// <summary>
		/// Called from the caller thread.
		/// This will give the control to the green thread.
		/// </summary>
		public void SwitchTo()
		{
			ParentThread = Thread.CurrentThread;
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
				GreenThread.ThisSemaphoreWaitOrParentThreadStopped();
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
