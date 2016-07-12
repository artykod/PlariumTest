using UnityEngine;

[PathInResources(Constants.Paths.Views.ALL)]
public class UnitSelection : MonoBehaviour
{
	[SerializeField]
	private Transform model;
	[SerializeField]
	private new MeshRenderer renderer;
	[SerializeField]
	private Material heroSelectionMaterial;
	[SerializeField]
	private Material minionSelectionMaterial;
	[SerializeField]
	private Material enemySelectionMaterial;

	private float scale = 1f;
	private SelectionTypes selectionType = SelectionTypes.Minion;

	public enum SelectionTypes
	{
		Hero,
		Minion,
		Enemy,
	}

	public SelectionTypes SelectionType
	{
		get
		{
			return selectionType;
		}
		set
		{
			selectionType = value;

			var selectionMaterial = minionSelectionMaterial;
			switch (selectionType)
			{
			case SelectionTypes.Hero:
				selectionMaterial = heroSelectionMaterial;
				break;
			case SelectionTypes.Enemy:
				selectionMaterial = enemySelectionMaterial;
				break;
			default:
				break;
			}
			renderer.sharedMaterial = selectionMaterial;
		}
	}

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
