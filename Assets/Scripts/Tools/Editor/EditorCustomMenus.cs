using System.IO;
using UnityEngine;
using UnityEditor;

public class EditorCustomMenus
{
	static EditorCustomMenus()
	{
		Game.DebugImpl.Instance = new DebugUnity();
	}

	[MenuItem("Tools/Copy descriptors from lib project")]
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

		AssetDatabase.Refresh();
		AssetDatabase.SaveAssets();

		Debug.Log("Copy done.");
	}

	[MenuItem("Tools/Clear player prefs")]
	private static void ClearPrefs()
	{
		PlayerPrefs.DeleteAll();
		PlayerPrefs.Save();
	}
}