using UnityEngine;
using Game.Logics.Buildings;
using Game.Logics;

public class FountainView : UnitView {
	public new Fountain Logic
	{
		get
		{
			return base.Logic as Fountain;
		}
	}

	public override void FetchLogic(Logic logic)
	{
		base.FetchLogic(logic);

		var cameraPosition = transform.position;
		cameraPosition.y += 30f;
		cameraPosition.z -= 5f;
		Camera.main.transform.position = cameraPosition;
	}
}
