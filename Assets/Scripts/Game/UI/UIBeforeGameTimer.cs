using UnityEngine;
using UnityEngine.UI;

public class UIBeforeGameTimer : MonoBehaviour
{
	private ComponentCache<Text> text;

	private void Update()
	{
		var txt = text.GetCache(gameObject);
		var gameController = Core.Instance.GameController;
		var timer = gameController.BeforeGameTime;
		if (gameController.IsRunned && timer > 0f)
		{
			txt.enabled = true;
			txt.text = timer >= 1f ? ((int)gameController.BeforeGameTime).ToString() : "FIGHT!";
		}
		else
		{
			txt.enabled = false;
		}
	}
}
