using CSharpUtils.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace CSharpUtilsTests
{
    [TestClass]
    public class TemplateTest
    {
        [TestMethod]
        public void ParseTest()
        {
            Assert.AreEqual("Hello Test", Template.ParseFromString("Hello {{ 'User' }}").RenderToString(new Dictionary<String, String>() {
                { "User", "Test" },
            }));
        }
    }
}
