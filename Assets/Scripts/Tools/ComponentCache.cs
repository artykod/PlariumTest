using UnityEngine;

public struct ComponentCache<T> where T : Object
{
	private T cache;
	private GameObject owner;

	public T GetCache(GameObject owner)
	{
		if (this.owner != owner)
		{
			cache = owner != null ? owner.GetComponent<T>() : null;
			this.owner = owner;
		}

		return cache;
	}
}
