using System;
using System.Collections;
using System.Collections.Generic;

namespace Game
{
	/// <summary>
	/// Базовый класс для контроллеров.
	/// При старте запускает корутину, в которой обновляет свое состояние на каждый кадр.
	/// Также обновляет всех своих чилдов.
	/// </summary>
	public abstract class BaseController
	{
		/// <summary>
		/// Чилды контроллера должны реализовывать этот интерфейс,
		/// т.к. обновление вызывается каждый кадр, а по интерфейсу вызовы работают быстрее на мобилках.
		/// </summary>
		public interface IUpdatable
		{
			void Update(float dt);
		}
		
		public bool IsRunned
		{
			get;
			private set;
		}

		private List<IUpdatable> updatables = new List<IUpdatable>(100);
		private TimeController.Coroutine updateRoutine;

		public void Run()
		{
			IsRunned = true;
			Start();
			updateRoutine = TimeController.StartCoroutine(UpdateRoutine());
		}

		public void Stop()
		{
			IsRunned = false;
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
