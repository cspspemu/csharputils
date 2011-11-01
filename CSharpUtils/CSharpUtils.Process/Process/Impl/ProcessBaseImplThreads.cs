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
		static public Thread MainThread;
		Semaphore Semaphore;
		Thread CurrentThread;
		/*Object Parent;

		public ProcessBaseImplThreads(Object Parent)
		{
			this.Parent = Parent;
		}*/

		~ProcessBaseImplThreads()
		{
			Console.WriteLine("~ProcessBaseImplThreads");
			Remove();
		}

		static public void Shutdown()
		{

		}

		public void Init(RunDelegate Delegate)
		{
			//Console.WriteLine("Init(" + Parent + ")");

			if (MainThread == null)
			{
				SemaphoreGlobal = new Semaphore(1, 1);
				SemaphoreGlobal.WaitOne();

				MainThread = Thread.CurrentThread;
				//mainMutex.WaitOne();
			}

			Semaphore = new Semaphore(1, 1);
			Semaphore.WaitOne();
			CurrentThread = new Thread(delegate()
			{
				Semaphore.WaitOne();
				//currentThread.Interrupt();
				Delegate();
			});

			CurrentThread.Start();

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
			CurrentThread.Abort();
		}

		public void Yield()
		{
			//Console.WriteLine("Yield(" + Parent + ")");
			SemaphoreGlobal.Release();
			Semaphore.WaitOne();
		}
	}
}
