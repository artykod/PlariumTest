using UnityEngine;

public abstract class FieldObject : MonoBehaviour
{
	[PathInResources(Constants.Paths.Visuals.GENERAL)]
	protected abstract class VisualDescriptor
	{
		public string PrefabName
		{
			get;
			private set;
		}

		public VisualDescriptor SetPrefabName(string prefabName)
		{
			PrefabName = prefabName;
			return this;
		}
	}

	private ComponentCache<Transform> cachedTransform;
	private VisualDescriptor visualDescriptor;
	private GameObject visual;

	public new Transform transform
	{
		get
		{
			return cachedTransform.GetCache(gameObject);
		}
	}

	protected abstract VisualDescriptor BuildVisualDescriptor();

	protected virtual void Awake()
	{
		RebuildVisual();
	}

	protected virtual void Start()
	{
	}

	protected virtual void Update()
	{
	}

	protected virtual void OnDestroy()
	{
	}

	protected void RebuildVisual()
	{
		visualDescriptor = BuildVisualDescriptor();

		DestroyVisual();

		var visualInstance = PrefabTool.CreateInstance<Transform>(visualDescriptor.GetType(), visualDescriptor.PrefabName);
		if (visualInstance != null)
		{
			visual = visualInstance.gameObject;
			visual.gameObject.name = "Visual";
			TransformTool.DropTo(visual, transform);
		}
	}

	private void DestroyVisual()
	{
		if (visual != null)
		{
			Destroy(visual);
			visual = null;
		}
	}
}
