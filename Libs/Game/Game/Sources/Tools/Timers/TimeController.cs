using System;
using System.Collections;
using System.Collections.Generic;
using Game;

public class TimeController
{
	public abstract class YieldInstruction
	{
		protected IEnumerator Routine
		{
			get;
			set;
		}

		internal bool Update()
		{
			var yield = Routine.Current as YieldInstruction;
			if (yield != null)
			{
				if (yield.Update())
				{
					return true;
				}
			}

			if (Routine == null || !Routine.MoveNext())
			{
				return false;
			}

			return true;
		}
	}

	public class Coroutine : YieldInstruction
	{
		internal Coroutine(IEnumerator routine)
		{
			Routine = routine;
		}
	}

	public class WaitForSeconds : YieldInstruction
	{
		private float timeEndWait;

		protected virtual float timer
		{
			get
			{
				return time;
			}
		}

		private IEnumerator WaitSeconds()
		{
			while (timeEndWait > timer)
			{
				yield return null;
			}
		}

		public WaitForSeconds(float seconds)
		{
			timeEndWait = timer + seconds;
			Routine = WaitSeconds();
		}
	}

	public class WaitForUnscaledSeconds : WaitForSeconds
	{
		protected override float timer
		{
			get
			{
				return unscaledTime;
			}
		}

		public WaitForUnscaledSeconds(float seconds) : base(seconds)
		{
		}
	}

	private static HashSet<YieldInstruction> coroutines = new HashSet<YieldInstruction>();
	private static HashSet<YieldInstruction> coroutinesToAddCache = new HashSet<YieldInstruction>();
	private static HashSet<YieldInstruction> coroutinesToRemoveCache = new HashSet<YieldInstruction>();

	public static float time
	{
		get
		{
			return TimeControllerImpl.Instance.time;
		}
	}

	public static float deltaTime
	{
		get
		{
			return TimeControllerImpl.Instance.deltaTime;
		}
	}

	public static float unscaledDeltaTime
	{
		get
		{
			return TimeControllerImpl.Instance.unscaledDeltaTime;
		}
	}

	public static float unscaledTime
	{
		get
		{
			return TimeControllerImpl.Instance.unscaledTime;
		}
	}

	public static void Update()
	{
		foreach (var coroutine in coroutinesToAddCache)
		{
			coroutines.Add(coroutine);
		}
		coroutinesToAddCache.Clear();

		foreach (var coroutine in coroutines)
		{
			UpdateCoroutine(coroutine);
		}

		foreach (var coroutine in coroutinesToRemoveCache)
		{
			coroutines.Remove(coroutine);
		}
		coroutinesToRemoveCache.Clear();
	}

	public static Coroutine StartCoroutine(IEnumerator coroutine)
	{
		var routine = new Coroutine(coroutine);
		if (UpdateCoroutine(routine))
		{
			coroutinesToAddCache.Add(routine);
		}
		return routine;
	}

	public static void StopCoroutine(Coroutine coroutine)
	{
		coroutinesToRemoveCache.Add(coroutine);
	}

	private static bool UpdateCoroutine(YieldInstruction coroutine)
	{
		try
		{
			if (coroutine.Update())
			{
				return true;
			}
		}
		catch (Exception e)
		{
			Debug.LogException(e);
		}

		coroutinesToRemoveCache.Add(coroutine);

		return false;
	}
}

namespace Game
{
	public class TimeControllerImpl
	{
		public static ITimeController Instance
		{
			get;
			set;
		}
	}
}