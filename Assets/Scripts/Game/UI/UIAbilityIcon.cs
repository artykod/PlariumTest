using System;
using Game.Logics;
using Game.Logics.Abilities;
using Game.Logics.Characters;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Иконка абилки героя.
/// </summary>
public class UIAbilityIcon : UIFillableIcon
{
	[SerializeField]
	private UIButton upgradeButton;
	[SerializeField]
	private Text hotKeyText;
	[SerializeField]
	private KeyCode hotKeyActivation;

	private UIHeroHUD hud;
	private Ability ability;

	public event Action<Ability, bool> OnSelectionChanged;

	public Ability Ability
	{
		get
		{
			return ability;
		}
	}

	public void FetchAbility(UIHeroHUD hud, Ability ability)
	{
		this.hud = hud;

		UnsubscribeFromLogic();
		this.ability = ability;
		SubscribeToLogic();
		OnLevelChanged(ability.Level, ability.Level);

		SetIcon(ResourcesTool.LoadIconByName(ability.Descriptor.IconId));
	}

	public void Cancel()
	{
		if (ability != null && ability.IsSelected)
		{
			ability.Cancel();
		}
	}

	public void Activate(Vec2 point, Unit unit)
	{
		if (ability != null)
		{
			ability.Activate(point, unit);
		}
	}

	protected override void Awake()
	{
		base.Awake();

		hotKeyText.text = "hotkey: " + hotKeyActivation.ToString().Replace("Alpha", "");
		upgradeButton.OnClick += OnUpgradeClicked;
	}

	private void OnUpgradeClicked()
	{
		if (ability != null)
		{
			var heroCaster = ability.Caster as Hero;
			if (heroCaster != null)
			{
				heroCaster.UpgradeAbility(ability);
			}
		}
	}

	protected override float FillAmount
	{
		get
		{
			return ability != null ? ability.CurrentCooldown / ability.TotalCooldown : 0f;
		}
	}

	protected override void OnClick()
	{
		base.OnClick();

		if (ability != null && !ability.IsRecharging && (hud.SelectedAbility == null || !hud.SelectedAbility.IsActivated))
		{
			ability.Select();
			Core.Instance.GameController.SelectUnit(ability.Caster);
		}
	}

	protected override void Update()
	{
		base.Update();

		if (Input.GetKeyDown(hotKeyActivation))
		{
			OnClick();
		}
	}

	private void SubscribeToLogic()
	{
		if (ability != null)
		{
			ability.OnDestroy += OnDestroyAbility;
			ability.OnExecute += OnExecute;
			ability.OnCancel += OnCancel;
			ability.OnSelect += OnSelect;
			ability.OnLevelChanged += OnLevelChanged;
		}
	}

	private void OnDestroyAbility(Logic logic)
	{
		ability.OnDestroy -= OnDestroyAbility;
		OnCancel(ability);
		ability = null;
	}

	private void OnLevelChanged(int oldLvl, int newLvl)
	{
		SetLevel(newLvl);
	}

	private void UnsubscribeFromLogic()
	{
		if (ability != null)
		{
			ability.OnDestroy -= OnDestroyAbility;
			ability.OnExecute -= OnExecute;
			ability.OnCancel -= OnCancel;
			ability.OnSelect -= OnSelect;
			ability.OnLevelChanged -= OnLevelChanged;
		}
	}

	private void OnSelect(Ability obj)
	{
		OnSelectionChanged.SafeInvoke(ability, true);
		Core.Instance.GameCamera.ChangeCursor(ability.Descriptor.CursorId);
	}

	private void OnCancel(Ability obj)
	{
		OnSelectionChanged.SafeInvoke(ability, false);
		Core.Instance.GameCamera.ChangeCursor(null);
	}

	private void OnExecute(Ability obj)
	{
	}
}
