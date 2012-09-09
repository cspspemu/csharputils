using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CSharpUtils;

public static class LinqExExtensions
{
	public static bool ContainsSubset<TSource>(this IEnumerable<TSource> Superset, IEnumerable<TSource> Subset)
	{
		return !Subset.Except(Superset).Any();
	}

	/// <summary>
	/// See ToDictionary.
	/// </summary>
	/// <typeparam name="TValue"></typeparam>
	/// <typeparam name="TKey"></typeparam>
	/// <param name="ListItems"></param>
	/// <param name="KeySelector"></param>
	/// <returns></returns>
	public static Dictionary<TKey, TValue> CreateDictionary<TValue, TKey>(this IEnumerable<TValue> ListItems, Func<TValue, TKey> KeySelector)
	{
		var Dictionary = new Dictionary<TKey, TValue>();
		foreach (var Item in ListItems) Dictionary.Add(KeySelector(Item), Item);
		return Dictionary;
	}

	public static IEnumerable<TSource> OrderByNatural<TSource, TString>(this IEnumerable<TSource> Items, Func<TSource, TString> selector)
	{
		Func<string, object> convert = str =>
		{
			int result;
			if (int.TryParse(str, out result))
			{
				return result;
			}
			else {
				return str;
			}
		};

		return Items.OrderBy(
			Item => Regex.Split(selector(Item).ToString().Replace(" ", ""), "([0-9]+)").Select(convert),
			new EnumerableComparer<object>()
		);
	}

	public static IEnumerable<TSource> OrderByNatural<TSource>(this IEnumerable<TSource> Items)
	{
		return Items.OrderByNatural(Value => Value);
	}

	public static IEnumerable<TSource> DistinctByKey<TSource, TResult>(this IEnumerable<TSource> Items, Func<TSource, TResult> Selector)
	{
		return Items.Distinct(new LinqEqualityComparer<TSource, TResult>(Selector));
	}

	public static String ToHexString(this IEnumerable<byte> Bytes)
	{
		return String.Join("", Bytes.Select(Byte => Byte.ToString("x2")));
	}

	public static String Implode<TSource>(this IEnumerable<TSource> Items, String Separator)
	{
		return String.Join(Separator, Items.Select(Item => Item.ToString()));
	}

	public static String ToStringArray<TSource>(this IEnumerable<TSource> Items, String Separator = ",")
	{
		return Items.Implode(Separator);
	}

	public static void ForEach<T>(this IEnumerable<T> Items, Action<int, T> action)
	{
		int index = 0;
		foreach (var Item in Items) action(index++, Item);
	}

	public static void ForEach<T>(this IEnumerable<T> Items, Action<T> action)
	{
		foreach (var Item in Items) action(Item);
	}

	public static T[] ToArray2<T>(this IEnumerable<T> Items)
	{
		List<T> ListItems = new List<T>();
		foreach (var Item in Items) ListItems.Add(Item);
		return ListItems.ToArray();
	}

	/*
	public static T ProcessNewObject<T>(T Object, Action<T> Callback)
	{
		Callback(Object);
		return Object;
	}
	*/
}
