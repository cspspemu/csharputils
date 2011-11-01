using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpUtils.Process;

namespace CSharpUtilsDokanMount
{
	public class ProcessTest
	{
		public void ProcessToTest()
		{
			var Output = new LinkedList<string>();
			var mainProcess = new MainProcess();
			var f1 = new MyProcess1();
			var f2 = new MyProcess2();

			MyProcess.DrawedHandler OnDrawed = delegate(object sender, EventArgs e)
			{
				var de = ((DrawedEventArgs)e);
				Console.WriteLine(":" + Convert.ToString(de.n));
				Output.AddLast(Convert.ToString(de.n));
			};

			f1.Drawed += new MyProcess.DrawedHandler(OnDrawed);
			f2.Drawed += new MyProcess.DrawedHandler(OnDrawed);
			while (mainProcess.State != State.Ended)
			{
				Output.AddLast("[");
				//Output.AddLast(String.Join(",", Process.allProcesses));
				mainProcess.ExecuteTree();
				mainProcess.DrawTree(null);
				//f1._ExecuteProcess();
				//Console.WriteLine("RemoveOld[1/2]");
				ProcessBase._removeOld();
				//Console.WriteLine("RemoveOld[2/2]");
				Output.AddLast("]");
			}
			//Console.ReadKey();
		}
	}

	class MainProcess : ProcessBase
	{
		protected override void Main()
		{
			while (State == State.Running)
			{
				Yield();
			}
		}
	}

	class DrawedEventArgs : EventArgs
	{
		public int n;
		public DrawedEventArgs(int n)
		{
			this.n = n;
		}
	}

	abstract class MyProcess : ProcessBase
	{
		int n = 0;
		public delegate void DrawedHandler(object sender, EventArgs e);
		public event DrawedHandler Drawed;

		protected void increment()
		{
			while (n < +4)
			{
				n++;
				Yield();
			}
		}

		protected void decrement()
		{
			while (n > -4)
			{
				n--;
				Yield();
			}
		}

		override protected void DrawItem(object _Context)
		{
			if (Drawed != null) Drawed(this, new DrawedEventArgs(n));
			//Console.WriteLine(n);
		}

	}

	class MyProcess1 : MyProcess
	{
		override protected void Main()
		{
			increment();
			decrement();
		}
	}

	class MyProcess2 : MyProcess
	{
		override protected void Main()
		{
			decrement();
			increment();
		}
	}
}
