using UnityEngine;
using UnityEngine.UI;

public class UIGoldView : MonoBehaviour
{
	private Text text;

	private void Awake()
	{
		text = GetComponent<Text>();
	}

	private void Start()
	{
		Core.Instance.GameController.GameProgress.OnGoldChanged += OnGoldChanged;
		OnGoldChanged(Core.Instance.GameController.GameProgress.Gold);
	}

	private void OnGoldChanged(int gold)
	{
		text.text = "Gold: " + gold;
	}
}
