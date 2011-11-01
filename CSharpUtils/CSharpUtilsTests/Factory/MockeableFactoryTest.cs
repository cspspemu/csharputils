using CSharpUtils.Factory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CSharpUtilsTests
{
	[TestClass]
	public class MockeableFactoryTest
	{
		class A
		{
			virtual public String Value
			{
				get
				{
					return "A";
				}
			}
		}

		class B : A
		{
			override public String Value
			{
				get
				{
					return "B";
				}
			}
		}

		[TestMethod]
		public void MockTypeTest()
		{
			var MockeableFactory = new MockeableFactory();
			Assert.AreEqual("A", MockeableFactory.New<A>().Value);
			MockeableFactory.MockType(typeof(A), typeof(B));
			Assert.AreEqual("B", MockeableFactory.New<A>().Value);
		}
	}
}
