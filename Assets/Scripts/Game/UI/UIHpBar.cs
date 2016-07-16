using UnityEngine;

public class UIHpBar : UIProgressBar
{
	protected override Color Color
	{
		get
		{
			return Color.red;
		}
	}

	protected override Color TextColor
	{
		get
		{
			return Color.yellow;
		}
	}
}
