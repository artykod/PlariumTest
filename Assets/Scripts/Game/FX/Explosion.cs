using UnityEngine;

/// <summary>
/// Визуальный эффект взрыва.
/// </summary>
[PathInResources(Constants.Paths.Views.ALL + "FX/Explosions/")]
public class Explosion : PoolableObject
{
	[SerializeField]
	private float lifeTime;

	private float time;

	/// <summary>
	/// Запустить взрыв в произвольной точке.
	/// </summary>
	/// <param name="position">точка, где будет отыгран взрыв.</param>
	public void Fire(Vector3 position)
	{
		transform.position = position;

		var particles = GetComponent<ParticleSystem>();
		if (particles != null)
		{
			particles.Play();
		}
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
			time -= Time.deltaTime;
			if (time < 0f)
			{
				ReturnToPool();
			}
		}
	}
}
