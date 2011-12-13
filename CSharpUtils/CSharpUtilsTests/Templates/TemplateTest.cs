using CSharpUtils.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using CSharpUtils;
using System.Linq;
using CSharpUtils.Templates.Tokenizers;
using CSharpUtils.Templates.TemplateProvider;
using System.Collections;
using CSharpUtils.Templates.Runtime;

namespace CSharpUtilsTests.Templates
{
	[TestClass]
	public class TemplateTest
	{
		static protected TemplateCode CompileTemplateCodeByString(String Code) {
			var TemplateCodeGen = new TemplateCodeGen(Code);
			TemplateCodeGen.OutputGeneratedCode = true;
			return TemplateCodeGen.GetTemplateCode();
		}

		[TestMethod]
		public void TestExecVariableSimple()
		{
			Assert.AreEqual("Hello Test", CompileTemplateCodeByString("Hello {{ User }}").RenderToString(new TemplateScope(new Dictionary<String, object>() {
				{ "User", "Test" },
			})));
		}

		[TestMethod]
		public void TestExecFor()
		{
			Assert.AreEqual("1234", CompileTemplateCodeByString("{% for Item in List %}{{ Item }}{% endfor %}").RenderToString(new TemplateScope(new Dictionary<String, object>() {
				{ "List", new int[] { 1, 2, 3, 4 } },
			})));
		}

		[TestMethod]
		public void TestTrueFalseNoneAreImmutableConstants()
		{
			var Scope = new TemplateScope(new Dictionary<String, object>() {
				{ "true", false },
				{ "false", true },
				{ "none", 1 },
			});

			Assert.AreEqual("1", CompileTemplateCodeByString("{% if true %}1{% else %}0{% endif %}").RenderToString(Scope));
			Assert.AreEqual("0", CompileTemplateCodeByString("{% if false %}1{% else %}0{% endif %}").RenderToString(Scope));
			Assert.AreEqual("0", CompileTemplateCodeByString("{% if none %}1{% else %}0{% endif %}").RenderToString(Scope));
		}

		/*
		[TestMethod]
		public void TestInvalidClosing()
		{
			// if / endfor
			// Check that thows a descriptive exception.
			Assert.AreEqual("1", CompileTemplateCodeByString("{% if true %}1{% enfor %}").RenderToString());
		}
		*/

		[TestMethod]
		public void TestExecForCreateScope()
		{
			Assert.AreEqual("1234", CompileTemplateCodeByString("{{ Item }}{% for Item in List %}{{ Item }}{% endfor %}{{ Item }}").RenderToString(new TemplateScope(new Dictionary<String, object>() {
				{ "List", new int[] { 1, 2, 3, 4 } },
			})));

			Assert.AreEqual("512345", CompileTemplateCodeByString("{{ Item }}{% for Item in List %}{{ Item }}{% endfor %}{{ Item }}").RenderToString(new TemplateScope(new Dictionary<String, object>() {
				{ "List", new int[] { 1, 2, 3, 4 } },
				{ "Item", 5 },
			})));
		}

		[TestMethod]
		public void TestExecPow()
		{
			Assert.AreEqual("256", CompileTemplateCodeByString("{{ 2 ** 8 }}").RenderToString());
		}

		[TestMethod]
		public void TestExecForInsideFor()
		{
			Assert.AreEqual("[1]1234[1][2]2468[2]", CompileTemplateCodeByString("{% for Item2 in List2 %}[{{ Item2 }}]{% for Item1 in List1 %}{{ Item1 * Item2 }}{% endfor %}[{{ Item2 }}]{% endfor %}{{ Item1 }}{{ Item2 }}").RenderToString(new TemplateScope(new Dictionary<String, object>() {
				{ "List1", new int[] { 1, 2, 3, 4 } },
				{ "List2", new int[] { 1, 2 } },
			})));
		}

		class Post
		{
			public String Text;
		}

		[TestMethod]
		public void TestExecForObjects()
		{
			Assert.AreEqual("HelloWorld", CompileTemplateCodeByString("{% for Post in Posts %}{{ Post.Text }}{% endfor %}").RenderToString(new TemplateScope(new Dictionary<String, object>() {
				{ "Posts", new List<Post>() {
					new Post() { Text = "Hello" },
					new Post() { Text = "World" },
				} },
			})));
		}

		[TestMethod]
		public void TestExecForElse()
		{
			var Params1 = new TemplateScope(new Dictionary<String, object>() { { "List", new int[] { 1, 2, 3, 4, 5 } } });
			var Params2 = new TemplateScope(new Dictionary<String, object>() { { "List", new int[] { } } });
			Assert.AreEqual("12345", CompileTemplateCodeByString("{% for Item in List %}{{ Item }}{% else %}Empty{% endfor %}").RenderToString(Params1));
			Assert.AreEqual("Empty", CompileTemplateCodeByString("{% for Item in List %}{{ Item }}{% else %}Empty{% endfor %}").RenderToString(Params2));
		}

		[TestMethod]
		public void TestExecBlock()
		{
			Assert.AreEqual("1", CompileTemplateCodeByString("{% block Body %}1{% endblock %}").RenderToString());
		}

		[TestMethod]
		public void TestExecForRange()
		{
			Assert.AreEqual("0123456789", CompileTemplateCodeByString("{% for Item in 0..9 %}{{ Item }}{% endfor %}").RenderToString());
		}

		[TestMethod]
		public void TestExecWithOperator()
		{
			Assert.AreEqual("Hello 3 World", CompileTemplateCodeByString("Hello {{ 1 + 2 }} World").RenderToString());
		}

		[TestMethod]
		public void TestExecWithOperatorPrecedence()
		{
			Assert.AreEqual("Hello 5 World", CompileTemplateCodeByString("Hello {{ 1 + 2 * 2 }} World").RenderToString());
		}

		[TestMethod]
		public void TestExecIfElseCond0()
		{
			Assert.AreEqual("B", CompileTemplateCodeByString("{% if 0 %}A{% else %}B{% endif %}").RenderToString());
		}

		[TestMethod]
		public void TestExecIfElseCond1()
		{
			Assert.AreEqual("A", CompileTemplateCodeByString("{% if 1 %}A{% else %}B{% endif %}").RenderToString());
		}

		[TestMethod]
		public void TestExecTernaryTrue()
		{
			Assert.AreEqual("A", CompileTemplateCodeByString("{{ 1 ? 'A' : 'B' }}").RenderToString());
		}

		[TestMethod]
		public void TestExecTernaryFalse()
		{
			Assert.AreEqual("B", CompileTemplateCodeByString("{{ (1 - 1) ? 'A' : 'B' }}").RenderToString());
		}

		[TestMethod]
		public void TestExecUnary()
		{
			Assert.AreEqual("-6", CompileTemplateCodeByString("{{ -(1 + 2) + -3 }}").RenderToString());
		}

		[TestMethod]
		public void TestExecIfAnd()
		{
			Assert.AreEqual("A", CompileTemplateCodeByString("{% if 1 && 2 %}A{% endif %}").RenderToString());
		}

		[TestMethod]
		public void TestExecIfOr()
		{
			Assert.AreEqual("A", CompileTemplateCodeByString("{% if 0 || 2 %}A{% endif %}").RenderToString());
		}

		[TestMethod]
		public void TestExecAccessSimple()
		{
			Assert.AreEqual("Value", CompileTemplateCodeByString("{{ Item.Key }}").RenderToString(new TemplateScope(new Dictionary<String, dynamic>()
			{
				{ "Item", new Hashtable() {
					{ "Key", "Value" },
				} }
			})));
		}

		[TestMethod]
		public void TestExecAccessSimpleIndexer()
		{
			Assert.AreEqual("Value", CompileTemplateCodeByString("{{ Item['Key'] }}").RenderToString(new TemplateScope(new Dictionary<String, dynamic>()
			{
				{ "Item", new Hashtable() {
					{ "Key", "Value" },
				} }
			})));
		}

		[TestMethod]
		public void TestExecAccess()
		{
			Assert.AreEqual("Value", CompileTemplateCodeByString("{{ Item.Key.SubKey }}").RenderToString(new TemplateScope(new Dictionary<String, dynamic>()
			{
				{ "Item", new Hashtable() {
					{ "Key", new Hashtable() {
						{ "SubKey", "Value" },
					} }
				} }
			})));
		}

		[TestMethod]
		public void TestExecAccessInexistentKey()
		{
			Assert.AreEqual("", CompileTemplateCodeByString("{{ Item.InexistentKey }}").RenderToString());
		}

		[TestMethod]
		public void TestExecAccessInexistentSubKey()
		{
			Assert.AreEqual("", CompileTemplateCodeByString("{{ Item.Key.InexistentKey }}").RenderToString(new TemplateScope(new Dictionary<String, dynamic>()
			{
				{ "Item", new Hashtable() {
					{ "Key", new Hashtable() {
						{ "SubKey", "Value" },
					} }
				} }
			})));
		}

		[TestMethod]
		public void TestExecFilter()
		{
			Assert.AreEqual("Hello World", CompileTemplateCodeByString("{{ 'Hello {0}'|format('World') }}").RenderToString());
		}

		[TestMethod]
		public void TestExecFilterShouldEscape()
		{
			Assert.AreEqual("Hello &lt;World&gt;", CompileTemplateCodeByString("{{ 'Hello {0}'|format('<World>') }}").RenderToString());
		}

		[TestMethod]
		public void TestExecFilterCombinedRaw()
		{
			Assert.AreEqual("Hello <World>", CompileTemplateCodeByString("{{ 'Hello {0}'|format('<World>')|raw }}").RenderToString());
		}

		[TestMethod]
		public void TestExecAutoescapeOff()
		{
			Assert.AreEqual("Hello <World>", CompileTemplateCodeByString("{% autoescape false %}{{ 'Hello <World>' }}{% endautoescape %}").RenderToString());
		}

		[TestMethod]
		public void TestExecAutoescapeOffEscape()
		{
			Assert.AreEqual("Hello &lt;World&gt;", CompileTemplateCodeByString("{% autoescape false %}{{ 'Hello <World>'|escape }}{% endautoescape %}").RenderToString());
		}

		[TestMethod]
		public void TestExecAutoescapeOnEscape()
		{
			Assert.AreEqual("Hello &lt;World&gt;", CompileTemplateCodeByString("{{ 'Hello <World>'|escape }}").RenderToString());
		}

		[TestMethod]
		public void TestExecAutoescapeOnEscapeShortcut()
		{
			Assert.AreEqual("Hello &lt;World&gt;", CompileTemplateCodeByString("{{ 'Hello <World>'|e }}").RenderToString());
		}

		[TestMethod]
		public void TestExecEscapeTwice()
		{
			Assert.AreEqual("Hello &amp;lt;World&amp;gt;", CompileTemplateCodeByString("{{ 'Hello <World>'|e|e }}").RenderToString());
		}

		[TestMethod]
		public void TestExecFunctionCall()
		{
		}

		[TestMethod]
		public void TestExecBasicInheritance()
		{
			TemplateProviderMemory TemplateProvider = new TemplateProviderMemory();
			TemplateFactory TemplateFactory = new TemplateFactory(TemplateProvider, OutputGeneratedCode: true);

			TemplateProvider.Add("Base.html", "Test{% block Body %}Base{% endblock %}Test");
			TemplateProvider.Add("Test.html", "{% extends 'Base.html' %}Not{% block Body %}Ex{% endblock %}Rendered");

			Assert.AreEqual("TestExTest", TemplateFactory.GetTemplateCodeByFile("Test.html").RenderToString());
		}

		[TestMethod]
		public void TestExecInheritanceWithParent()
		{
			TemplateProviderMemory TemplateProvider = new TemplateProviderMemory();
			TemplateFactory TemplateFactory = new TemplateFactory(TemplateProvider, OutputGeneratedCode: true);

			TemplateProvider.Add("Base.html", "Test{% block Body %}Base{% endblock %}Test");
			TemplateProvider.Add("Test.html", "{% extends 'Base.html' %}Not{% block Body %}1{% parent %}2{% endblock %}Rendered");

			Assert.AreEqual("Test1Base2Test", TemplateFactory.GetTemplateCodeByFile("Test.html").RenderToString());
		}

		[TestMethod]
		[ExpectedException(typeof(TemplateParentOutsideBlockException))]
		public void TestExecInheritanceWithParentOutside()
		{
			TemplateProviderMemory TemplateProvider = new TemplateProviderMemory();
			TemplateFactory TemplateFactory = new TemplateFactory(TemplateProvider, OutputGeneratedCode: true);

			TemplateProvider.Add("Base.html", "Test{% block Body %}Base{% endblock %}Test");
			TemplateProvider.Add("Test.html", "{% extends 'Base.html' %}Not{% block Body %}12{% endblock %}{% parent %}Rendered");

			Assert.AreEqual("Test1Base2Test", TemplateFactory.GetTemplateCodeByFile("Test.html").RenderToString());
		}
	}
}
