using UnityEngine;
using Game.Logics.Maps;
using Game.Logics;

[PathInResources(Constants.Paths.Views.ALL + "Maps/")]
public class MapView : View {
	public new Map Logic
	{
		get
		{
			return base.Logic as Map;
		}
	}

	public override void FetchLogic(Logic logic)
	{
		base.FetchLogic(logic);

		transform.position = new Vector3(Logic.Descriptor.Width / 2f, 0f, Logic.Descriptor.Height / 2f);
	}
}
