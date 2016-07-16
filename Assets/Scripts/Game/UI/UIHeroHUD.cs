using UnityEngine;
using Game.Logics;
using Game.Logics.Characters;
using Game.Logics.Abilities;

/// <summary>
/// Панель главного персонажа.
/// </summary>
public class UIHeroHUD : MonoBehaviour {
	[SerializeField]
	private UIHeroAvatar heroIcon;
	[SerializeField]
	private UIAbilityIcon[] abilitiesIcons;

	private UnitSelection areaSelection;

	public bool IsAbilitySelected
	{
		get
		{
			return SelectedAbility != null;
		}
	}

	/// <summary>
	/// Выбранная в текущий момент абилка.
	/// Если null, то ни одна абилка не выбрана.
	/// </summary>
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
		var prev = SelectedAbility;
		if (isSelected && prev != null)
		{
			prev.Cancel();
		}

		SelectedAbility = isSelected ? ability : null;
		areaSelection.IsVisible = IsAbilitySelected;
		if (IsAbilitySelected)
		{
			if (!string.IsNullOrEmpty(SelectedAbility.Descriptor.CursorId))
			{
				areaSelection.IsVisible = false;
			}
			areaSelection.Scale = SelectedAbility.CurrentLevel.Radius * 2f;
		}
		else
		{
			SelectedAbility = null;
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

	/// <summary>
	/// Передать абилка нажатие по полю.
	/// </summary>
	/// <param name="point">в какой точке поля нажали.</param>
	/// <param name="units">были ли под курсором юниты.</param>
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
