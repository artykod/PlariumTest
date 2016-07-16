using UnityEngine;

public abstract class SingletonBehaviour<TypeBase, TypeImpl> : MonoBehaviour where TypeBase : MonoBehaviour where TypeImpl : TypeBase
{
	private static TypeBase instance;

	public static TypeBase Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new GameObject().AddComponent<TypeImpl>();
				var singletonBehaviour = instance as SingletonBehaviour<TypeBase, TypeImpl>;
				if (singletonBehaviour != null)
				{
					instance.gameObject.name = singletonBehaviour.SingletonName;
				}
			}
			return instance;
		}
	}

	public virtual string SingletonName
	{
		get
		{
			return GetType().ToString();
		}
	}

	public void Create()
	{
	}
}
