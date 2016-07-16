using UnityEngine;

[PathInResources(Constants.Paths.Views.ALL + "FX/Explosions/")]
public class Explosion : PoolableObject
{
	[SerializeField]
	private float lifeTime;

	private float time;

	public void Fire(Vector3 position)
	{
		transform.position = position;

		var particles = GetComponent<ParticleSystem>();
		particles.Play();
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
