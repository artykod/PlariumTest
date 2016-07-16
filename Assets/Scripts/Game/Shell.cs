using UnityEngine;
using Game.Logics;

[PathInResources(Constants.Paths.Views.ALL + "Shells/")]
public class Shell : PoolableObject
{
	private ParticleSystem particles;
	private Vector3 fromPoint;
	private Vector3 toPoint;
	private bool isFly;
	private float time;
	private float currentTime;
	private int framesToSkip;

	private void Awake()
	{
		particles = GetComponent<ParticleSystem>();
		if (particles != null)
		{
			particles.enableEmission = false;
		}
	}

	public void Fire(Vector3 from, Vector3 to, float flyTime)
	{
		fromPoint = from;
		toPoint = to;
		time = currentTime = flyTime;
		isFly = true;
		framesToSkip = 1;

		if (particles != null)
		{
			particles.Play();
			particles.enableEmission = false;
		}

		transform.position = from;
	}

	public void Fire(Unit fromUnit, Unit toUnit, float flyTime)
	{
		Fire(new Vector3(fromUnit.Position.x, 1f, fromUnit.Position.y), new Vector3(toUnit.Position.x, 1f, toUnit.Position.y), flyTime);
	}

	public void Fire(Unit fromUnit, Vector3 to, float flyTime)
	{
		Fire(new Vector3(fromUnit.Position.x, 1f, fromUnit.Position.y), to, flyTime);
	}

	protected override void OnReturn()
	{
		base.OnReturn();

		isFly = false;
		time = currentTime = 0f;

		if (particles != null)
		{
			particles.Stop();
			particles.enableEmission = false;
		}
	}

	private void Update()
	{
		if (isFly)
		{
			var t = 1f - currentTime / time;
			transform.position = Vector3.Lerp(fromPoint, toPoint, t);
			transform.LookAt(toPoint);

			if (particles != null)
			{
				if (framesToSkip < 0)
				{
					particles.enableEmission = true;
				}
				else
				{
					framesToSkip--;
				}
			}

			currentTime -= Time.deltaTime;
			if (currentTime < 0f)
			{
				ReturnToPool();
			}
		}
	}
}
