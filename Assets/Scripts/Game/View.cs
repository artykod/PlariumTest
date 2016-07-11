using UnityEngine;
using Game.Logics;

[PathInResources(Constants.Paths.Views.ALL)]
public class View : MonoBehaviour {
	public Logic Logic
	{
		get;
		private set;
	}

	protected virtual void Awake()
	{
	}

	public virtual void FetchLogic(Logic logic)
	{
		Logic = logic;
	}

	protected virtual void Start()
	{
	}

	protected virtual void Update()
	{
	}
}
