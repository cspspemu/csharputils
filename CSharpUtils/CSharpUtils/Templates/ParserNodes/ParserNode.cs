using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CSharpUtils.Templates.TemplateProvider;
using CSharpUtils.Templates.Utils;
using CSharpUtils.Templates.Runtime;

namespace CSharpUtils.Templates.ParserNodes
{
	public class ParserNodeContext
	{
		public TextWriter TextWriter;
		public TemplateFactory TemplateFactory;
	}

	abstract public class ParserNode
	{
		virtual public ParserNode Optimize(ParserNodeContext Context)
		{
			return this;
		}

		virtual public void Dump(int Level = 0, String Info = "")
		{
			Console.WriteLine("{0}{1}:{2}", new String(' ', Level * 4), Info, this);
		}

		virtual public void WriteTo(ParserNodeContext Context)
		{
		}

		protected T CreateThisInstanceAs<T>()
		{
			return (T)(Activator.CreateInstance(this.GetType()));
		}

		/*
		override public ParserNode Optimize(ParserNodeContext Context)
		{
			ParserNodeParent ParserNodeParent = Activator.CreateInstance(this.GetType());
			ParserNodeParent.Parent = Parent.Optimize(Context);
			return ParserNodeParent;
		}
			* */


		public override string ToString()
		{
			return String.Format("{0}", this.GetType().Name);
		}

		internal void OptimizeAndWrite(ParserNodeContext Context)
		{
			Optimize(Context).WriteTo(Context);
		}
	}

	public class DummyParserNode : ParserNode
	{
	}

	public class ForParserNode : ParserNode
	{
		public String VarName;
		public ParserNode LoopIterator;
		public ParserNode BodyBlock;

		public override void Dump(int Level = 0, String Info = "")
		{
			base.Dump(Level, Info);
			LoopIterator.Dump(Level + 1, "LoopIterator");
			BodyBlock.Dump(Level + 1, "BodyBlock");
		}

		override public void WriteTo(ParserNodeContext Context)
		{
			Context.TextWriter.Write("foreach (var LoopVar in (");
			LoopIterator.WriteTo(Context);
			Context.TextWriter.Write(")) {");
			Context.TextWriter.WriteLine("");
			Context.TextWriter.WriteLine("Context.SetVar({0}, LoopVar);", StringUtils.EscapeString(VarName));
			BodyBlock.WriteTo(Context);
			Context.TextWriter.Write("}");
			Context.TextWriter.WriteLine("");
		}

		public override string ToString()
		{
			return base.ToString() + "('" + VarName + "')";
		}
	}

	public class ParserNodeIf : ParserNode
	{
		public ParserNode ConditionNode;
		public ParserNode BodyIfNode;
		public ParserNode BodyElseNode;

		public override void Dump(int Level = 0, String Info = "")
		{
			base.Dump(Level, Info);
			ConditionNode.Dump(Level + 1, "Condition");
			BodyIfNode.Dump(Level + 1, "IfBody");
			BodyElseNode.Dump(Level + 1, "ElseBody");
		}

		override public void WriteTo(ParserNodeContext Context)
		{
			Context.TextWriter.Write("if (DynamicUtils.ConvertToBool(");
			ConditionNode.WriteTo(Context);
			Context.TextWriter.Write(")) {");
			Context.TextWriter.WriteLine("");
			BodyIfNode.WriteTo(Context);
			Context.TextWriter.Write("}");
			if (!(BodyElseNode is DummyParserNode))
			{
				Context.TextWriter.Write(" else {");
				BodyElseNode.WriteTo(Context);
				Context.TextWriter.Write("}");
			}
			Context.TextWriter.WriteLine("");
		}
	}

	public class ParserNodeContainer : ParserNode
	{
		protected List<ParserNode> Nodes;

		public override void Dump(int Level = 0, String Info = "")
		{
			base.Dump(Level, Info);
			int n = 0;
			foreach (var Node in Nodes)
			{
				Node.Dump(Level + 1, String.Format("Node{0}", n));
				n++;
			}
		}

		public ParserNodeContainer()
		{
			Nodes = new List<ParserNode>();
		}

		public void Add(ParserNode Node)
		{
			Nodes.Add(Node);
		}

		override public ParserNode Optimize(ParserNodeContext Context)
		{
			ParserNodeContainer OptimizedNode = CreateThisInstanceAs<ParserNodeContainer>();
			foreach (var Node in Nodes)
			{
				OptimizedNode.Add(Node.Optimize(Context));
			}
			return OptimizedNode;
		}

		override public void WriteTo(ParserNodeContext Context)
		{
			foreach (var Node in Nodes)
			{
				Node.WriteTo(Context);
			}
		}
	}

	public class ParserNodeIdentifier : ParserNode
	{
		protected String Id;

		public ParserNodeIdentifier(String Id)
		{
			this.Id = Id;
		}

		override public void WriteTo(ParserNodeContext Context)
		{
			Context.TextWriter.Write("Context.GetVar({0})", StringUtils.EscapeString(Id));
		}

		public override string ToString()
		{
			return base.ToString() + "('" + Id + "')";
		}
	}

	public class ParserNodeNumericLiteral : ParserNode
	{
		public long Value;

		override public void WriteTo(ParserNodeContext Context)
		{
			Context.TextWriter.Write(Value);
		}

		public override string ToString()
		{
			return base.ToString() + "(" + Value + ")";
		}
	}

	public class ParserNodeStringLiteral : ParserNode
	{
		protected String Value;

		public ParserNodeStringLiteral(String Value)
		{
			this.Value = Value;
		}

		override public void WriteTo(ParserNodeContext Context)
		{
			Context.TextWriter.Write(StringUtils.EscapeString(Value));
		}

		public override string ToString()
		{
			return base.ToString() + "('" + Value + "')";
		}
	}

	public class ParserNodeLiteral : ParserNode
	{
		public String Text;

		override public void WriteTo(ParserNodeContext Context)
		{
			Context.TextWriter.Write(String.Format("Context.Output.Write(Context.AutoFilter({0}));", StringUtils.EscapeString(Text)));
			Context.TextWriter.WriteLine("");
		}

		public override string ToString()
		{
			return base.ToString() + "('" + Text + "')";
		}
	}

	public class ParserNodeParent : ParserNode
	{
		public ParserNode Parent;

		public override void Dump(int Level = 0, String Info = "")
		{
			base.Dump(Level, Info);
			Parent.Dump(Level + 1, "Parent");
		}

		override public ParserNode Optimize(ParserNodeContext Context)
		{
			var That = CreateThisInstanceAs<ParserNodeParent>();
			That.Parent = Parent.Optimize(Context);
			return That;
		}
	}

	public class ParserNodeOutputExpression : ParserNodeParent
	{
		override public void WriteTo(ParserNodeContext Context)
		{
			Context.TextWriter.Write("Context.OutputWriteAutoFiltered(");
			Parent.WriteTo(Context);
			Context.TextWriter.Write(");");
			Context.TextWriter.WriteLine("");
		}
	}

	public class ParserNodeExtends : ParserNodeParent
	{
		override public void WriteTo(ParserNodeContext Context)
		{
			Context.TextWriter.Write("SetAndRenderParentTemplate(");
			Parent.WriteTo(Context);
			Context.TextWriter.Write(", Context);");
			Context.TextWriter.WriteLine("");
		}
	}

	public class ParserNodeCallBlock : ParserNode
	{
		public String BlockName;

		public ParserNodeCallBlock(String BlockName)
		{
			this.BlockName = BlockName;
		}

		override public void WriteTo(ParserNodeContext Context)
		{
			Context.TextWriter.WriteLine("CallBlock({0}, Context);", StringUtils.EscapeString(this.BlockName));
			Context.TextWriter.WriteLine("");
		}
	}

	public class ParserNodeUnaryOperation : ParserNodeParent
	{
		public String Operator;

		override public void WriteTo(ParserNodeContext Context)
		{
			Context.TextWriter.Write("{0}(", Operator);
			Parent.WriteTo(Context);
			Context.TextWriter.Write(")");
		}
	}

	public class ParserNodeTernaryOperation : ParserNode
	{
		public ParserNode ConditionNode;
		public ParserNode TrueNode;
		public ParserNode FalseNode;
		public String Operator;

		public ParserNodeTernaryOperation(ParserNode ConditionNode, ParserNode TrueNode, ParserNode FalseNode, String Operator)
		{
			this.ConditionNode = ConditionNode;
			this.TrueNode = TrueNode;
			this.FalseNode = FalseNode;
			this.Operator = Operator;
		}

		override public void WriteTo(ParserNodeContext Context)
		{
			switch (Operator)
			{
				case "?":
					Context.TextWriter.Write("DynamicUtils.ConvertToBool(");
					ConditionNode.WriteTo(Context);
					Context.TextWriter.Write(")");
					Context.TextWriter.Write("?");
					Context.TextWriter.Write("(");
					TrueNode.WriteTo(Context);
					Context.TextWriter.Write(")");
					Context.TextWriter.Write(":");
					Context.TextWriter.Write("(");
					FalseNode.WriteTo(Context);
					Context.TextWriter.Write(")");
					break;
				default:
					throw (new Exception(String.Format("Unknown Operator '{0}'", Operator)));
			}
		}
	}

	public class ParserNodeBlockParent : ParserNode
	{
		String BlockName;

		public ParserNodeBlockParent(String BlockName)
		{
			this.BlockName = BlockName;
		}

		override public void WriteTo(ParserNodeContext Context)
		{
			Context.TextWriter.WriteLine("CallParentBlock({0}, Context);", StringUtils.EscapeString(BlockName));
		}
	}

	public class ParserNodeAccess : ParserNode
	{
		ParserNode Parent;
		ParserNode Key;

		public ParserNodeAccess(ParserNode Parent, ParserNode Key)
		{
			this.Parent = Parent;
			this.Key = Key;
		}

		override public void WriteTo(ParserNodeContext Context)
		{
			Context.TextWriter.Write("DynamicUtils.Access(");
			Parent.WriteTo(Context);
			Context.TextWriter.Write(",");
			Key.WriteTo(Context);
			Context.TextWriter.Write(")");
		}
	}

	public class ParserNodeBinaryOperation : ParserNode
	{
		public ParserNode LeftNode;
		public ParserNode RightNode;
		public String Operator;

		public ParserNodeBinaryOperation(ParserNode LeftNode, ParserNode RightNode, String Operator)
		{
			this.LeftNode = LeftNode;
			this.RightNode = RightNode;
			this.Operator = Operator;
		}

		public override void Dump(int Level = 0, String Info = "")
		{
			base.Dump(Level, Info);
			LeftNode.Dump(Level + 1, "Left");
			RightNode.Dump(Level + 1, "Right");
		}

		override public ParserNode Optimize(ParserNodeContext Context)
		{
			var LeftNodeOptimized = LeftNode.Optimize(Context);
			var RightNodeOptimized = RightNode.Optimize(Context);

			if ((LeftNodeOptimized is ParserNodeNumericLiteral) && (RightNodeOptimized is ParserNodeNumericLiteral))
			{
				var LeftNodeLiteral = (ParserNodeNumericLiteral)LeftNodeOptimized;
				var RightNodeLiteral = (ParserNodeNumericLiteral)RightNodeOptimized;
				switch (Operator)
				{
					case "+": return new ParserNodeNumericLiteral() { Value = LeftNodeLiteral.Value + RightNodeLiteral.Value, };
					case "-": return new ParserNodeNumericLiteral() { Value = LeftNodeLiteral.Value - RightNodeLiteral.Value, };
					case "*": return new ParserNodeNumericLiteral() { Value = LeftNodeLiteral.Value * RightNodeLiteral.Value, };
					case "/": return new ParserNodeNumericLiteral() { Value = LeftNodeLiteral.Value / RightNodeLiteral.Value, };
				}
			}
			return this;
		}

		override public void WriteTo(ParserNodeContext Context)
		{
			Context.TextWriter.Write("DynamicUtils.BinaryOperation_" + DynamicUtils.GetOpName(Operator) + "(");
			LeftNode.WriteTo(Context);
			Context.TextWriter.Write(",");
			RightNode.WriteTo(Context);
			Context.TextWriter.Write(")");
		}

		public override string ToString()
		{
			return String.Format("ParserNodeBinaryOperation('{0}')", Operator);
		}
	}
}
