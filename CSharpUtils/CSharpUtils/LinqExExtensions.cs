using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpUtils
{
	static public class LinqExExtensions
	{
		static public bool ContainsSubset<TSource>(this IEnumerable<TSource> Superset, IEnumerable<TSource> Subset)
		{
			return !Subset.Except(Superset).Any();
		}
	}
}
