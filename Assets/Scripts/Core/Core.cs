using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Game;
using Game.Logics;
using Game.Logics.Maps;

public class Core : MonoBehaviour
{
#if UNITY_EDITOR
	static Core()
	{
		DebugImpl.Instance = new DebugUnity();
	}
#endif

	private Loader loader;
	private GameController gameController;
	private Dictionary<Logic, View> logicToViewMap = new Dictionary<Logic, View>();

	private void Awake()
	{
		DebugImpl.Instance = new DebugUnity();
		DebugConsole.Instance.Create();
		TimeControllerImpl.Instance = new TimeControllerUnity();

		gameController = new GameController(new Loader().LoadDescriptorsFromGameResources(Constants.Paths.Descriptors.ALL));
		gameController.OnLogicCreate += OnLogicCreate;
		gameController.OnLogicDestroy += OnLogicDestroy;
		gameController.RunWithMapId("map.forest");
		gameController.Map.Fountain.FetchHeroId("unit.main_character");
	}

	private void OnLogicCreate(Logic logic)
	{
		var viewId = string.Empty;

		if (logic is Unit)
		{
			var unit = logic as Unit;
			viewId = unit.Descriptor.UnitLevels[unit.Level].ViewId;
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

	private void Update()
	{
		TimeController.Update();
	}

#if UNITY_EDITOR
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
}