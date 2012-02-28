using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CSharpUtils
{
	[AttributeUsage(AttributeTargets.Field)]
	public class EndianAttribute : Attribute
	{
		public Endianness Endianness { get; private set; }

		public EndianAttribute(Endianness endianness)
		{
			this.Endianness = endianness;
		}
	}

	public class StructUtils
	{
		static public void ExpectSize<T>(int ExpectedSize)
		{
			int RealSize = Marshal.SizeOf(typeof(T));
			if (RealSize != ExpectedSize)
			{
				throw (new Exception("Expecting struct '" + typeof(T).FullName + "' size. Expected(" + ExpectedSize + ") but Obtained(" + RealSize + ")."));
			}
		}

		public static void RespectEndianness(Type type, byte[] data)
		{
			var fields = type.GetFields().Where(f => f.IsDefined(typeof(EndianAttribute), false))
				.Select(f => new
				{
					Field = f,
					Attribute = (EndianAttribute)f.GetCustomAttributes(typeof(EndianAttribute), false)[0],
					Offset = Marshal.OffsetOf(type, f.Name).ToInt32()
				}).ToList();

			foreach (var field in fields)
			{
				if ((field.Attribute.Endianness == Endianness.BigEndian && BitConverter.IsLittleEndian) ||
					(field.Attribute.Endianness == Endianness.LittleEndian && !BitConverter.IsLittleEndian))
				{
					Array.Reverse(data, field.Offset, Marshal.SizeOf(field.Field.FieldType));
				}
			}
		}

		public static T BytesToStruct<T>(byte[] rawData) where T : struct
		{
			T result = default(T);

			RespectEndianness(typeof(T), rawData);

			GCHandle handle = GCHandle.Alloc(rawData, GCHandleType.Pinned);

			try
			{
				IntPtr rawDataPtr = handle.AddrOfPinnedObject();
				result = (T)Marshal.PtrToStructure(rawDataPtr, typeof(T));
			}
			finally
			{
				handle.Free();
			}

			return result;
		}

		unsafe public static T[] BytesToStructArray<T>(byte[] RawData) where T : struct
		{
			int ElementSize = Marshal.SizeOf(typeof(T));
			T[] Array = new T[RawData.Length / ElementSize];
			var Type = typeof(T);
			fixed (byte* RawDataPointer = &RawData[0])
			{
				for (int n = 0; n < Array.Length; n++)
				{
					Marshal.PtrToStructure(new IntPtr(RawDataPointer + n * ElementSize), Type);
				}
			}
			return Array;
			//T[] Array = 
			//Marshal.PtrToStructure(
		}

		public static byte[] StructToBytes<T>(T data) where T : struct
		{
			byte[] rawData = new byte[Marshal.SizeOf(data)];
			GCHandle handle = GCHandle.Alloc(rawData, GCHandleType.Pinned);
			try
			{
				IntPtr rawDataPtr = handle.AddrOfPinnedObject();
				Marshal.StructureToPtr(data, rawDataPtr, false);
			}
			finally
			{
				handle.Free();
			}

			RespectEndianness(typeof(T), rawData);

			return rawData;
		}

		public static byte[] StructArrayToBytes<T>(T[] dataArray) where T : struct
		{
			int ElementSize = Marshal.SizeOf(dataArray[0]);
			byte[] rawData = new byte[ElementSize * dataArray.Length];
			GCHandle handle = GCHandle.Alloc(rawData, GCHandleType.Pinned);
			try
			{
				for (int n = 0; n < dataArray.Length; n++)
				{
					IntPtr rawDataPtr = handle.AddrOfPinnedObject() + ElementSize * n;
					Marshal.StructureToPtr(dataArray[n], rawDataPtr, false);
				}
			}
			finally
			{
				handle.Free();
			}

			//RespectEndianness(typeof(T), rawData);

			return rawData;
		}
	}
}
