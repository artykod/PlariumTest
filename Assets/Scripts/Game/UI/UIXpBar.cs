using UnityEngine;

public class UIXpBar : UIProgressBar
{
	protected override Color Color
	{
		get
		{
			return Color.yellow;
		}
	}

	protected override Color TextColor
	{
		get
		{
			return Color.red;
		}
	}
}
