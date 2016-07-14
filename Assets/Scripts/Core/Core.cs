using System.Collections.Generic;
using UnityEngine;
using Game;
using Game.Logics;
using Game.Logics.Maps;

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

		gameController = new GameController(new Loader().LoadDescriptorsFromGameResources(Constants.Paths.Descriptors.ALL));
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

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (UIDialogBase.CurrentDialog == null)
			{
				UIDialogGameMenu.Show().Build("Main menu", "")
					.AddButton("Surrender", GameController.Surrender)
					.AddButton("Settings", () => UIDialogText.Show().Build("Not implemented", "").AddButton("OK"))
					.AddButton("Clear all progress and quit", ClearAllProgressAndQuit)
					.AddButton("Quit", Application.Quit);
			}
			else if (UIDialogGameMenu.CurrentInstance != null)
			{
				UIDialogGameMenu.CurrentInstance.Close();
			}
		}
	}

	private void OnDestroy()
	{
		instance = null;
	}

	private void ClearAllProgressAndQuit()
	{
		PlayerPrefs.DeleteAll();
		PlayerPrefs.Save();
		Application.Quit();
	}

	private void OnLogicCreate(Logic logic)
	{
		var viewId = string.Empty;

		if (logic is Unit)
		{
			var unit = logic as Unit;
			viewId = unit.Descriptor.Levels[unit.Level].ViewId;
		}
		else if (logic is Map)
		{
			var map = logic as Map;
			viewId = map.Descriptor.ViewId;
		}

		// TODO add instances pool
		var view = PrefabTool.CreateInstance<View>(viewId);
		if (view != null)
		{
			logicToViewMap[logic] = view;
			view.FetchLogic(logic);
		}
	}

	private void OnLogicDestroy(Logic logic)
	{
		if (logic != null)
		{
			View view = null;
			if (logicToViewMap.TryGetValue(logic, out view))
			{
				Destroy(view.gameObject);
				logicToViewMap.Remove(logic);
			}
		}
	}
}