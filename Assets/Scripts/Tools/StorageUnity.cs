using UnityEngine;

public class StorageUnity : IStorage
{
	string IStorage.LoadValueByKey(string key)
	{
		return PlayerPrefs.GetString(key);
	}

	void IStorage.SaveValueByKey(string key, string value)
	{
		PlayerPrefs.SetString(key, value);
		PlayerPrefs.Save();
	}
}
