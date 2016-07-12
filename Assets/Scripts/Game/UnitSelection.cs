using UnityEngine;

[PathInResources(Constants.Paths.Views.ALL)]
public class UnitSelection : MonoBehaviour
{
	[SerializeField]
	private Transform model;

	private float scale = 1f;

	public float Scale
	{
		get
		{
			return scale;
		}
		set
		{
			scale = value;
			model.localScale = new Vector3(scale, scale, 1f);
		}
	}

	public bool IsVisible
	{
		get
		{
			return gameObject.activeSelf;
		}
		set
		{
			gameObject.SetActive(value);
		}
	}
}
