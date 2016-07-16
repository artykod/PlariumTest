using UnityEngine;

public interface IPoolableObject
{
	void OnCreate(IObjectPool pool, string listId);
	void OnGet();
	void OnReturn();
}

/// <summary>
/// Компонент, от которого должны наследоваться объекты, которые должны храниться в пулах.
/// </summary>
public abstract class PoolableObject : MonoBehaviour, IPoolableObject
{
	/// <summary>
	/// Емкость пула для этого класса.
	/// При нехватке места в пуле будет создано заранее столько объектов сколько емкость.
	/// </summary>
	[SerializeField]
	private int poolCapacity = 1;

	private IObjectPool parentPool;
	private string parentPoolListId;

	public int PoolCapacity
	{
		get
		{
			return poolCapacity;
		}
	}

	public void ReturnToPool()
	{
		if (parentPool != null)
		{
			parentPool.Return(this, parentPoolListId);
		}
	}

	void IPoolableObject.OnCreate(IObjectPool pool, string listId)
	{
		parentPool = pool;
		parentPoolListId = listId;
	}

	void IPoolableObject.OnGet()
	{
		gameObject.SetActive(true);
		OnGet();
	}

	void IPoolableObject.OnReturn()
	{
		OnReturn();
		gameObject.SetActive(false);
	}

	protected virtual void OnGet()
	{
	}

	protected virtual void OnReturn()
	{
	}
}
