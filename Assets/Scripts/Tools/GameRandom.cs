using UnityEngine;

public class GameRandom
{
	public static float value
	{
		get
		{
			return Random.value;
		}
	}

	public static int Range(int min, int max)
	{
		return Random.Range(min, max);
	}

	public static float Range(float min, float max)
	{
		return Random.Range(min, max);
	}

	public static T FromArray<T>(T[] array)
	{
		return array != null ? array[Range(0, array.Length)] : default(T);
	}
}