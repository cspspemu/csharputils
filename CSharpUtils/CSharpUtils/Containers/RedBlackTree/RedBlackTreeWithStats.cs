using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CountType = System.Int32;

namespace CSharpUtils.Containers.RedBlackTree
{
	public partial class RedBlackTreeWithStats<Type> : ICollection<Type>, ICloneable
	{
		Node RootNode = null;
		CountType _Length = 0;
		IComparer<Type> Comparer = null;
		bool AllowDuplicates = false;
		//const bool AllowDuplicates = false;

		public RedBlackTreeWithStats()
		{
			this.Comparer = Comparer<Type>.Default;
			//this.allowDuplicates = false;
			_setup();
		}

		public RedBlackTreeWithStats(IComparer<Type> Comparer, Node _end, int _length)
		{
			this.Comparer = Comparer;
			this.RootNode = _end;
			this._Length = _length;
		}

		public RedBlackTreeWithStats(IComparer<Type> Comparer)
		{
			this.Comparer = Comparer;
			//this.allowDuplicates = false;
			_setup();
		}

		private void _setup()
		{
			Assert(RootNode == null); //Make sure that _setup isn't run more than once.
			RootNode = Allocate();
		}

		private Node Allocate()
		{
			return new Node();
		}

		private Node Allocate(Type n)
		{
			Node node = new Node();
			node.Value = n;
			return node;
		}

		bool _less(Type A, Type B)
		{
			return Comparer.Compare(A, B) < 0;
		}

		private Node _add(Type n)
		{
			bool added;
			return _add(n, out added);
		}

		private Node _add(Type n, out bool added)
		{
			Node result = null;
			added = true;
		
			if (RootNode.left == null)
			{
				RootNode.left = result = Allocate(n);
			}
			else
			{
				Node newParent = RootNode.left;
				Node nxt;

				while (true)
				{
					if(_less(n, newParent.value))
					{
						nxt = newParent.left;
						if(nxt == null)
						{
							//
							// add to right of new parent
							//
							newParent.left = result = Allocate(n);
							break;
						}
					}
					else
					{
						if (!AllowDuplicates)
						{
							if (!_less(newParent.value, n))
							{
								result = newParent;
								added = false;
								break;
							}
						}

						nxt = newParent.right;
						if (nxt == null)
						{
							//
							// add to right of new parent
							//
							newParent.right = result = Allocate(n);
							break;
						}
					}

					newParent = nxt;
				}
			}
		
			if (AllowDuplicates) {
				result.UpdateCurrentAndAncestors(+1);
				result.setColor(RootNode);
				_Length++;
				return result;
			} else {
				if (added) {
					result.UpdateCurrentAndAncestors(+1);
					result.setColor(RootNode);
				}
				_Length++;
				return result;
			}
		}


		// find a node based on an element value
		public Node _find(Type e)
		{
			if (AllowDuplicates)
			{
				Node cur = RootNode.left;
				Node result = null;
				while (cur != null)
				{
					if (_less(cur.value, e)) {
						cur = cur.right;
					} else if(_less(e, cur.value)) {
						cur = cur.left;
					} else {
						// want to find the left-most element
						result = cur;
						cur = cur.left;
					}
				}
				return result;
			}
			else
			{
				Node cur = RootNode.left;
				//writefln("------------- (search:%s)", e);
				while (cur != null)
				{
					//writefln("%s", *cur);
					if (_less(cur.value, e)) {
						cur = cur.right;
					} else if (_less(e, cur.value)) {
						cur = cur.left;
					} else {
						//writefln("found!");
						return cur;
					}
				}
				return null;
			}
		}

		bool IsEmpty
		{
			get {
				return RootNode.left == null;
			}
		}

		CountType Length
		{
			get {
				return _Length;
			}
		}

		/**
		 * Fetch a range that spans all the elements in the container.
		 *
		 * Complexity: $(BIGOH log(n))
		 */
		/*
		Range Slice()
		{
			return all();
		}
		*/

		/**
		 * The front element in the container
		 *
		 * Complexity: $(BIGOH log(n))
		 */
		Type front
		{
			get {
				return RootNode.leftmost.value;
			}
		}

		/**
		 * The last element in the container
		 *
		 * Complexity: $(BIGOH log(n))
		 */
		Type back
		{
			get {
				return RootNode.prev.value;
			}
		}

		public bool Contains(Type V) {
			return _find(V) != null;
		}

		void clear()
		{
			RootNode.left = null;
			_Length = 0;
		}

		/**
		 * Insert a single element in the container.  Note that this does not
		 * invalidate any ranges currently iterating the container.
		 *
		 * Complexity: $(BIGOH log(n))
		 */
		public CountType insert(Type stuff)
		{
			if (AllowDuplicates)
			{
				_add(stuff);
				return 1;
			}
			else
			{
				bool added;
				_add(stuff, out added);
				return added ? 1 : 0;
			}
		}

		/**
		 * Remove an element from the container and return its value.
		 *
		 * Complexity: $(BIGOH log(n))
		 */
		Type removeAny()
		{
			var n = RootNode.leftmost;
			var result = n.value;
			n.remove(RootNode);
			_Length--;
			return result;
		}

		/**
		 * Remove the front element from the container.
		 *
		 * Complexity: $(BIGOH log(n))
		 */
		void removeFront()
		{
			RootNode.leftmost.remove(RootNode);
			_Length--;
		}

		/**
		 * Remove the back element from the container.
		 *
		 * Complexity: $(BIGOH log(n))
		 */
		void removeBack()
		{
			RootNode.prev.remove(RootNode);
			_Length--;
		}

		/*
		Range remove(Range r)
		{
			var b = r._rbegin;
			var e = r._rend;
			while(b != e)
			{
				b = b.remove(_end);
				--_length;
			}
			return new Range(e, _end);
		}

		Range remove(Range r)
		{
			auto b = r._rbegin;

			while(!r.empty)
				r.popFront(); // move take range to its last element

			auto e = r._rbegin;

			while(b != e)
			{
				b = b.remove(_end);
				--_length;
			}

			return new Range(e, _end);
		}
		*/

			/*
		size_t removeKey(U)(U[] elems...)
			if(isImplicitlyConvertible!(U, Elem))
		{
			immutable lenBefore = length;

			foreach(e; elems)
			{
				auto beg = _firstGreaterEqual(e);
				if(beg is _end || _less(e, beg.value))
					// no values are equal
					continue;
				beg.remove(_end);
				--_length;
			}

			return lenBefore - length;
		}
			 * */

		/*
		size_t removeKey(Stuff)(Stuff stuff)
			if(isInputRange!Stuff &&
			   isImplicitlyConvertible!(ElementType!Stuff, Elem) &&
			   !is(Stuff == Elem[]))
		{
			//We use array in case stuff is a Range from this RedBlackTree - either
			//directly or indirectly.
			return removeKey(array(stuff));
		}
		 * */

		// find the first node where the value is > e
		private Node _firstGreater(Type e)
		{
			// can't use _find, because we cannot return null
			var cur = RootNode.left;
			var result = RootNode;
			while (cur != null)
			{
				if(_less(e, cur.value))
				{
					result = cur;
					cur = cur.left;
				}
				else {
					cur = cur.right;
				}
			}
			return result;
		}

		// find the first node where the value is >= e
		private Node _firstGreaterEqual(Type e)
		{
			// can't use _find, because we cannot return null.
			var cur = RootNode.left;
			var result = RootNode;
			while (cur != null)
			{
				if (_less(cur.value, e))
				{
					cur = cur.right;
				}
				else
				{
					result = cur;
					cur = cur.left;
				}

			}
			return result;
		}

		/**
		 * Get a range from the container with all elements that are > e according
		 * to the less comparator
		 *
		 * Complexity: $(BIGOH log(n))
		 */
		Range upperBound(Type e)
		{
			return new Range(this, _firstGreater(e), RootNode);
		}

		/**
		 * Get a range from the container with all elements that are < e according
		 * to the less comparator
		 *
		 * Complexity: $(BIGOH log(n))
		 */
		Range lowerBound(Type e)
		{
			return new Range(this, RootNode.leftmost, _firstGreaterEqual(e));
		}

		/**
		 * Get a range from the container with all elements that are == e according
		 * to the less comparator
		 *
		 * Complexity: $(BIGOH log(n))
		 */
		Range equalRange(Type e)
		{
			var beg = _firstGreaterEqual(e);
			if(beg == RootNode || _less(e, beg.value)) {
				// no values are equal
				return new Range(this, beg, beg);
			}

			if(AllowDuplicates)
			{
				return new Range(this, beg, _firstGreater(e));
			}
			else
			{
				// no sense in doing a full search, no duplicates are allowed,
				// so we just get the next node.
				return new Range(this, beg, beg.next);
			}
		}

		//auto _equals(Node a, Node b) { return !_less(a, b) && !_less(b, a); }
		//auto _lessOrEquals(Node a, Node b) { return _less(a, b) || _equals(a, b); }
	
		CountType countLesser(Node node) {
			if (node == null) return -1;
			if (node.parent == null) return node.ChildCountLeft;

			//auto prev = node;
			var it = node;
			CountType count = 0;
			while (true) {
				if (it.parent == null) break;
				//writefln("+%d+1", it.childCountLeft);
				//if (it.value <= node.value) {
				if (!_less(node.value, it.value)) {
					count += it.ChildCountLeft + 1;
				}
				it = it.parent;
				if (it == null) {
					//writefln("it is null");
					break;
				} else {
					//writefln("less(%s, %s) : %d", it.value, node.value, it.value < node.value);
					
					//if (_less(it, node)) break;
					//if (it.value >= node.value) break;
				}
				//_less
				//if (it._right != prev) break;
				//prev = it;
			}
			return count - 1;
		}
	
		CountType getNodePosition(Node Node) {
			return countLesser(Node);
		}
	
		Node locateNodeAtPosition(CountType positionToFind) {
			if (positionToFind < 0) throw(new Exception("Negative locateNodeAt"));
			Node current = RootNode;
			CountType currentPosition = RootNode.ChildCountLeft;
				
			//writefln("[AA---(%d)]", positionToFind);
				
			while (true) {
				if (current == null) return null;
				//writefln("%s : %d", current, currentPosition);
					
				//CountType currentPositionExpected = getNodePosition(current);
				if (currentPosition == positionToFind) return current;
					
				if (positionToFind < currentPosition) {
					//currentPosition += current.childCountLeft;
					current = current.left;
					if (current == null) return null;
					//writefln("Left(%s/%s) ::: %d-%d", current.childCountLeft, current.childCountRight, currentPosition, current.childCountRight);
					currentPosition -= current.ChildCountRight + 1;
				} else {
					current = current.right;
					if (current == null) return null;
					//writefln("Right(%s/%s) ::: %d+%d", current.childCountLeft, current.childCountRight, currentPosition, current.childCountLeft);
					currentPosition += current.ChildCountLeft + 1;
				}
				//writefln("currentPosition: %d/%d/%d", currentPosition, currentPositionExpected, positionToFind);
			}
		}
	
		public Range All {
			get {
				//return new Range(_end.leftmost, _end);
				return new Range(this, RootNode.leftmost, RootNode, 0, _Length);
			}
		}
	
		CountType DebugValidateStatsNodeSubtree() {
			return RootNode.DebugValidateStatsNodeSubtree();
		}

		public void PrintTree()
		{
			RootNode.left.PrintTree();
		}
	
		public void DebugValidateTree()
		{
			int InternalLength = _Length;
			int CalculatedLength = DebugValidateStatsNodeSubtree();
			Assert(CalculatedLength == InternalLength);
		}

		public void Add(Type item)
		{
			insert(item);
		}

		public void Clear()
		{
			clear();
		}

		public void CopyTo(Type[] array, CountType arrayIndex)
		{
			throw new NotImplementedException();
		}

		public CountType Count
		{
			get { return Length; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public bool Remove(Type item)
		{
			Node node = _firstGreaterEqual(item);
			if (node != null)
			{
				node.remove(RootNode);
				return true;
			}
			else
			{
				return false;
			}
			//_find(item).remove(_end);
			//throw new NotImplementedException();
		}

		public IEnumerator<Type> GetEnumerator()
		{
			return (All as IEnumerable<Type>).GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return (All as IEnumerable<Type>).GetEnumerator();
		}

		public RedBlackTreeWithStats<Type> Clone()
		{
			return new RedBlackTreeWithStats<Type>(Comparer, RootNode.Clone(), _Length);
		}

		object ICloneable.Clone()
		{
			return new RedBlackTreeWithStats<Type>(Comparer, RootNode.Clone(), _Length);
		}
	}
}
