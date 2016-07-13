using UnityEngine;
using UnityEngine.UI;

public class UIHeroAvatar : MonoBehaviour
{
	[SerializeField]
	private Image avatarTonerImage;

	private void Update()
	{
		var fillAmount = 0f;
		var fountain = Core.Instance.GameController.Map.Fountain;
		if (fountain.Hero == null || fountain.HeroTotalRespawnTime > 0.001f)
		{
			fillAmount = fountain.HeroRespawnTimer / fountain.HeroTotalRespawnTime;
		}
		avatarTonerImage.fillAmount = fillAmount;
	}
}
