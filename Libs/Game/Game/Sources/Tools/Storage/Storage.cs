using Game;

public class Storage
{
	public static void SaveValueByKey(string key, string value)
	{
		StorageImpl.Instance.SaveValueByKey(key, value);
	}

	public static string LoadValueByKey(string key)
	{
		return StorageImpl.Instance.LoadValueByKey(key);
	}
}

namespace Game
{
	public class StorageImpl
	{
		public static IStorage Instance
		{
			get;
			set;
		}
	}
}