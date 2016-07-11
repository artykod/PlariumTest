using System;
using System.Collections;
using System.Collections.Generic;

namespace Game
{
	public abstract class BaseController
	{
		public interface IUpdatable
		{
			void Update(float dt);
		}

		private List<IUpdatable> updatables = new List<IUpdatable>(100);
		private TimeController.Coroutine updateRoutine;

		public void Run()
		{
			Start();
			updateRoutine = TimeController.StartCoroutine(UpdateRoutine());
		}

		public void Stop()
		{
			if (updateRoutine != null)
			{
				TimeController.StopCoroutine(updateRoutine);
				updateRoutine = null;
			}
			End();
		}

		public void AddUpdatable(IUpdatable updatable)
		{
			if (!updatables.Contains(updatable))
			{
				updatables.Add(updatable);
			}
		}

		public void RemoveUpdatable(IUpdatable updatable)
		{
			updatables.Remove(updatable);
		}

		private IEnumerator UpdateRoutine()
		{
			while (true)
			{
				Update();
				yield return null;
			}
		}

		protected virtual void Start()
		{
		}

		protected virtual void Update()
		{
			var dt = TimeController.deltaTime;
			for (int i = 0; i < updatables.Count; i++)
			{
				try
				{
					updatables[i].Update(dt);
				}
				catch (Exception e)
				{
					Debug.LogException(e);
				}
			}
		}

		protected virtual void End()
		{
		}
	}
}
