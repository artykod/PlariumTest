using UnityEngine;

public abstract class UIDialogGeneric<T> : UIDialogBase where T : UIDialogBase
{
	public static T CurrentInstance
	{
		get;
		private set;
	}

	public static T Show(bool singleInstance = false)
	{
		if (singleInstance)
		{
			DestroyCurrent();
		}

		var type = typeof(T);
		var pathAttribute = type.GetAttribute<PathInResources>();
		var prefabName = pathAttribute != null ? pathAttribute.Path : type.Name;
		var prefab = Resources.Load<T>("UI/Dialogs/" + prefabName);
		if (prefab != null)
		{
			CurrentInstance = Instantiate(prefab);
		}

		return CurrentInstance;
	}

	public static void DestroyCurrent()
	{
		if (CurrentInstance != null)
		{
			var current = CurrentInstance;
			CurrentInstance = null;
			Destroy(current.gameObject);
		}
	}

	public static void CloseCurrent()
	{
		if (CurrentInstance != null)
		{
			var current = CurrentInstance;
			CurrentInstance = null;
			current.Close();
		}
	}
}
