using UnityEngine;
using System.Collections.Generic;

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
}