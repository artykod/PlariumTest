using System;
using System.Collections.Generic;

public class Multiton<KeyType, InstanceType> where InstanceType : class
{
	private static Dictionary<KeyType, InstanceType> allInstances = new Dictionary<KeyType, InstanceType>();

	public static void Clear()
	{
		allInstances = new Dictionary<KeyType, InstanceType>();
	}

	public static InstanceType GetInstance(KeyType id)
	{
		InstanceType result = default(InstanceType);
		allInstances.TryGetValue(id, out result);
		return result;
	}

	public static T GetInstance<T>(KeyType id) where T : class
	{
		return GetInstance(id) as T;
	}

	public static void ForEach(Action<InstanceType> func)
	{
		foreach (var i in allInstances)
		{
			try
			{
				func(i.Value);
			} catch (Exception e) {
				Debug.LogException(e);
			}
		}
	}

	protected virtual InstanceType SetInstance(KeyType id, InstanceType instance)
	{
		allInstances[id] = instance;
		return instance;
	}
}
