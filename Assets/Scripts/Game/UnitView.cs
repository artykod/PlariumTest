using UnityEngine;
using Game.Logics;
using Game.Logics.Characters;

public class UnitView : View {
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

	private void OnLogicDestroy(Logic logic)
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

	private void OnDrawGizmos()
	{
		var color = Color.red;
		color.a = 0.5f;
		Gizmos.color = color;

		var character = Logic as Character;
		if (selection.IsVisible && character != null && character.TargetUnit != null)
		{
			var targetPos = character.TargetUnit.Position;
			Gizmos.DrawSphere(new Vector3(targetPos.x, 0f, targetPos.y), Mathf.Max(0.1f, character.TargetUnit.Descriptor.Size));
		}
	}
}
