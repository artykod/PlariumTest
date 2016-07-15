using System;
using System.Collections.Generic;

namespace Game.Logics
{
	using Descriptors;
	using Descriptors.Abilities;

	public abstract class Unit : Logic
	{
		/// <summary>
		/// Данные наложенного модификатора статов.
		/// </summary>
		protected class ModificatorData {
			/// <summary>
			/// Дескриптор модификатора.
			/// </summary>
			public Modificator Modificator
			{
				get;
				private set;
			}
			/// <summary>
			/// Время его действия.
			/// </summary>
			public float Time
			{
				get;
				set;
			}
			public ModificatorData(Modificator modificator)
			{
				Modificator = modificator;
				Time = modificator.Trigger == Modificator.Triggers.Now ? 0f : modificator.TriggerTime;
			}
		}
		
		/// <summary>
		/// Текущий уровень прокачки юнита.
		/// </summary>
		protected int currentLevel;
		/// <summary>
		/// Текущее направление движения юнита.
		/// </summary>
		private Vec2 direction;
		/// <summary>
		/// Выделен ли юнит игроком.
		/// </summary>
		private bool isSelected;
		/// <summary>
		/// Список наложенных модификаторов.
		/// </summary>
		protected List<ModificatorData> modificators = new List<ModificatorData>();

		public event Action<Unit, bool> OnSelection;
		public event Action<int, int> OnLevelChanged;

		/// <summary>
		/// Текущая позиция юнита на карте.
		/// </summary>
		public Vec2 Position
		{
			get;
			set;
		}
		/// <summary>
		/// Текущий поворот юнита. Оно же направление движения.
		/// </summary>
		public Vec2 Direction
		{
			get
			{
				return direction;
			}
			set
			{
				direction = value;
				direction.Normalize();
			}
		}
		/// <summary>
		/// Текущая скорость передвижения юнита.
		/// </summary>
		public float Velocity
		{
			get;
			protected set;
		}
		/// <summary>
		/// Номер текущего уровня прокачки юнита (от 0).
		/// </summary>
		public int Level
		{
			get
			{
				return currentLevel;
			}
			set
			{
				var previous = currentLevel;
				currentLevel = value;
				LevelChanged(previous, currentLevel);
			}
		}
		/// <summary>
		/// Текущее значение жизней.
		/// </summary>
		public int HP
		{
			get;
			protected set;
		}
		/// <summary>
		/// Текущее максимальное кол-во жизней.
		/// </summary>
		public int TotalHP
		{
			get;
			protected set;
		}
		/// <summary>
		/// Подвержен ли юнит урону.
		/// </summary>
		public bool IsImmortal
		{
			get;
			protected set;
		}
		/// <summary>
		/// Идентификатор команды юнита.
		/// </summary>
		public string Team
		{
			get;
			private set;
		}
		/// <summary>
		/// Статичный ли юнит. Например, здание.
		/// </summary>
		public bool IsStatic
		{
			get;
			protected set;
		}
		/// <summary>
		/// Выделен ли сейчас юнит игроком.
		/// </summary>
		public bool IsSelected
		{
			get
			{
				return isSelected;
			}
			set
			{
				isSelected = value;
				OnSelection.SafeInvoke(this, isSelected);
			}
		}

		/// <summary>
		/// Конкретные юниты могут определять когда нужно двигаться, а когда нужно стоять на месте.
		/// </summary>
		protected bool CanMove
		{
			get;
			set;
		}

		public new UnitDescriptor Descriptor
		{
			get
			{
				return base.Descriptor as UnitDescriptor;
			}
		}

		public UnitDescriptor.Level CurrentLevel
		{
			get
			{
				return GetCurrentLevelImpl<UnitDescriptor.Level>();
			}
		}

		protected abstract T GetCurrentLevelImpl<T>() where T : UnitDescriptor.Level;

		public Unit(GameController gameController, Descriptor descriptor) : base(gameController, descriptor)
		{
			IsImmortal = true;
			TotalHP = HP = int.MaxValue;
			CanMove = true;
			Level = 0;
		}

		/// <summary>
		/// Привязать юнита к команде.
		/// </summary>
		/// <param name="team">ид команды.</param>
		public void AttachToTeam(string team)
		{
			Team = team;
		}
		/// <summary>
		/// Наложить модификатор на юнита.
		/// </summary>
		/// <param name="modificator">накладываемый модификатор.</param>
		public void AddModificator(Modificator modificator)
		{
			modificators.Add(new ModificatorData(modificator));
			UpdateStats();
		}
		/// <summary>
		/// Нанести урон юниту в соответствии с его статами.
		/// </summary>
		/// <param name="damageValue">размер урона.</param>
		/// <returns>был ли убит юнит в следствие урона.</returns>
		public bool TakeDamage(int damageValue)
		{
			if (!IsImmortal && damageValue > 0)
			{
				var damage = ComputeDamage(damageValue);
				HP -= damage;
				return CheckHP();
			}
			return false;
		}
		/// <summary>
		/// Восстановить жизни юнита.
		/// </summary>
		/// <param name="healValue">на сколько восстановить.</param>
		public void Heal(int healValue)
		{
			if (!IsImmortal && healValue > 0)
			{
				HP += healValue;
				if (HP >= TotalHP)
				{
					HP = TotalHP;
				}
			}
		}

		/// <summary>
		/// Вычисление наносимого урона.
		/// Каждый юнит может по своему модифицировать урон.
		/// </summary>
		/// <param name="damageValue">размер урона.</param>
		/// <returns>модифицированное значение.</returns>
		protected virtual int ComputeDamage(int damageValue)
		{
			return damageValue;
		}

		protected override void Update(float dt)
		{
			base.Update(dt);

			// передвижение юнита каждый кадр в зависимости от его скорости.
			if (CanMove)
			{
				Position += Direction * Velocity * dt;
			}

			// обработка модификаторов. отмена истекших по времени.
			var modificatorsChanged = false;
			for (int i = 0; i < modificators.Count; i++)
			{
				var modificator = modificators[i];
				modificator.Time -= dt;
				if (modificator.Time <= 0f)
				{
					modificators.RemoveAt(i);
					modificatorsChanged = true;
					i--;
				}
			}
			if (modificatorsChanged)
			{
				UpdateStats();
			}
		}

		protected virtual void LevelChanged(int previousLevel, int newLevel)
		{
			UpdateStats();

			if (previousLevel != newLevel)
			{
				OnLevelChanged.SafeInvoke(previousLevel, newLevel);
			}
		}

		/// <summary>
		/// Применение наложенных модификаторов.
		/// </summary>
		protected virtual void UpdateStats()
		{
			var originalVelocity = Descriptor.Levels[Level].Speed;

			Velocity = originalVelocity;

			foreach (var i in modificators)
			{
				switch (i.Modificator.Kind)
				{
				case Modificator.Kinds.Speed:
					Velocity = ModifyValue(originalVelocity, i.Modificator);
					break;
				case Modificator.Kinds.HP:
					if (!IsImmortal)
					{
						HP = (int)ModifyValue(HP, i.Modificator);
					}
					break;
				}
			}

			CheckHP();
		}

		protected float ModifyValue(float originalValue, Modificator modificator)
		{
			switch (modificator.Type)
			{
			case Modificator.Types.Add:
				originalValue += modificator.Value;
				break;
			case Modificator.Types.Percent:
				originalValue = originalValue * modificator.Value;
				break;
			}

			return originalValue;
		}

		private bool CheckHP()
		{
			if (!IsImmortal && HP <= 0)
			{
				Destroy();
				return true;
			}
			return false;
		}
	}
}
