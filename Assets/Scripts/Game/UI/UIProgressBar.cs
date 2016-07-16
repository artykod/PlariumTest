using UnityEngine;
using UnityEngine.UI;

public class UIProgressBar : MonoBehaviour
{
	[SerializeField]
	private Slider progressBar;
	[SerializeField]
	private Image progressLine;
	[SerializeField]
	private Text valueText;

	protected virtual Color Color
	{
		get
		{
			return Color.white;
		}
	}

	protected virtual Color TextColor
	{
		get
		{
			return Color.black;
		}
	}

	public void SetValue(int current, int total)
	{
		var progress = total > 0 ? (float)current / total : 1f;
		progressBar.normalizedValue = progress;
		valueText.text = total > 0 ? string.Format("{0}/{1}", current, total) : "Maximum";
	}

	protected virtual void Awake()
	{
		progressLine.color = Color;
		valueText.color = TextColor;
	}
}
