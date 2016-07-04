using UnityEngine;

public class PrefabTool
{
	public static T CreateInstance<T>(System.Type prefabType, string prefabName = null) where T : Object
	{
		var prefabPath = prefabType.GetAttribute<PathInResources>();
		if (prefabPath != null)
		{
			if (prefabName == null)
			{
				prefabName = prefabType.Name;
			}

			var prefab = Resources.Load(prefabPath.Path + prefabName);
			if (prefab != null)
			{
				var instance = Object.Instantiate(prefab);
				if (instance is PrefabLinker)
				{
					var obj = (instance as PrefabLinker).CreatedInstance;
					return obj != null ? obj.GetComponent(prefabType) as T : null;
				}
				else
				if (instance is GameObject)
				{
					return (instance as GameObject).GetComponent<T>();
				}
				else
				if (instance is MonoBehaviour)
				{
					return (instance as MonoBehaviour).gameObject.GetComponent<T>();
				}
				else
				{
					return instance as T;
				}
			}
		}

		return null;
	}

	public static T CreateInstance<T>(System.Type prefabType, System.Enum subType) where T : Object
	{
		return CreateInstance<T>(prefabType, subType != null ? subType.ToString() : null);
	}

	public static T CreateInstance<T>(string prefabName = null) where T : MonoBehaviour, new()
	{
		return CreateInstance<T>(typeof(T), prefabName);
	}

	public static T CreateInstance<T>(System.Enum subType) where T : Object
	{
		return CreateInstance<T>(typeof(T), subType);
	}
}