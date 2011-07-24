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
		public void TokenizeRange()
		{
			TokenizerAssertEquals(
				"{% for n in 0..10 %}",
				"{%", "for", "n", "in", "0", "..", "10", "%}"
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
