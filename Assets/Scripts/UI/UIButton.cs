using UnityEngine;
using UnityEngine.UI;

public class UIButton : Button
{
	[SerializeField]
	private Text text;

	public event System.Action OnClick;

	public string Text
	{
		get
		{
			if (text != null)
			{
				return text.text;
			}

			return "";
		}
		set
		{
			if (text != null)
			{
				text.text = value;
			}
		}
	}

	protected override void Awake()
	{
		base.Awake();

		onClick.AddListener(OnClickHandler);

		if (text == null)
		{
			text = GetComponentInChildren<Text>();
		}
	}

	protected virtual void OnClickHandler()
	{
		OnClick.SafeInvoke();
	}
}
