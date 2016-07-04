using UnityEngine;

public class Core : MonoBehaviour
{
	private void Awake()
	{
		DebugConsole.Instance.Create();

		//PrefabTool.CreateInstance<FieldForest>();
	}
}