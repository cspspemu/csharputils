using CSharpUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using CSharpUtils.Extensions;

namespace CSharpUtilsTests
{
	[TestClass]
	public class TypeUtilsTest
	{
		public class MyType
		{
		}

		public class MyExtendedType1 : MyType
		{
		}

		public class MyExtendedType2 : MyType
		{
		}

		[TestMethod]
		public void GetTypesExtendingTest()
		{
			/*
			Assert.Equals(
				"",
				TypeUtils.GetTypesExtending(typeof(MyType)).ToStringArray()
			);
			*/
			Assert.Inconclusive();
		}
	}
}
