using Game.Logics.Characters;

public class UIHeroAvatar : UIFillableIcon
{
	public void FetchHero(Hero hero)
	{
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
			Core.Instance.Camera.Mode = GameCamera.Modes.FollowMainCharacter;
		}
	}
}
