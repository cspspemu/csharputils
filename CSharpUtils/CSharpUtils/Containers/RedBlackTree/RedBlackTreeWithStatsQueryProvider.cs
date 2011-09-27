using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpUtils.Containers.RedBlackTree
{
	public class RedBlackTreeWithStatsQueryProvider : IQueryProvider
	{

		public IQueryable<TElement> CreateQuery<TElement>(System.Linq.Expressions.Expression expression)
		{
			throw new NotImplementedException();
		}

		public IQueryable CreateQuery(System.Linq.Expressions.Expression expression)
		{
			throw new NotImplementedException();
		}

		public TResult Execute<TResult>(System.Linq.Expressions.Expression expression)
		{
			throw new NotImplementedException();
		}

		public object Execute(System.Linq.Expressions.Expression expression)
		{
			throw new NotImplementedException();
		}
	}
}
