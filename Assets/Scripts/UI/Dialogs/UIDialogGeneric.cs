using UnityEngine;

public abstract class UIDialogGeneric<T> : UIDialogBase where T : UIDialogBase
{
	public static T CurrentDialog
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

		var prefab = Resources.Load<T>("UI/Dialogs/" + typeof(T).Name);
		if (prefab != null)
		{
			CurrentDialog = Instantiate(prefab);
		}

		return CurrentDialog;
	}

	public static void DestroyCurrent()
	{
		if (CurrentDialog != null)
		{
			var current = CurrentDialog;
			CurrentDialog = null;
			Destroy(current.gameObject);
		}
	}

	public static void CloseCurrent()
	{
		if (CurrentDialog != null)
		{
			var current = CurrentDialog;
			CurrentDialog = null;
			current.Close();
		}
	}
}
