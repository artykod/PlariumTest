using UnityEngine;
using Game.Descriptors;
using Game.Descriptors.Buildings;

public class Core : MonoBehaviour
{
	private void Awake()
	{
		DebugConsole.Instance.Create();
		DebugImpl.Instance = new DebugUnity();

		var jsonDescriptors = Resources.LoadAll<TextAsset>("TestJsonData");
		foreach (var json in jsonDescriptors)
		{
			var descriptor = Descriptor.Deserialize(json.text);
			descriptor.Init();
		}
		Descriptor.ForEach(desc => desc.PostInit());

		var map = Descriptor.GetInstance<Map>("map.forest");
		if (map != null)
		{
			var mapView = PrefabTool.CreateInstance<View>(map.ViewId);
			if (mapView != null)
			{
				mapView.transform.position = new Vector3(map.Width / 2f, 0f, map.Height / 2f);
			}

			foreach (var i in map.Markers)
			{
				var building = i.Object as Building;
				if (building != null)
				{
					var viewId = building.UnitLevels[0].ViewId;
					var view = PrefabTool.CreateInstance<View>(viewId);
					if (view != null)
					{
						var position = new Vector3(i.X, 0f, i.Y);
						view.transform.position = position;

						var fountain = building as Fountain;
						if (fountain != null)
						{
							var cameraPosition = position;
							cameraPosition.y += 30f;
							cameraPosition.z -= 5f;
							Camera.main.transform.position = cameraPosition;
						}
					}
				}
			}
		}
	}
}