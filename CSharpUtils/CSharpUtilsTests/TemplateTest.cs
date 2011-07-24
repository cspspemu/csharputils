using CSharpUtils.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using CSharpUtils;
using System.Linq;

namespace CSharpUtilsTests
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


        [TestMethod]
        public void TokenizeTest()
        {
            var StringTokens = Template.Tokenize("Hello {{ 'User' + 15 }} Text ").Select(Token => Token.Text).ToArray();
            CollectionAssert.AreEqual(StringTokens, new String[] {
                "Hello ", "{{", "'User'", "+", "15", "}}", " Text ",
            });
        }

        [TestMethod]
        public void Tokenize2Test()
        {
            var StringTokens = Template.Tokenize("Hello {% for n in [1, 2, 3, 4] %}{{ n }}{% endfor %}").Select(Token => Token.Text).ToArray();
            CollectionAssert.AreEqual(StringTokens, new String[] {
                "Hello ", "{%", "for", "n", "in", "[", "1", ",", "2", ",", "3", ",", "4", "]", "%}", "{{", "n", "}}", "{%", "endfor", "%}"
            });
        }
    }
}
