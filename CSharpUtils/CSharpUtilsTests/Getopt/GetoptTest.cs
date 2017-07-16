using System.Collections.Generic;
using CSharpUtils.Getopt;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpUtilsTests
{
    [TestClass]
    public class GetoptTest
    {
        [TestMethod]
        public void AddRuleTest()
        {
            var booleanValue = false;
            var integerValue = 0;
            var getopt = new Getopt(new string[] {"-b", "-i", "50"});
            getopt.AddRule("-b", ref booleanValue);
            getopt.AddRule("-i", ref integerValue);
            getopt.Process();

            Assert.AreEqual(true, booleanValue);
            Assert.AreEqual(50, integerValue);
        }

        [TestMethod]
        public void AddRule2Test()
        {
            var booleanValue = false;
            var integerValue = 0;
            var stringValue = "";
            var getopt = new Getopt(new string[] {"-b", "-i", "50", "-s", "hello_world"});
            getopt.AddRule("-b", (bool value) => { booleanValue = value; });
            getopt.AddRule("-i", (int value) => { integerValue = value; });
            getopt.AddRule("-s", (string value) => { stringValue = value; });
            getopt.Process();

            Assert.AreEqual(true, booleanValue);
            Assert.AreEqual(50, integerValue);
            Assert.AreEqual("hello_world", stringValue);
        }

        [TestMethod]
        public void AddRule3Test()
        {
            var values = new List<int>();
            var getopt = new Getopt(new[] {"-i=50", "-i=25"});
            getopt.AddRule("-i", (int value) => { values.Add(value); });
            getopt.Process();
            Assert.AreEqual("50,25", values.ToStringArray());
        }

        [TestMethod]
        public void AddRule4Test()
        {
            var executedCount = 0;
            var getopt = new Getopt(new[] {"-a", "-a"});
            getopt.AddRule("-a", () => { executedCount++; });
            getopt.Process();
            Assert.AreEqual(2, executedCount);
        }
    }
}