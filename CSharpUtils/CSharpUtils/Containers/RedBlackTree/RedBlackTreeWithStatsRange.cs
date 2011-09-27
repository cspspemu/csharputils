using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CountType = System.Int32;

namespace CSharpUtils.Containers.RedBlackTree
{
	public partial class RedBlackTreeWithStats<Type>
	{
		public class Range : IEnumerable<Type>
		{
			internal RedBlackTreeWithStats<Type> ParentTree;
			internal Node _rbegin;
			internal Node _rend;
			internal CountType _rbeginPosition;
			internal CountType _rendPosition;
		
			public CountType GetOffsetPosition(CountType index) {
				if (_rbeginPosition == -1) {
					return ParentTree.getNodePosition(_rbegin) + index;
				}
				return _rbeginPosition + index;
			}
		
			internal Range(RedBlackTreeWithStats<Type> ParentTree, Node b, Node e, CountType rbeginPosition = -1, CountType rendPosition = -1) {
				this.ParentTree = ParentTree;
				if (b == null) b = ParentTree.locateNodeAtPosition(rbeginPosition);
				if (e == null) e = ParentTree.locateNodeAtPosition(rendPosition);
				if (b == null || e == null) {
					//b = _end.left;
					//e = _end.left;
					b = e = ParentTree._end;
					rbeginPosition = -1;
					rendPosition = -1;
				}
				_rbegin = b;
				_rend = e;
				_rbeginPosition = rbeginPosition;
				_rendPosition = rendPosition;
			}
		
			public Range Clone() {
				return new Range(ParentTree, _rbegin, _rend, _rbeginPosition, _rendPosition);
			}
		
			public Range Limit(CountType limitCount) {
				Assert(limitCount >= 0);
			
				if (_rbeginPosition != -1 && _rendPosition != -1) {
					if (_rbeginPosition + limitCount > _rendPosition) {
						limitCount = _rendPosition - _rbeginPosition; 
					}
				} else {
					// Unsecure.
				}
				return LimitUnchecked(limitCount);
			}
		
			public Range LimitUnchecked(CountType limitCount) {
				Assert(limitCount >= 0);

				return new Range(
					ParentTree,
					_rbegin, null,
					_rbeginPosition, GetOffsetPosition(limitCount)
				);
			}
		
			public Range Skip(CountType skipCount)
			{
				return SkipUnchecked(skipCount);
			}
		
			public Range SkipUnchecked(CountType skipCount)
			{
				return new Range(
					ParentTree,
					null, _rend,
					GetOffsetPosition(skipCount), _rendPosition
				);
			}

			bool IsEmpty
			{
				get {
					return _rbegin == _rend;
				}
			}

			public CountType Length
			{
				get
				{
					//writefln("Begin: %d:%s", countLesser(_begin), *_begin);
					//writefln("End: %d:%s", countLesser(_end), *_end);
					//return _begin

					if (_rbeginPosition != -1 && _rendPosition != -1)
					{
						return _rendPosition - _rbeginPosition;
					}

					return ParentTree.countLesser(_rend) - ParentTree.countLesser(_rbegin);
				}
			}

			public Range Slice()
			{
				return this.Clone();
			}

			public Range Slice(CountType start, CountType end)
			{
				return new Range(
					ParentTree,
					null,
					null,
					GetOffsetPosition(start),
					GetOffsetPosition(end)
				);
			}

			public Range Slice(CountType start)
			{
				return new Range(
					ParentTree,
					null,
					null,
					GetOffsetPosition(start),
					Length
				);
			}

			public Node this[CountType Index]
			{
				get
				{
					return ParentTree.locateNodeAtPosition(GetOffsetPosition(Index));
				}
			}

			/*
			Type front
			{
				get
				{
					return _rbegin.value;
				}
			}

			Type back
			{
				get
				{
					return _rend.prev.value;
				}
			}

			void popFront()
			{
				_rbegin = _rbegin.next;
			}
		
			void popBack()
			{
				_rend = _rend.prev;
			}
			*/

			IEnumerator<Type> IEnumerable<Type>.GetEnumerator()
			{
				for (Node current = _rbegin; current != _rend; current = current.next)
				{
					yield return current.Value;
				}
			}

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				for (Node current = _rbegin; current != _rend; current = current.next)
				{
					yield return current.Value;
				}
			}
		}
	}
}
