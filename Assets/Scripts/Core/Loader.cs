using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class Loader
{
	public string[] LoadDescriptorsFromGameResources(string filesRootDirectory)
	{
		var jsonDescriptors = Resources.LoadAll<TextAsset>(filesRootDirectory);
		var filesContent = new List<string>(jsonDescriptors.Length);
		foreach (var json in jsonDescriptors)
		{
			filesContent.Add(json.text);
		}
		return filesContent.ToArray();
	}

	public string[] LoadDescriptorsFromExternalFiles(string filesRootDirectory)
	{
		var jsonDescriptors = Directory.GetFiles(Application.dataPath + "/../" + filesRootDirectory, "*.json", SearchOption.AllDirectories);
		var filesContent = new List<string>(jsonDescriptors.Length);
		foreach (var json in jsonDescriptors)
		{
			using (var reader = new StreamReader(json))
			{
				filesContent.Add(reader.ReadToEnd());
			}
		}
		return filesContent.ToArray();
	}
}