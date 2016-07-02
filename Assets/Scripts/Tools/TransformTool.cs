using UnityEngine;

public static class TransformTool
{
	public static void DropTo(MonoBehaviour obj, MonoBehaviour to)
	{
		if (obj != null && to != null)
		{
			UnsafeDropTo(obj.transform, to.transform);
		}
	}

	public static void DropTo(GameObject obj, GameObject to)
	{
		if (obj != null && to != null)
		{
			UnsafeDropTo(obj.transform, to.transform);
		}
	}

	public static void DropTo(Transform obj, Transform to)
	{
		if (obj != null && to != null)
		{
			UnsafeDropTo(obj, to);
		}
	}

	private static void UnsafeDropTo(Transform obj, Transform to)
	{
		obj.SetParent(to, false);
		obj.localPosition = Vector3.zero;
		obj.localScale = Vector3.one;
		obj.localRotation = Quaternion.identity;
	}
}
