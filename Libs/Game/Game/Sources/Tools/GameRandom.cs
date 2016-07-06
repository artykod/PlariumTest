using System;

public class GameRandom
{
	private static Random random = new Random();

	public static float value
	{
		get
		{
			return (float)random.NextDouble();
		}
	}

	public static int Range(int min, int max)
	{
		return random.Next(min, max);
	}

	public static float Range(float min, float max)
	{
		return min + value * (max - min);
	}

	public static T FromArray<T>(T[] array)
	{
		return array != null ? array[Range(0, array.Length)] : default(T);
	}
}