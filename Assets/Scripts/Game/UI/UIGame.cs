using UnityEngine;
using Game.Logics;
using Game.Logics.Characters;
using Game.Logics.Buildings;
using System.Collections.Generic;

public class UIGame : MonoBehaviour
{
	private class UnitUI
	{
		public UIProgressBar hp;
		public UIProgressBar xp;
		public GameObject targetMarker;

		public UnitUI(UIProgressBar hp, UIProgressBar xp, GameObject targetMarker)
		{
			this.hp = hp;
			this.xp = xp;
			this.targetMarker = targetMarker;
		}
	}

	[SerializeField]
	private UIHpBar hpBar;
	[SerializeField]
	private UIXpBar xpBar;
	[SerializeField]
	private GameObject enemyMarker;
	[SerializeField]
	private UIBarracksUpgradePanel barracksUpgradePanel;

	private Dictionary<Unit, UnitUI> unitsUI = new Dictionary<Unit, UnitUI>();
	private Dictionary<Barracks, UIBarracksUpgradePanel> barracksUI = new Dictionary<Barracks, UIBarracksUpgradePanel>();

	private void Start()
	{
		Core.Instance.GameController.OnLogicCreate += OnLogicCreate;
		Core.Instance.GameController.OnLogicDestroy += OnLogicDestroy;
	}

	private void OnLogicCreate(Logic logic)
	{
		var unit = logic as Unit;
		if (unit != null && !unit.IsImmortal)
		{
			var hp = Instantiate(hpBar);
			hp.transform.DropTo(transform);
			hp.transform.SetAsFirstSibling();

			var xp = default(UIProgressBar);
			if (unit is Hero)
			{
				xp = Instantiate(xpBar);
				xp.transform.DropTo(transform);
				xp.transform.SetAsFirstSibling();
			}

			var marker = Instantiate(enemyMarker);
			marker.transform.DropTo(transform);
			marker.transform.SetAsFirstSibling();
			marker.gameObject.SetActive(false);

			unitsUI[unit] = new UnitUI(hp, xp, marker);
		}

		var minionBarracks = logic as MinionBarracks;
		if (minionBarracks != null)
		{
			var barracksPanel = Instantiate(barracksUpgradePanel);
			barracksPanel.DropTo(transform);
			barracksUI[minionBarracks] = barracksPanel;
			barracksPanel.Logic = minionBarracks;
		}
	}

	private void OnLogicDestroy(Logic logic)
	{
		var unit = logic as Unit;
		if (unit != null)
		{
			var ui = default(UnitUI);
			if (unitsUI.TryGetValue(unit, out ui))
			{
				if (ui.hp != null)
				{
					Destroy(ui.hp.gameObject);
				}
				if (ui.xp != null)
				{
					Destroy(ui.xp.gameObject);
				}
				if (ui.targetMarker != null)
				{
					Destroy(ui.targetMarker.gameObject);
				}
				unitsUI.Remove(unit);
			}
		}

		var barracks = logic as Barracks;
		if (barracks != null)
		{
			var ui = default(UIBarracksUpgradePanel);
			if (barracksUI.TryGetValue(barracks, out ui))
			{
				Destroy(ui.gameObject);
				barracksUI.Remove(barracks);
			}
		}
	}

	private void Update()
	{
		foreach (var i in unitsUI)
		{
			var unit = i.Key;
			var ui = i.Value;

			var barsPos = UnitWorldToScreen(unit, 2f);

			if (ui.hp != null)
			{
				ui.hp.transform.position = barsPos;
				ui.hp.SetValue(unit.HP, unit.TotalHP);
			}

			if (ui.xp != null && unit is Hero)
			{
				var hero = unit as Hero;
				ui.xp.SetValue(hero.XP, hero.TotalXP);
			}

			if (ui.xp != null)
			{
				ui.xp.transform.position = barsPos + new Vector3(0f, 15f);
			}

			if (unit is Character)
			{
				var character = unit as Character;
				var isVisible = character.TargetUnit != null && !(character.TargetUnit is MoveTarget) && unit.IsSelected;
				if (ui.targetMarker.activeSelf != isVisible)
				{
					ui.targetMarker.SetActive(isVisible);
				}
				if (isVisible)
				{
					ui.targetMarker.transform.position = UnitWorldToScreen(character.TargetUnit, 2.5f);
				}
			}
		}
	}

	public static Vector3 UnitWorldToScreen(Unit unit, float offsetY)
	{
		return Camera.main.WorldToScreenPoint(new Vector3(unit.Position.x, offsetY, unit.Position.y));
	}
}
