using UnityEngine;
using Game.Logics;
using Game.Logics.Characters;

[PathInResources(Constants.Paths.Views.ALL + "Units/")]
public class UnitView : View
{
	protected UnitSelection selection;

	public new Unit Logic
	{
		get
		{
			return base.Logic as Unit;
		}
	}

	protected virtual float RotationLerpSpeed
	{
		get
		{
			return 1f;
		}
	}

	public override void FetchLogic(Logic logic)
	{
		base.FetchLogic(logic);
		SyncTransform();

		selection.Scale = Logic.Descriptor.Size * 2.5f;
		selection.IsVisible = false;

		if (Logic is Hero)
		{
			selection.SelectionType = UnitSelection.SelectionTypes.Hero;
		}
		else if (Logic.Team != Core.Instance.GameController.Map.Sofa.Team)
		{
			selection.SelectionType = UnitSelection.SelectionTypes.Enemy;
		}
		else
		{
			selection.SelectionType = UnitSelection.SelectionTypes.Minion;
		}

		Logic.OnDestroy += OnLogicDestroy;
		Logic.OnSelection += OnSelectionUnit;
	}

	protected virtual void OnLogicDestroy(Logic logic)
	{
		Logic.OnDestroy -= OnLogicDestroy;
		Logic.OnSelection -= OnSelectionUnit;
	}

	private void OnSelectionUnit(Unit unit, bool isSelected)
	{
		if (unit == Logic)
		{
			if (selection != null)
			{
				selection.IsVisible = isSelected;
			}
		}
	}

	protected void SyncTransform()
	{
		if (Logic != null)
		{
			transform.position = new Vector3(Logic.Position.x, 0f, Logic.Position.y);
			if (Mathf.Abs(Logic.Direction.y) > 0.001f && Mathf.Abs(Logic.Direction.x) > 0.001f)
			{
				transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, Mathf.Atan2(-Logic.Direction.y, Logic.Direction.x) * 180f / Mathf.PI + 90f, 0f), RotationLerpSpeed);
			}
		}
	}

	protected override void Awake()
	{
		base.Awake();

		selection = PrefabTool.CreateInstance<UnitSelection>();
		selection.DropTo(transform);
	}

	protected override void Update()
	{
		base.Update();

		SyncTransform();
	}
}
