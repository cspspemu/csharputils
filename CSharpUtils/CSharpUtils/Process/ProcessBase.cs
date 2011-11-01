using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;
using System.Runtime.InteropServices;

namespace CSharpUtils.Process
{
	abstract public class ProcessBase : ProcessBaseCore
	{
		static protected ProcessBase CurrentExecutingProcess = null;

		public int priority = 0;
		public float x = 0, y = 0, z = 0;
		public float angle = 0, scaleX = 1, scaleY = 1;

		protected ProcessBase Parent = null;
		protected LinkedList<ProcessBase> Childs;

		static private LinkedList<ProcessBase> _AllProcesses = new LinkedList<ProcessBase>();

		static public LinkedList<ProcessBase> allProcesses
		{
			get
			{
				return new LinkedList<ProcessBase>(_AllProcesses);
			}
		}

		static public void _removeOld()
		{
			foreach (var process in allProcesses)
			{
				if (process.State == State.Ended) process._Remove();
			}
		}

		private void ExecuteTreeBefore()
		{
			foreach (var process in Childs.Where(process => process.priority < 0).OrderBy(process => process.priority)) process.ExecuteTree();
		}

		private void ExecuteTreeAfter()
		{
			foreach (var process in Childs.Where(process => process.priority >= 0).OrderBy(process => process.priority)) process.ExecuteTree();
		}

		public void ExecuteTree()
		{
			if (State == State.Ended) return;

			CurrentExecutingProcess = this;

			this.ExecuteTreeBefore();
			//Console.WriteLine("<Execute " + this + ">");
			this.SwitchTo();
			//Console.WriteLine("</Execute " + this + ">");
			this.ExecuteTreeAfter();
		}

		private void DrawTreeBefore(object _Context)
		{
			foreach (var Process in Childs.Where(process => process.z < 0).OrderBy(process => process.z)) Process.DrawTree(_Context);
		}

		private void DrawTreeAfter(object _Context)
		{
			foreach (var process in Childs.Where(process => process.z >= 0).OrderBy(process => process.z)) process.DrawTree(_Context);
		}

		virtual public void DrawTree(object _Context)
		{
			this.DrawTreeBefore(_Context);
			this.DrawItem(_Context);
			this.DrawTreeAfter(_Context);
		}

		virtual protected void DrawItem(object _Context)
		{
			//Console.WriteLine(this);
		}

		public ProcessBase() : base()
		{
			_AllProcesses.AddLast(this);
			Childs = new LinkedList<ProcessBase>();
			Parent = CurrentExecutingProcess;
			if (Parent != null)
			{
				Parent.Childs.AddLast(this);
			}
		}

		~ProcessBase()
		{
			_Remove();
		}

		override protected void _Remove()
		{
			if (Parent != null)
			{
				Parent.Childs.Remove(this);
			}
			_AllProcesses.Remove(this);
			base._Remove();
		}
	}
}
