using UnityEngine;
using Game.Logics;

public class UnitView : View {
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

	protected override void Update()
	{
		base.Update();

		SyncTransform();
	}
}
