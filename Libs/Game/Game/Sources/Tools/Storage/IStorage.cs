public interface IStorage
{
	void SaveValueByKey(string key, string value);
	string LoadValueByKey(string key);
}