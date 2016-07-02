using UnityEngine;

public class GameCore : MonoBehaviour
{
	private void Awake()
	{
		DebugConsole.Instance.Create();
	}

	System.Collections.IEnumerator Start()
	{
		while (true)
		{
			Debug.Log("log");
			yield return new WaitForSeconds(1f);
			Debug.LogWarning("warning");
			yield return new WaitForSeconds(1f);
			Debug.LogError("error");
			yield return new WaitForSeconds(1f);
		}
	}
}