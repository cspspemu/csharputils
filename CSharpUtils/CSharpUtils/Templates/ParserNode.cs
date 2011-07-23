using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CSharpUtils.Templates
{
    public class ParserNodeContext
    {
        public TextWriter TextWriter;
        public dynamic Parameters;
    }

    abstract public class ParserNode
    {
        virtual public ParserNode Optimize(ParserNodeContext Context)
        {
            return this;
        }

        virtual public void WriteTo(ParserNodeContext Context)
        {
        }

        protected String EscapeString(String Value)
        {
            return '"' + Value + '"';
        }
    }

    public class DummyParserNode : ParserNode
    {
    }

    public class IfParserNode : ParserNode
    {
        public ParserNode ConditionNode;
        public ParserNode BodyIfNode;
        public ParserNode BodyElseNode;

        override public void WriteTo(ParserNodeContext Context)
        {
            Context.TextWriter.Write("if (");
            ConditionNode.WriteTo(Context);
            Context.TextWriter.Write(") {");
            BodyIfNode.WriteTo(Context);
            Context.TextWriter.Write("}");
            if (!(BodyElseNode is DummyParserNode))
            {
                Context.TextWriter.Write("else {");
                BodyElseNode.WriteTo(Context);
                Context.TextWriter.Write("}");
            }
        }

    }

    public class ParserNodeContainer : ParserNode
    {
        protected List<ParserNode> Nodes;

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
            ParserNodeContainer OptimizedNode = new ParserNodeContainer();
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
        public String Id;

        override public ParserNode Optimize(ParserNodeContext Context)
        {
            return this;
        }

        override public void WriteTo(ParserNodeContext Context)
        {
            //Context.TextWriter.Write(Context.Parameters[Id]);
            Context.TextWriter.Write("Context.GetVar({0})", EscapeString(Id));
        }
    }

    public class ParserNodeNumericLiteral : ParserNode
    {
        public long Value;

        override public ParserNode Optimize(ParserNodeContext Context)
        {
            return this;
        }

        override public void WriteTo(ParserNodeContext Context)
        {
            Context.TextWriter.Write(Value);
        }
    }

    public class ParserNodeLiteral : ParserNode
    {
        public String Text;

        override public ParserNode Optimize(ParserNodeContext Context)
        {
            return this;
        }

        override public void WriteTo(ParserNodeContext Context)
        {
            Context.TextWriter.Write(String.Format("Context.Output.Write({0});", EscapeString(Text)));
        }
    }

    public class ParserNodeOutputExpression : ParserNode
    {
        public ParserNode Parent;

        override public ParserNode Optimize(ParserNodeContext Context)
        {
            return new ParserNodeOutputExpression() { Parent = Parent.Optimize(Context) };
        }

        override public void WriteTo(ParserNodeContext Context)
        {
            Context.TextWriter.Write("Context.Output.Write(");
            Parent.WriteTo(Context);
            Context.TextWriter.Write(");");
        }
    }

    public class ParserNodeBinaryOperation : ParserNode
    {
        public ParserNode LeftNode;
        public ParserNode RightNode;
        public String Operator;

        override public ParserNode Optimize(ParserNodeContext Context)
        {
            LeftNode = LeftNode.Optimize(Context);
            RightNode = RightNode.Optimize(Context);

            if (LeftNode is ParserNodeNumericLiteral && RightNode is ParserNodeNumericLiteral)
            {
                var LeftNodeLiteral = (ParserNodeNumericLiteral)LeftNode;
                var RightNodeLiteral = (ParserNodeNumericLiteral)RightNode;
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
            Context.TextWriter.Write("(");
            LeftNode.WriteTo(Context);
            Context.TextWriter.Write(" " + Operator + " ");
            RightNode.WriteTo(Context);
            Context.TextWriter.Write(")");
        }
    }

}
