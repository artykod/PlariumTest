using UnityEngine;

public class PrefabLinker : MonoBehaviour
{
	[SerializeField]
	private GameObject linkedPrefab;

	public GameObject CreatedInstance
	{
		get;
		private set;
	}

	private void Awake()
	{
		if (linkedPrefab != null)
		{
			CreatedInstance = Instantiate(linkedPrefab);
		}
	}
}
