using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CSharpUtils.Process.Impl
{
	public class ProcessBaseImplThreads : IProcessBaseImpl
	{
		static public Semaphore SemaphoreGlobal;
		static public Thread mainThread;
		Semaphore Semaphore;
		Thread currentThread;
		/*Object Parent;

		public ProcessBaseImplThreads(Object Parent)
		{
			this.Parent = Parent;
		}*/

		public void Init(RunDelegate Delegate)
		{
			//Console.WriteLine("Init(" + Parent + ")");

			if (mainThread == null)
			{
				SemaphoreGlobal = new Semaphore(1, 1);
				SemaphoreGlobal.WaitOne();

				mainThread = Thread.CurrentThread;
				//mainMutex.WaitOne();
			}

			Semaphore = new Semaphore(1, 1);
			Semaphore.WaitOne();
			currentThread = new Thread(delegate()
			{
				Semaphore.WaitOne();
				//currentThread.Interrupt();
				Delegate();
			});

			currentThread.Start();

			//Mutex.WaitOne();
		}

		public void SwitchTo()
		{
			//Console.WriteLine("SwitchTo(" + Parent + ")");
			Semaphore.Release();
			SemaphoreGlobal.WaitOne();
		}

		public void Remove()
		{
			//Console.WriteLine("Remove(" + Parent + ")");
			currentThread.Abort();
		}

		public void Yield()
		{
			//Console.WriteLine("Yield(" + Parent + ")");
			SemaphoreGlobal.Release();
			Semaphore.WaitOne();
		}
	}
}
