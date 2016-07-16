using UnityEngine;

/// <summary>
/// Визуальный эффект маркера.
/// Например, для визуализации выбранного места для передвижения героя.
/// </summary>
[PathInResources(Constants.Paths.Views.ALL + "FX/Markers/")]
public class Marker : PoolableObject
{
	[SerializeField]
	private float lifeTime;

	private float time;

	/// <summary>
	/// Показать маркер в произвольной точке.
	/// </summary>
	/// <param name="position">точка, где будет отыгран взрыв.</param>
	public void Show(Vector3 position)
	{
		transform.position = position;
		transform.localScale = Vector3.one;
	}

	protected override void OnGet()
	{
		base.OnGet();
		time = lifeTime;
	}

	private void Update()
	{
		if (time > 0f)
		{
			var scale = Mathf.Clamp01(time / lifeTime);
			transform.localScale = new Vector3(scale, 1f, scale);
			time -= Time.deltaTime;
			if (time < 0f)
			{
				ReturnToPool();
			}
		}
	}
}
