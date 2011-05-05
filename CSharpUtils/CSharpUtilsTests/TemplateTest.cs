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
        public void ParseTest()
        {
            Assert.AreEqual("Hello Test", Template.ParseFromString("Hello {{ User }}").RenderToString(new Dictionary<String, String>() {
                { "User", "Test" },
            }));
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
