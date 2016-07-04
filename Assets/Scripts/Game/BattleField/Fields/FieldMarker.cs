using UnityEngine;

public class FieldMarker : MonoBehaviour
{
	[SerializeField]
	private string objectType;

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		var style = new GUIStyle();
		style.alignment = TextAnchor.MiddleCenter;
		UnityEditor.Handles.Label(transform.position, "Ololo", style);
	}
#endif
}
