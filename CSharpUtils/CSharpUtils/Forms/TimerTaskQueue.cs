using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CSharpUtils.Forms
{
	public class TimerTaskQueue
	{
		Timer Timer;
		public Queue<TaskDelegate> Tasks = new Queue<TaskDelegate>();
		public delegate void TaskDelegate();

		static protected Dictionary<String, TimerTaskQueue> NamedInstances = new Dictionary<string, TimerTaskQueue>();

		static public TimerTaskQueue GetNamedInstance(String Name, int Interval)
		{
			if (!NamedInstances.ContainsKey(Name))
			{
				NamedInstances[Name] = new TimerTaskQueue(Interval);
			}
			return NamedInstances[Name];
		}

		public TimerTaskQueue(int Interval)
		{
			Timer = new Timer();
			Timer.Interval = Interval;
			Timer.Tick += new EventHandler(delegate(object sender, EventArgs e)
			{
				ExecuteOne();
			});
			Timer.Start();
		}

		protected void ExecuteOne()
		{
			var TaskDelegate = Tasks.Dequeue();
			TaskDelegate();
			if (Tasks.Count == 0)
			{
				Timer.Stop();
			}
		}

		public void AddQueue(TaskDelegate TaskDelegate)
		{
			Tasks.Enqueue(TaskDelegate);
			Timer.Start();
		}
	}
}
