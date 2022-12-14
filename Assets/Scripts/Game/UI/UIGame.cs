using UnityEngine;
using Game.Logics;
using Game.Logics.Characters;
using Game.Logics.Buildings;
using System.Collections.Generic;

/// <summary>
/// Игровой интерфейс на поле боя.
/// </summary>
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
	[SerializeField]
	private RectTransform gameHUDRoot;
	[SerializeField]
	private UIHeroHUD heroHUD;

	private static Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
	private Dictionary<Unit, UnitUI> unitsUI = new Dictionary<Unit, UnitUI>();
	private Dictionary<Barracks, UIBarracksUpgradePanel> barracksUI = new Dictionary<Barracks, UIBarracksUpgradePanel>();

	public UIHeroHUD HeroHUD
	{
		get
		{
			return heroHUD;
		}
	}

	public static Vector3 UnitWorldToScreen(Unit unit, float offsetY)
	{
		return Camera.main.WorldToScreenPoint(new Vector3(unit.Position.x, offsetY, unit.Position.y));
	}

	public static Vector3 ScreenToGroundPosition(Vector2 screenPosition)
	{
		var rayToWorld = Camera.main.ScreenPointToRay(screenPosition);
		var rayDistance = 0f;
		if (groundPlane.Raycast(rayToWorld, out rayDistance))
		{
			return rayToWorld.GetPoint(rayDistance);
		}
		return Vector3.zero;
	}

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
			hp.transform.DropTo(gameHUDRoot);
			hp.transform.SetAsFirstSibling();

			var xp = default(UIProgressBar);
			if (unit is Hero)
			{
				xp = Instantiate(xpBar);
				xp.transform.DropTo(gameHUDRoot);
				xp.transform.SetAsFirstSibling();
			}

			var marker = Instantiate(enemyMarker);
			marker.transform.DropTo(gameHUDRoot);
			marker.transform.SetAsFirstSibling();
			marker.gameObject.SetActive(false);

			unitsUI[unit] = new UnitUI(hp, xp, marker);
		}

		var minionBarracks = logic as MinionBarracks;
		if (minionBarracks != null)
		{
			var barracksPanel = Instantiate(barracksUpgradePanel);
			barracksPanel.DropTo(gameHUDRoot);
			barracksUI[minionBarracks] = barracksPanel;
			barracksPanel.Logic = minionBarracks;
		}

		var hero = logic as Hero;
		if (hero != null)
		{
			heroHUD.FetchHero(hero);
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

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (heroHUD.IsAbilitySelected)
			{
				heroHUD.CancelAbility();
			}
			else
			{
				if (UIDialogBase.CurrentDialog == null)
				{
					UIDialogGameMenu.Show().Build("Main menu", "")
						.AddButton("Surrender", Core.Instance.GameController.Surrender)
						.AddButton("Add 1000 gold", AddManyCoins)
						.AddButton("Level up hero", LevelUpHero)
						.AddButton("Clear all progress and quit", ClearAllProgressAndQuit)
						.AddButton("Quit", Application.Quit);
				}
				else if (UIDialogGameMenu.CurrentInstance != null)
				{
					UIDialogGameMenu.CurrentInstance.Close();
				}
			}
		}
	}

	private void AddManyCoins()
	{
		Core.Instance.GameController.GameProgress.AddGold(1000);
	}

	private void LevelUpHero()
	{
		var hero = Core.Instance.GameController.Map.Fountain.Hero;
		if (hero != null)
		{
			hero.AddXP(hero.TotalXP - hero.XP);
		}
	}

	private void ClearAllProgressAndQuit()
	{
		PlayerPrefs.DeleteAll();
		PlayerPrefs.Save();
		Application.Quit();
	}
}
