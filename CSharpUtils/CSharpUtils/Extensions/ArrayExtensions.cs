using System;
using System.Collections.Generic;

public static class ArrayExtensions
{
	public static T[] Concat<T>(this T[] Left, T[] Right)
	{
		var Return = new T[Left.Length + Right.Length];
		Left.CopyTo(Return, 0);
		Right.CopyTo(Return, Left.Length);
		return Return;
	}

	public static T[] Concat<T>(this T[] Left, T[] Right, int RightOffset, int RightLength)
	{
		var Return = new T[Left.Length + RightLength];
		Array.Copy(Left, 0, Return, 0, Left.Length);
		Array.Copy(Right, RightOffset, Return, Left.Length, RightLength);
		return Return;
	}

	public static T[] SliceWithBounds<T>(this T[] This, int Low, int High)
	{
		if (High < 0)
		{
			return This.Slice(Low, This.Length + High - Low);
		}
		else
		{
			return This.Slice(Low, High - Low);
		}
	}

	public static T[] Slice<T>(this T[] This, int Start, int Length)
	{
		if (Start < 0) Start = This.Length - Start;
		Start = Math.Min(Math.Max(0, Start), This.Length);
		Length = Math.Min(This.Length - Start, Length);
		var Return = new T[Length];
		Array.Copy(This, Start, Return, 0, Length);
		return Return;
	}

	public static T[] Slice<T>(this T[] This, int Start)
	{
		return This.Slice(Start, This.Length - Start);
	}

	public static IEnumerable<T[]> Split<T>(this T[] This, int ChunkSize)
	{
		int CompleteChunksCount = This.Length / ChunkSize;
		int PartialChunkSize = This.Length % ChunkSize;
		int Offset = 0;
		for (int n = 0; n < CompleteChunksCount; n++)
		{
			var Chunk = new T[PartialChunkSize];
			Array.Copy(This, Offset, Chunk, 0, CompleteChunksCount); Offset += CompleteChunksCount;
			yield return Chunk;
		}
		if (PartialChunkSize > 0)
		{
			var Chunk = new T[PartialChunkSize];
			Array.Copy(This, Offset, Chunk, 0, PartialChunkSize); Offset += PartialChunkSize;
			//Chunk.CopyTo);
			yield return Chunk;
		}
	}
}
