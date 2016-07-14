using System.IO;
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
		DebugConsole.Instance.Create();
		TimeControllerImpl.Instance = new TimeControllerUnity();

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

	#region Editor tools

#if UNITY_EDITOR
	static Core()
	{
		DebugImpl.Instance = new DebugUnity();
	}

	[UnityEditor.MenuItem("Tools/Copy descriptors from lib project")]
	private static void CopyDescriptorsDataFromLibProject()
	{
		var inAssetsPath = Application.dataPath + @"/Resources/DescriptorsData";
		var inLibPath = Application.dataPath + @"/../Libs/Game/ConsoleTest/TestJsonData";

		Debug.Log("Copy descriptors data from {0} to {1}", inLibPath, inAssetsPath);

		if (Directory.Exists(inAssetsPath))
		{
			Directory.Delete(inAssetsPath, true);
		}

		foreach (string dirPath in Directory.GetDirectories(inLibPath, "*", SearchOption.AllDirectories))
		{
			Directory.CreateDirectory(dirPath.Replace(inLibPath, inAssetsPath));
		}

		foreach (string newPath in Directory.GetFiles(inLibPath, "*.*", SearchOption.AllDirectories))
		{
			File.Copy(newPath, newPath.Replace(inLibPath, inAssetsPath), true);
		}

		UnityEditor.AssetDatabase.Refresh();
		UnityEditor.AssetDatabase.SaveAssets();

		Debug.Log("Copy done.");
	}
#endif

	#endregion
}