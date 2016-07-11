using UnityEngine;

public class GridLines : MonoBehaviour {
#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		var center = transform.position;
		var mapSize = 25f;
		Gizmos.color = Color.red;
		for (float i = -mapSize; i < mapSize; i += 1f)
		{
			Gizmos.DrawLine(center + new Vector3(i, 0f, -mapSize), center + new Vector3(i, 0f, mapSize));
			Gizmos.DrawLine(center + new Vector3(-mapSize, 0f, i), center + new Vector3(mapSize, 0f, i));
		}
	}
#endif
}
