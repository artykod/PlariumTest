public class ConsoleTimeController : ITimeController
{
	private float deltaTime;
	private float time;

	public int FrameDurationMs
	{
		get
		{
			return 16;
		}
	}

	float ITimeController.deltaTime
	{
		get
		{
			return deltaTime;
		}
	}

	float ITimeController.time
	{
		get
		{
			return time;
		}
	}

	float ITimeController.unscaledDeltaTime
	{
		get
		{
			return deltaTime;
		}
	}

	float ITimeController.unscaledTime
	{
		get
		{
			return time;
		}
	}

	public void Update()
	{
		deltaTime = FrameDurationMs / 1000f;
		time += deltaTime;
		TimeController.Update();
	}
}
