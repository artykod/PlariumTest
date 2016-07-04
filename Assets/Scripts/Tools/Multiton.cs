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

	protected virtual InstanceType SetInstance(KeyType id, InstanceType instance)
	{
		allInstances[id] = instance;
		return instance;
	}
}
