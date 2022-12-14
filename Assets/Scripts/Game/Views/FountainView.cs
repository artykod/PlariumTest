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

		var camera = Core.Instance.GameCamera;
		var position = transform.position;
		var cameraPosition = camera.transform.position;

		// изначально камера чуть позади фонтана.
		cameraPosition.x = position.x;
		cameraPosition.y = position.y + 30f;
		cameraPosition.z = position.z - 5f;
		camera.transform.position = cameraPosition;
	}
}
