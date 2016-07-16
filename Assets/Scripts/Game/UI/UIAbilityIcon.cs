using System;
using Game.Logics;
using Game.Logics.Abilities;

public class UIAbilityIcon : UIFillableIcon
{
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

		if (!hud.IsAbilitySelected && ability != null && !ability.IsRecharging)
		{
			ability.Select();
		}
	}

	private void SubscribeToLogic()
	{
		if (ability != null)
		{
			ability.OnExecute += OnExecute;
			ability.OnCancel += OnCancel;
			ability.OnSelect += OnSelect;
		}
	}

	private void UnsubscribeFromLogic()
	{
		if (ability != null)
		{
			ability.OnExecute -= OnExecute;
			ability.OnCancel -= OnCancel;
			ability.OnSelect -= OnSelect;
		}
	}

	private void OnSelect(Ability obj)
	{
		OnSelectionChanged.SafeInvoke(ability, true);
		Debug.Log("Select ability " + ability.Descriptor.Name);

		Core.Instance.Camera.ChangeCursor(ability.Descriptor.CursorId);
	}

	private void OnCancel(Ability obj)
	{
		OnSelectionChanged.SafeInvoke(ability, false);
		Debug.Log("Cancel ability " + ability.Descriptor.Name);

		Core.Instance.Camera.ChangeCursor(null);
	}

	private void OnExecute(Ability obj)
	{
		Debug.Log("Execute ability " + ability.Descriptor.Name);
	}
}
