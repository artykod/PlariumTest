using UnityEngine;
using Game.Logics;

[PathInResources(Constants.Paths.Views.ALL)]
public class View : PoolableObject
{
	private ComponentCache<Transform> cacheTransform;

	public Logic Logic
	{
		get;
		private set;
	}

	public new Transform transform
	{
		get
		{
			return cacheTransform.GetCache(gameObject);
		}
	}

	protected virtual void Awake()
	{
	}

	public virtual void FetchLogic(Logic logic)
	{
		Logic = logic;
		transform.rotation = Quaternion.identity;
		transform.position = Vector3.zero;
	}

	protected virtual void Start()
	{
	}

	protected virtual void Update()
	{
	}
}
