using UnityEngine;
using Game.Logics;
using Game.Logics.Characters;
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

	private Dictionary<Unit, UnitUI> unitsUI = new Dictionary<Unit, UnitUI>();

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
			hp.transform.SetParent(transform, false);

			var xp = default(UIProgressBar);
			if (unit is Hero)
			{
				xp = Instantiate(xpBar);
				xp.transform.SetParent(transform, false);
			}

			var marker = Instantiate(enemyMarker);
			marker.transform.SetParent(transform, false);
			marker.gameObject.SetActive(false);

			unitsUI[unit] = new UnitUI(hp, xp, marker);
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

	private Vector3 UnitWorldToScreen(Unit unit, float offsetY)
	{
		return Camera.main.WorldToScreenPoint(new Vector3(unit.Position.x, offsetY, unit.Position.y));
	}
}
