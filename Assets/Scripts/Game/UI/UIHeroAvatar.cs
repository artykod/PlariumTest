using Game.Logics.Characters;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Иконка героя в панели главного персонажа.
/// </summary>
public class UIHeroAvatar : UIFillableIcon
{
	[SerializeField]
	private Text upgradesPointsText;

	private Hero hero;

	public void FetchHero(Hero hero)
	{
		this.hero = hero;
		hero.OnLevelChanged += OnLevelChanged;
		OnLevelChanged(hero.Level, hero.Level);
		SetIcon(ResourcesTool.LoadIconByName(hero.Descriptor.IconId));
	}

	private void OnLevelChanged(int oldLvl, int newLvl)
	{
		SetLevel(newLvl);
	}

	protected override float FillAmount
	{
		get
		{
			var fillAmount = 0f;
			var fountain = Core.Instance.GameController.Map.Fountain;
			if (fountain.Hero == null || fountain.HeroTotalRespawnTime > 0.001f)
			{
				fillAmount = fountain.HeroRespawnTimer / fountain.HeroTotalRespawnTime;
			}
			return fillAmount;
		}
	}

	protected override void OnClick()
	{
		base.OnClick();

		var hero = Core.Instance.GameController.Map.Fountain.Hero;
		if (hero != null)
		{
			Core.Instance.GameCamera.Mode = GameCamera.Modes.FollowMainCharacter;
			Core.Instance.GameController.SelectUnit(hero);
		}
	}

	protected override void Update()
	{
		base.Update();

		if (hero != null)
		{
			upgradesPointsText.text = "points: " + hero.UpgradesPoints.ToString();
		}
	}
}
