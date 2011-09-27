using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CountType = System.Int32;

namespace CSharpUtils.Containers.RedBlackTree
{
	public partial class RedBlackTreeWithStats<Type>
	{
		static void DebugAssert(bool Assertion)
		{
			if (!Assertion) throw (new InvalidOperationException());
		}

		static void Assert(bool Assertion)
		{
			DebugAssert(Assertion);
		}

		internal enum Color
		{
			Red, Black
		}

		public class Node
		{
			internal Node _LeftNode;
			internal Node _RightNode;
			internal Node _ParentNode;

			internal CountType ChildCountLeft;
			internal CountType ChildCountRight;

			internal Type Value;
			internal Color Color;

			internal Type value
			{
				get
				{
					return Value;
				}
				set
				{
					Value = value;
				}
			}

			internal Color color
			{
				get
				{
					return Color;
				}
				set
				{
					Color = value;
				}
			}

			internal Node _left
			{
				get
				{
					return _LeftNode;
				}
				set
				{
					_LeftNode = value;
				}
			}

			internal Node _right
			{
				get
				{
					return _RightNode;
				}
				set
				{
					_RightNode = value;
				}
			}

			internal Node _parent
			{
				get
				{
					return _ParentNode;
				}
				set
				{
					_ParentNode = value;
				}
			}

			internal Node LeftNode
			{
				get
				{
					return _LeftNode;
				}
				set
				{
					_LeftNode = value;
					if (value != null) value._ParentNode = this;
				}
			}

			internal Node RightNode
			{
				get
				{
					return _RightNode;
				}
				set
				{
					_RightNode = value;
					if (value != null) value._ParentNode = this;
				}
			}

			internal Node ParentNode
			{
				get
				{
					return _ParentNode;
				}
			}

			internal Node left
			{
				get
				{
					return _LeftNode;
				}
				set
				{
					_LeftNode = value;
					if (value != null) value._ParentNode = this;
				}
			}

			internal Node right
			{
				get
				{
					return _RightNode;
				}
				set
				{
					_RightNode = value;
					if (value != null) value._ParentNode = this;
				}
			}

			internal Node parent
			{
				get
				{
					return _ParentNode;
				}
			}

			internal Node RootNode
			{
				get
				{
					var Current = this;
					while (Current.ParentNode != null) Current = Current.ParentNode;
					return Current;
				}
			}

			internal CountType DebugValidateStatsNodeSubtree()
			{
				CountType TotalChildCountLeft = 0;
				CountType TotalChildCountRight = 0;
				if (LeftNode != null) TotalChildCountLeft = LeftNode.DebugValidateStatsNodeSubtree();
				if (RightNode != null) TotalChildCountRight = RightNode.DebugValidateStatsNodeSubtree();
				DebugAssert(ChildCountLeft != TotalChildCountLeft);
				DebugAssert(ChildCountRight != TotalChildCountRight);
				return 1 + this.ChildCountLeft + this.ChildCountRight;
			}

			internal void UpdateCurrentAndAncestors(CountType CountIncrement)
			{
				var PreviousNode = this;
				var CurrentNode = this.ParentNode;
				while (CurrentNode != null)
				{
					// @TODO: Change
					// prev.isLeftNode
					if (CurrentNode.LeftNode == PreviousNode)
					{
						CurrentNode.ChildCountLeft += ChildCountRight;
					}
					else if (CurrentNode.RightNode == PreviousNode)
					{
						CurrentNode.ChildCountRight += ChildCountRight;
					}
					PreviousNode = CurrentNode;
					CurrentNode = CurrentNode.ParentNode;
				}
			}

			public override string ToString()
			{
				return String.Format(
					"RedBlackTreeWithStats.Node(Value={0}, Color={1}, ChildCountLeft={2}, ChildCountRight={3})",
					Value,
					Enum.GetName(typeof(Color), Color),
					ChildCountLeft,
					ChildCountRight
				);
			}

			internal void PrintTree(Node MarkNode = null, int Level = 0, String Label = "L")
			{
				string Info = "";
				if (this == MarkNode) Info = " (mark)";

				Console.WriteLine(
					"{0}- {1}:{2}{3}",
					new String(' ', Level * 2),
					Label, this, Info
				);

				if (LeftNode != null)
				{
					LeftNode.PrintTree(MarkNode, Level + 1, "L");
				}

				if (RightNode != null)
				{
					RightNode.PrintTree(MarkNode, Level + 1, "R");
				}
			}

			internal bool IsLeftNode
			{
				get
				{
					Assert(ParentNode != null);
					return this == ParentNode.LeftNode;
				}
			}

			internal bool IsRightNode
			{
				get
				{
					Assert(ParentNode != null);
					return this == ParentNode.RightNode;
				}
			}

			internal Node rotateL()
			{
				return RotateLeft();
			}

			internal Node rotateR()
			{
				return RotateRight();
			}

			internal Node RotateRight()
			{
				Assert(LeftNode != null);

				if (this.IsLeftNode)
				{
					ParentNode.LeftNode = this.LeftNode;
				}
				else
				{
					ParentNode.RightNode = this.LeftNode;
				}

				this.ChildCountLeft = this.LeftNode.ChildCountRight;
				LeftNode.ChildCountRight = this.ChildCountLeft + this.ChildCountRight + 1;

				Node TempNode = LeftNode.RightNode;
				LeftNode.RightNode = this;
				LeftNode = TempNode;

				return this;
			}

			Node RotateLeft()
			{
				Assert(RightNode != null);

				// sets _right._parent also
				if (this.IsLeftNode)
				{
					ParentNode.LeftNode = this.RightNode;
				}
				else
				{
					ParentNode.RightNode = this.RightNode;
				}

				this.ChildCountRight = this.RightNode.ChildCountLeft;
				RightNode.ChildCountLeft = this.ChildCountLeft + this.ChildCountRight + 1;

				Node TempNode = RightNode.LeftNode;
				RightNode.LeftNode = this;
				RightNode = TempNode;

				return this;
			}

			internal void setColor(Node End)
			{
				SetColor(End);
			}

			internal void SetColor(Node End)
			{
				//writefln("Updating tree...");
				// test against the marker node
				if (ParentNode == End)
				{
					//
					// this is the root node, color it black
					//
					Color = Color.Black;
					return;
				}

				if (ParentNode.Color != Color.Red)
				{
					return;
				}

				Node cur = this;

				while (true)
				{
					// because root is always black, _parent._parent always exists
					if (cur.ParentNode.IsLeftNode)
					{
						// parent is left node, y is 'uncle', could be null
						Node y = cur._parent._parent._right;
						if (y != null && y.Color == Color.Red)
						{
							cur._parent.Color = Color.Black;
							y.Color = Color.Black;
							cur = cur._parent._parent;
							if (cur._parent == End)
							{
								// root node
								cur.Color = Color.Black;
								break;
							}
							else
							{
								// not root node
								cur.Color = Color.Red;
								if (cur._parent.Color == Color.Black)
								{
									// satisfied, exit the loop
									break;
								}
							}
						}
						else
						{
							if (!cur.IsLeftNode) cur = cur._parent.RotateLeft();
							cur._parent.Color = Color.Black;
							cur = cur._parent._parent.RotateRight();
							cur.Color = Color.Red;
							// tree should be satisfied now
							break;
						}
					}
					else
					{
						// parent is right node, y is 'uncle'
						Node y = cur._parent._parent._left;
						if (y != null && y.Color == Color.Red)
						{
							cur._parent.Color = Color.Black;
							y.Color = Color.Black;
							cur = cur._parent._parent;
							if (cur._parent == End)
							{
								// root node
								cur.Color = Color.Black;
								break;
							}
							else
							{
								// not root node
								cur.Color = Color.Red;

								if (cur._parent.Color == Color.Black)
								{
									// satisfied, exit the loop
									break;
								}
							}
						}
						else
						{
							if (cur.IsLeftNode)
							{
								cur = cur._parent.RotateRight();
							}
							cur._parent.Color = Color.Black;
							cur = cur._parent._parent.RotateLeft();
							cur.Color = Color.Red;
							// tree should be satisfied now
							break;
						}
					}
				}

			}

			internal void SetParentThisChild(Node newThis)
			{
				if (IsLeftNode)
				{
					_parent.LeftNode = newThis;
				}
				else
				{
					_parent.RightNode = newThis;
				}
			}

			internal Node remove(Node end)
			{
				//
				// remove this node from the tree, fixing the color if necessary.
				//
				Node x;
				Node ret;

				if (_left == null || _right == null)
				{
					//static if (hasStats) updateCurrentAndAncestors(-1);
					ret = NextNode;
				}
				else
				{
					//
					// normally, we can just swap this node's and y's value, but
					// because an iterator could be pointing to y and we don't want to
					// disturb it, we swap this node and y's structure instead.  This
					// can also be a benefit if the value of the tree is a large
					// struct, which takes a long time to copy.
					//
					Node yp, yl, yr;
					Node y = NextNode;
					yp = y._parent;
					yl = y._left;
					yr = y._right;
					var y_childCountLeft = y.ChildCountLeft;
					var y_childCountRight = y.ChildCountRight;
					var yc = y.Color;
					var isyleft = y.IsLeftNode;

					//
					// replace y's structure with structure of this node.
					//
					SetParentThisChild(y);
					//
					// need special case so y doesn't point back to itself
					//
					y.LeftNode = _left;
					if (_right == y)
					{
						y.RightNode = this;
					}
					else
					{
						y.RightNode = _right;
					}
					y.Color = Color;

					y.ChildCountLeft = ChildCountLeft;
					y.ChildCountRight = ChildCountRight;

					//
					// replace this node's structure with structure of y.
					//
					left = yl;
					right = yr;
					if (_parent != y)
					{
						if (isyleft)
						{
							yp.left = this;
						}
						else
						{
							yp.right = this;
						}
					}
					Color = yc;

					ChildCountLeft = y_childCountLeft;
					ChildCountRight = y_childCountRight;

					//
					// set return value
					//
					ret = y;
				}

				UpdateCurrentAndAncestors(-1);

				// if this has less than 2 children, remove it
				if (_left != null)
				{
					x = _left;
				}
				else
				{
					x = _right;
				}

				// remove this from the tree at the end of the procedure
				bool removeThis = false;
				if (x == null)
				{
					// pretend this is a null node, remove this on finishing
					x = this;
					removeThis = true;
				}
				else if (IsLeftNode)
				{
					_parent.left = x;
				}
				else
				{
					_parent.right = x;
				}

				// if the color of this is black, then it needs to be fixed
				if (Color == Color.Black)
				{
					// need to recolor the tree.
					while (x._parent != end && x.Color == Color.Black)
					{
						if (x.IsLeftNode)
						{
							// left node
							var w = x._parent._right;
							if (w.Color == Color.Red)
							{
								w.Color = Color.Black;
								x._parent.Color = Color.Red;
								x._parent.RotateLeft();
								w = x._parent._right;
							}

							var wl = w.left;
							var wr = w.right;

							if (
								(wl == null || wl.Color == Color.Black) &&
								(wr == null || wr.Color == Color.Black)
							)
							{
								w.Color = Color.Red;
								x = x._parent;
							}
							else
							{
								if (wr == null || wr.Color == Color.Black)
								{
									// wl cannot be null here
									wl.color = Color.Black;
									w.color = Color.Red;
									w.RotateRight();
									w = x._parent._right;
								}

								w.color = x._parent.color;
								x._parent.color = Color.Black;
								w._right.color = Color.Black;
								x._parent.RotateLeft();
								x = end.left; // x = root
							}
						}
						else
						{
							// right node
							var w = x._parent._left;
							if (w.color == Color.Red)
							{
								w.color = Color.Black;
								x._parent.color = Color.Red;
								x._parent.rotateR();
								w = x._parent._left;
							}
							var wl = w.left;
							var wr = w.right;
							if (
								(wl == null || wl.color == Color.Black) &&
								(wr == null || wr.color == Color.Black)
							)
							{
								w.color = Color.Red;
								x = x._parent;
							}
							else
							{
								if (wl == null || wl.color == Color.Black)
								{
									// wr cannot be null here
									wr.color = Color.Black;
									w.color = Color.Red;
									w.rotateL();
									w = x._parent._left;
								}

								w.color = x._parent.color;
								x._parent.color = Color.Black;
								w._left.color = Color.Black;
								x._parent.rotateR();
								x = end.left; // x = root
							}
						}
					}
					x.color = Color.Black;
				}

				if (removeThis)
				{
					//
					// clear this node out of the tree
					//
					if (IsLeftNode)
					{
						_parent.left = null;
					}
					else
					{
						_parent.right = null;
					}
				}

				return ret;
			}

			internal Node leftmost
			{
				get
				{
					return LeftMostNode;
				}
			}

			internal Node rightmost
			{
				get
				{
					return RightMostNode;
				}
			}

			internal Node next
			{
				get
				{
					return NextNode;
				}
			}

			internal Node prev
			{
				get
				{
					return PreviousNode;
				}
			}

			internal Node LeftMostNode
			{
				get
				{
					var result = this;
					while (result._left != null) result = result._left;
					return result;
				}
			}

			internal Node RightMostNode
			{
				get
				{
					var result = this;
					while (result._right != null) result = result._right;
					return result;
				}
			}

			internal Node NextNode
			{
				get
				{
					Node n = this;
					if (n.right == null)
					{
						while (!n.IsLeftNode) n = n._parent;
						return n._parent;
					}
					else
					{
						return n.right.LeftMostNode;
					}
				}
			}

			internal Node PreviousNode
			{
				get
				{
					Node n = this;
					if (n.left == null)
					{
						while (n.IsLeftNode) n = n._parent;
						return n._parent;
					}
					else
					{
						return n.left.RightMostNode;
					}
				}
			}

			internal Node Clone()
			{
				Node that = new Node();
				that.Value = this.Value;
				that.Color = this.Color;
				that.ChildCountLeft = this.ChildCountLeft;
				that.ChildCountRight = this.ChildCountRight;
				if (this._left != null) that.left = _left.Clone();
				if (this._right != null) that.right = _right.Clone();
				return that;
			}
		}
	}
}
