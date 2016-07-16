using UnityEngine;
using Game.Logics;

/// <summary>
/// Визуальный эффект для полета снарядов.
/// </summary>
[PathInResources(Constants.Paths.Views.ALL + "FX/Shells/")]
public class Shell : PoolableObject
{
	private ParticleSystem particles;
	private Vector3 fromPoint;
	private Vector3 toPoint;
	private bool isFly;
	private float time;
	private float currentTime;
	private System.Action onFlyEnd;

	/// <summary>
	/// для нормального отображения партиклов, генерящихся в зависимости от дистанции
	/// пропускаются кадры перед эмитом частиц.
	/// </summary>
	private int framesToSkip;

	private void Awake()
	{
		particles = GetComponent<ParticleSystem>();
		if (particles != null)
		{
			particles.enableEmission = false;
		}
	}

	/// <summary>
	/// Запустить снаряд.
	/// </summary>
	/// <param name="from">из какой точки полетит.</param>
	/// <param name="to">в какую точку прилетит.</param>
	/// <param name="flyTime">время полета.</param>
	/// <param name="onFlyEnd">колбек на окончание полета.</param>
	public void Fire(Vector3 from, Vector3 to, float flyTime, System.Action onFlyEnd = null)
	{
		fromPoint = from;
		toPoint = to;
		time = currentTime = flyTime;
		isFly = true;
		framesToSkip = 1;
		this.onFlyEnd = onFlyEnd;

		if (particles != null)
		{
			particles.Play();
			particles.enableEmission = false;
		}

		transform.position = from;
	}
	/// <summary>
	/// Запустить снаряд от юнита в другого юнита.
	/// </summary>
	/// <param name="fromUnit">от какого юнита полетит.</param>
	/// <param name="toUnit">в какого прилетит.</param>
	/// <param name="flyTime">время полета.</param>
	public void Fire(Unit fromUnit, Unit toUnit, float flyTime)
	{
		if (fromUnit != toUnit)
		{
			Fire(new Vector3(fromUnit.Position.x, 1f, fromUnit.Position.y), new Vector3(toUnit.Position.x, 1f, toUnit.Position.y), flyTime);
		}
	}
	/// <summary>
	/// Запустить снаряд от юнита в любую точку.
	/// </summary>
	/// <param name="fromUnit">от какого юнита полетит.</param>
	/// <param name="to">в какую точку прилетит.</param>
	/// <param name="flyTime">время полета.</param>
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
				onFlyEnd.SafeInvoke();
			}
		}
	}
}
