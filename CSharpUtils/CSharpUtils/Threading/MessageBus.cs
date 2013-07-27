using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpUtils.Threading
{
	//public class MessageBusItemDone
	//{
	//	static public readonly MessageBusItemDone Done = new MessageBusItemDone(true);
	//
	//	private readonly ManualResetEvent Event;
	//
	//	internal MessageBusItemDone(bool Initial = false)
	//	{
	//		Event = new ManualResetEvent(Initial);
	//	}
	//
	//	internal void Set()
	//	{
	//		Event.Set();
	//	}
	//
	//	public void Wait()
	//	{
	//		Event.WaitOne();
	//	}
	//}

	public class MessageBus<T>
	{
		private LinkedList<T> Queue = new LinkedList<T>();
		private ManualResetEvent HasItems = new ManualResetEvent(false);

		public void AddFirst(T Item)
		{
			lock (this)
			{
				Queue.AddFirst(Item);
				HasItems.Set();
			}
		}

		public void AddLast(T Item)
		{
			lock (this)
			{
				Queue.AddLast(Item);
				HasItems.Set();
			}
		}

		public T ReadOne()
		{
			HasItems.WaitOne();
			lock (this)
			{
				var Item = Queue.First.Value;
				Queue.RemoveFirst();
				if (Queue.Count == 0) HasItems.Reset();
				return Item;
			}
		}
	}
}
