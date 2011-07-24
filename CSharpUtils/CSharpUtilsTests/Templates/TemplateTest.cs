using CSharpUtils.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using CSharpUtils;
using System.Linq;

namespace CSharpUtilsTests.Templates
{
    [TestClass]
    public class TemplateTest
    {
        [TestMethod]
        public void TestExecVariableSimple()
        {
            Assert.AreEqual("Hello Test", Template.ParseFromString("Hello {{ User }}").RenderToString(new Dictionary<String, object>() {
                { "User", "Test" },
            }));
        }

        [TestMethod]
        public void TestExecFor()
        {
            Assert.AreEqual("1234", Template.ParseFromString("{% for Item in List %}{{ Item }}{% endfor %}").RenderToString(new Dictionary<String, object>() {
                { "List", new int[] { 1, 2, 3, 4 } },
            }));
        }

        [TestMethod]
        public void TestExecForRange()
        {
            Assert.AreEqual("0123456789", Template.ParseFromString("{% for Item in 0..9 %}{{ Item }}{% endfor %}").RenderToString());
        }

        [TestMethod]
        public void TestExecWithOperator()
        {
            Assert.AreEqual("Hello 3 World", Template.ParseFromString("Hello {{ 1 + 2 }} World").RenderToString());
        }

        [TestMethod]
        public void TestExecWithOperatorPrecedence()
        {
            Assert.AreEqual("Hello 5 World", Template.ParseFromString("Hello {{ 1 + 2 * 2 }} World").RenderToString());
        }

        [TestMethod]
        public void TestExecIfElseCond0()
        {
            Assert.AreEqual("B", Template.ParseFromString("{% if 0 %}A{% else %}B{% endif %}").RenderToString());
        }

        [TestMethod]
        public void TestExecIfElseCond1()
        {
            Assert.AreEqual("A", Template.ParseFromString("{% if 1 %}A{% else %}B{% endif %}").RenderToString());
        }

        protected void TokenizerAssertEquals(String Base, params String[] Tokens)
        {
            var StringTokens = Template.Tokenize(Base).Select(Token => Token.Text).ToArray();
            foreach (var StringToken in StringTokens)
            {
                Console.WriteLine(StringToken);
            }
            CollectionAssert.AreEqual(StringTokens, Tokens);
        }

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
    }
}
