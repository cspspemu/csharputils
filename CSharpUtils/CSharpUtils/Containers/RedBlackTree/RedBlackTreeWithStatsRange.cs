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
		
			public CountType getOffsetPosition(CountType index) {
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
		
			public Range clone() {
				return new Range(ParentTree, _rbegin, _rend, _rbeginPosition, _rendPosition);
			}
		
			public Range limit(CountType limitCount) {
				Assert(limitCount >= 0);
			
				if (_rbeginPosition != -1 && _rendPosition != -1) {
					if (_rbeginPosition + limitCount > _rendPosition) {
						limitCount = _rendPosition - _rbeginPosition; 
					}
				} else {
					// Unsecure.
				}
				return limitUnchecked(limitCount);
			}
		
			public Range limitUnchecked(CountType limitCount) {
				Assert(limitCount >= 0);

				return new Range(
					ParentTree,
					_rbegin, null,
					_rbeginPosition, getOffsetPosition(limitCount)
				);
			}
		
			public Range skip(CountType skipCount) {
				return skipUnchecked(skipCount);
			}
		
			public Range skipUnchecked(CountType skipCount) {
				return new Range(
					ParentTree,
					null, _rend,
					getOffsetPosition(skipCount), _rendPosition
				);
			}

			bool IsEmpty
			{
				get {
					return _rbegin == _rend;
				}
			}

			CountType length() {
				//writefln("Begin: %d:%s", countLesser(_begin), *_begin);
				//writefln("End: %d:%s", countLesser(_end), *_end);
				//return _begin
			
				if (_rbeginPosition != -1 && _rendPosition != -1) {
					return _rendPosition - _rbeginPosition;
				}

				return ParentTree.countLesser(_rend) - ParentTree.countLesser(_rbegin);
			}

			Range Slice() {
				return this.clone();
			}
		
			Range Slice(CountType start, CountType end) {
				return new Range(
					ParentTree,
					null,
					null,
					getOffsetPosition(start),
					getOffsetPosition(end)
				);
			}

			public Node this[CountType Index]
			{
				get
				{
					return ParentTree.locateNodeAtPosition(getOffsetPosition(Index));
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
