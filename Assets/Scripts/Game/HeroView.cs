using Game.Logics;
using UnityEngine;
using System.Collections;

public class HeroView : CharacterView
{
	[SerializeField]
	private GameObject levelUpEffect;

	public override void FetchLogic(Logic logic)
	{
		base.FetchLogic(logic);

		Logic.OnLevelChanged += OnLevelChanged;
	}

	private void OnLevelChanged(int prevLevel, int newLevel)
	{
		if (newLevel - prevLevel == 1)
		{
			var effect = Instantiate(levelUpEffect);
			effect.DropTo(transform);
			StartCoroutine(DestroyAfterDelay(effect, 3f));
		}
	}

	private IEnumerator DestroyAfterDelay(GameObject obj, float delay)
	{
		yield return new WaitForSeconds(delay);
		if (obj != null)
		{
			Destroy(obj);
		}
	}
}
