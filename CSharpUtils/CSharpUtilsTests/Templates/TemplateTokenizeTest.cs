using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSharpUtils.Templates.Tokenizers;

namespace CSharpUtilsTests.Templates
{
	/// <summary>
	/// Descripción resumida de TemplateTokenizeTest
	/// </summary>
	[TestClass]
	public class TemplateTokenizeTest
	{
		[TestMethod]
		public void TokenizeTest()
		{
			TokenizerAssertEquals(
				"Hello {{ 'User' + 15 }} Text ",
				"Hello ", "{{", "'User'", "+", "15", "}}", " Text "
			);
		}

		[TestMethod]
		public void Tokenize2Test()
		{
			TokenizerAssertEquals(
				"Hello {% for n in [1, 2, 3, 4] %}{{ n }}{% endfor %}",
				"Hello ", "{%", "for", "n", "in", "[", "1", ",", "2", ",", "3", ",", "4", "]", "%}", "{{", "n", "}}", "{%", "endfor", "%}"
			);
		}

		[TestMethod]
		public void TokenizeBinary()
		{
			TokenizerAssertEquals(
				"{{ 1 + 2 * 3 / 4 % 5 // 6 ** 7 }}",
				"{{", "1", "+", "2", "*", "3", "/", "4", "%", "5", "//", "6", "**", "7", "}}"
			);
		}

		[TestMethod]
		public void TokenizeTernary()
		{
			TokenizerAssertEquals(
				"{{ 1 ? 1 : 0 }}",
				"{{", "1", "?", "1", ":", "0", "}}"
			);
		}

		[TestMethod]
		public void TokenizeRange()
		{
			TokenizerAssertEquals(
				"{% for n in 0..10 %}",
				"{%", "for", "n", "in", "0", "..", "10", "%}"
			);
		}

		[TestMethod]
		public void TokenizeUnary()
		{
			TokenizerAssertEquals(
				"{{ -(1 + 2) + -3  }}",
				"{{", "-", "(", "1", "+", "2", ")", "+", "-", "3", "}}"
			);
		}

		[TestMethod]
		public void TokenizeAccessDot()
		{
			TokenizerAssertEquals(
				"{{ test.key.subkey }}",
				"{{", "test", ".", "key", ".", "subkey", "}}"
			);
		}

		[TestMethod]
		public void TokenizeComments()
		{
			TokenizerAssertEquals(
				"Hello {# World #}{{ test }}A{# Test #}B",
				"Hello ", "{{", "test", "}}", "A", "B"
			);
		}

		protected void TokenizerAssertEquals(String TemplateString, params String[] Tokens)
		{
			var StringTokens = TemplateTokenizer.Tokenize(new TokenizerStringReader(TemplateString)).Select(Token => Token.Text).ToArray();
			foreach (var StringToken in StringTokens)
			{
				Console.WriteLine(StringToken);
			}
			CollectionAssert.AreEqual(StringTokens, Tokens);
		}
	}
}
