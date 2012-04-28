using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

static public class LinkedListExtensions
{
	static public T RemoveFirstAndGet<T>(this LinkedList<T> List)
	{
		try
		{
			return List.First.Value;
		}
		finally
		{
			List.RemoveFirst();
		}
	}

	static public T RemoveLastAndGet<T>(this LinkedList<T> List)
	{
		try
		{
			return List.Last.Value;
		}
		finally
		{
			List.RemoveLast();
		}
	}
}
