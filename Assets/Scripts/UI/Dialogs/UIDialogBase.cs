using UnityEngine;
using System;

[PathInResources("DialogBase")]
public class UIDialogBase : MonoBehaviour
{
	public static UIDialogBase CurrentDialog
	{
		get;
		private set;
	}

	public event Action<UIDialogBase> OnClose;

	public virtual void Close()
	{
		OnClose.SafeInvoke(this);
		Destroy(gameObject);
	}

	protected virtual void Awake()
	{
		CurrentDialog = this;
	}

	protected virtual void OnDestroy()
	{
		if (CurrentDialog == this)
		{
			CurrentDialog = null;
		}
	}
}
