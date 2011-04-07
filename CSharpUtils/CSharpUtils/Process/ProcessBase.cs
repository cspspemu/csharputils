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

		public ProcessBase()
		{
			//Console.WriteLine(this + " : " + parent);

			Impl = new ProcessBaseImplFibers();
			//Impl = new ProcessBaseImplThreads(this);
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
