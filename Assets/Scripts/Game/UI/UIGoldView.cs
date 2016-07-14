using UnityEngine;
using UnityEngine.UI;

public class UIGoldView : MonoBehaviour
{
	private ComponentCache<Text> text;

	private void Start()
	{
		Core.Instance.GameController.GameProgress.OnGoldChanged += OnGoldChanged;
		OnGoldChanged(Core.Instance.GameController.GameProgress.Gold);
	}

	private void OnGoldChanged(int gold)
	{
		text.GetCache(gameObject).text = "Gold: " + gold;
	}
}
