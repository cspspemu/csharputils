using System.Collections.Generic;
using CSharpUtils.Json;
using NUnit.Framework;

namespace CSharpUtilsTests
{
	[TestFixture]
	public class JSONTest
	{
		sealed class TestJsonSerializable : IJsonSerializable
		{
			public string ToJson()
			{
				return "TestJsonSerializable(123)";
			}
		}

		[Test]
		public void StringifyNullTest()
		{
			Assert.AreEqual("null", JSON.Stringify(null));
		}

		[Test]
		public void StringifyStringTest()
		{
			Assert.AreEqual("\"Hello World!\"", JSON.Stringify("Hello World!"));
		}

		[Test]
		public void StringifyBooleanTest()
		{
			Assert.AreEqual("true", JSON.Stringify(true));
			Assert.AreEqual("false", JSON.Stringify(false));
		}

		[Test]
		public void StringifyIntTest()
		{
			Assert.AreEqual("777", JSON.Stringify(777));
		}

		[Test]
		public void StringifyDoubleTest()
		{
			Assert.AreEqual("777.777", JSON.Stringify(777.777));
		}

		[Test]
		public void StringifyStringArrayTest()
		{
			Assert.AreEqual("[\"a\",\"b\",\"c\",\"d\"]", JSON.Stringify(new string[] { "a", "b", "c", "d" }));
		}

		[Test]
		public void StringifyIntArrayTest()
		{
			Assert.AreEqual("[1,2,3,4]", JSON.Stringify(new int[] { 1, 2, 3, 4 }));
		}

		[Test]
		public void StringifyDictionaryTest()
		{
			Assert.AreEqual("{\"one\":1,\"two\":2,\"three\":3,\"four\":4}", JSON.Stringify(new Dictionary<string, int>
			{
				{ "one", 1 },
				{ "two", 2 },
				{ "three", 3 },
				{ "four", 4 },
			}));
		}

		[Test]
		public void StringifyJsonSerializableTest()
		{
			Assert.AreEqual(
				"TestJsonSerializable(123)",
				JSON.Stringify(new TestJsonSerializable())
			);
		}
	}
}
