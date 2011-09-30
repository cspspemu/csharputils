using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpUtils.Extensions
{
	static public class DictionaryExtensions
	{
		static public TValue GetOrCreate<TKey, TValue>(this Dictionary<TKey, TValue> This, TKey Key, Func<TValue> Allocator)
		{
			TValue Item;
			if (!This.TryGetValue(Key, out Item))
			{
				return This[Key] = Allocator();
			}
			return Item;
		}

		static public TValue GetOrCreate<TKey, TValue>(this Dictionary<TKey, TValue> This, TKey Key) where TValue : new()
		{
			return This.GetOrCreate(Key, () => { return new TValue(); });
		}
	}
}
