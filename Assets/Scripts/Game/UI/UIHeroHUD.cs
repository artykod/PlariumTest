using UnityEngine;
using Game.Logics;
using Game.Logics.Characters;
using Game.Logics.Abilities;

public class UIHeroHUD : MonoBehaviour {
	[SerializeField]
	private UIHeroAvatar heroIcon;
	[SerializeField]
	private UIAbilityIcon[] abilitiesIcons;

	private UnitSelection areaSelection;

	public bool IsAbilitySelected
	{
		get;
		private set;
	}

	public Ability SelectedAbility
	{
		get;
		private set;
	}

	private void Awake()
	{
		foreach (var i in abilitiesIcons)
		{
			i.OnSelectionChanged += OnChangedSelection;
		}

		areaSelection = PrefabTool.CreateInstance<UnitSelection>();
		areaSelection.IsVisible = false;
	}

	private void Update()
	{
		if (areaSelection.IsVisible)
		{
			areaSelection.transform.position = UIGame.ScreenToGroundPosition(Input.mousePosition);
		}
	}

	private void OnChangedSelection(Ability ability, bool isSelected)
	{
		SelectedAbility = ability;
		IsAbilitySelected = isSelected;

		areaSelection.IsVisible = IsAbilitySelected;
		if (IsAbilitySelected)
		{
			areaSelection.Scale = SelectedAbility.CurrentLevel.Radius * 2f;
		}
	}

	public void FetchHero(Hero hero)
	{
		heroIcon.FetchHero(hero);

		for (int i = 0, l = Mathf.Min(abilitiesIcons.Length, hero.Abilities.Length); i < l; i++)
		{
			abilitiesIcons[i].FetchAbility(this, hero.Abilities[i]);
		}
	}

	public void CancelAbility()
	{
		foreach (var i in abilitiesIcons)
		{
			i.Cancel();
		}
	}

	public void ClickOnGround(Vector3 point, Unit[] units)
	{
		var firstUnit = units != null && units.Length > 0 ? units[0] : null;
		foreach (var i in abilitiesIcons)
		{
			if (i.Ability == SelectedAbility)
			{
				i.Activate(new Vec2(point.x, point.z), firstUnit);
				break;
			}
		}
	}
}
