using UnityEngine;

public class TimeControllerUnity : ITimeController
{
	float ITimeController.deltaTime
	{
		get
		{
			return Time.deltaTime * 4f;
		}
	}

	float ITimeController.time
	{
		get
		{
			return Time.time;
		}
	}

	float ITimeController.unscaledDeltaTime
	{
		get
		{
			return Time.unscaledDeltaTime;
		}
	}

	float ITimeController.unscaledTime
	{
		get
		{
			return Time.unscaledTime;
		}
	}
}
