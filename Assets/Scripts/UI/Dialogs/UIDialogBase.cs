using UnityEngine;
using System;

public class UIDialogBase : MonoBehaviour
{
	public event Action<UIDialogBase> OnClose;

	public virtual void Close()
	{
		OnClose.SafeInvoke(this);
		Destroy(gameObject);
	}

	protected virtual void Awake()
	{
	}

	protected virtual void OnDestroy()
	{
	}
}
