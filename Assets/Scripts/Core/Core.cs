using System.Collections.Generic;
using UnityEngine;
using Game;
using Game.Logics;
using Game.Logics.Maps;
using Game.Logics.Abilities;

public class Core : MonoBehaviour
{
	[SerializeField]
	private GameCamera gameCamera;
	[SerializeField]
	private UIGame gameUI;

	private Loader loader;
	private GameController gameController;
	private Dictionary<Logic, View> logicToViewMap = new Dictionary<Logic, View>();

	private static Core instance;

	public static Core Instance
	{
		get
		{
			return instance;
		}
	}

	public GameController GameController
	{
		get
		{
			return gameController;
		}
	}

	public GameCamera Camera
	{
		get
		{
			return gameCamera;
		}
	}

	public UIGame GameUI
	{
		get
		{
			return gameUI;
		}
	}

	public View FindViewForLogic(Logic logic)
	{
		View result = null;
		logicToViewMap.TryGetValue(logic, out result);
		return result;
	}

	private void Awake()
	{
		instance = this;

		DebugImpl.Instance = new DebugUnity();
		TimeControllerImpl.Instance = new TimeControllerUnity();
		StorageImpl.Instance = new StorageUnity();

		DebugConsole.Instance.Create();

		var jsons = default(string[]);
		try
		{
			jsons = new Loader().LoadDescriptorsFromExternalFiles(Constants.Paths.Descriptors.ALL);
			if (jsons.Length < 1)
			{
				throw new System.Exception();
			}
		}
		catch
		{
			Debug.LogWarning("Cannot find descriptors in external storage. Use built-in data.");
			jsons = new Loader().LoadDescriptorsFromGameResources(Constants.Paths.Descriptors.ALL);
		}

		gameController = new GameController(jsons);
		gameController.OnLogicCreate += OnLogicCreate;
		gameController.OnLogicDestroy += OnLogicDestroy;
		gameController.OnGameEnd += OnGameEnd;

		StartNewBattle();
	}

	private void OnGameEnd(bool isPlayerWin)
	{
		var message = isPlayerWin ? "All enemies died! You win" : "Sofa of Developers destroyed! You lose";
		UIDialogText.Show().Build("Game Over", message)
			.AddButton("Retry", StartNewBattle)
			.AddButton("Quit", Application.Quit);
	}

	private void StartNewBattle()
	{
		gameController.RunWithMapId("map.forest");
		gameController.Map.Fountain.FetchHeroId("unit.main_character");
	}

	private void Update()
	{
		TimeController.Update();
	}

	private void OnDestroy()
	{
		instance = null;
	}

	private void OnLogicCreate(Logic logic)
	{
		var viewType = typeof(View);
		var viewId = string.Empty;

		if (logic is Unit)
		{
			var unit = logic as Unit;
			viewId = unit.Descriptor.Levels[unit.Level].ViewId;
			viewType = typeof(UnitView);
		}
		else if (logic is Map)
		{
			var map = logic as Map;
			viewId = map.Descriptor.ViewId;
			viewType = typeof(MapView);
		}
		else if (logic is Ability)
		{
			var ability = logic as Ability;
			viewId = ability.Descriptor.ViewId;
			viewType = typeof(AbilityView);
		}

		if (!string.IsNullOrEmpty(viewId))
		{
			var view = PrefabTool.CreateInstance<View>(viewType, viewId);
			if (view != null)
			{
				logicToViewMap[logic] = view;
				view.FetchLogic(logic);
			}
			else
			{
				Debug.LogWarning("Not found view " + viewId);
			}
		}
	}

	private void OnLogicDestroy(Logic logic)
	{
		if (logic != null)
		{
			View view = null;
			if (logicToViewMap.TryGetValue(logic, out view))
			{
				view.ReturnToPool();
				logicToViewMap.Remove(logic);
			}
		}
	}
}