using System;

namespace Game.Logics.Abilities
{
	using Descriptors;
	using Descriptors.Abilities;
	using Characters;

	/// <summary>
	/// Логика работы абилок.
	/// </summary>
	public abstract class Ability : Logic
	{
		/// <summary>
		/// Для оптимизации расчета попаданий в радиус без взятия корня.
		/// </summary>
		protected float radiusSqr;
		/// <summary>
		/// Текущий уровень прокачки абилки.
		/// </summary>
		private int level;

		/// <summary>
		/// Событие на выбор абилки.
		/// </summary>
		public event Action<Ability> OnSelect;
		/// <summary>
		/// Событие на отмену выбора абилки.
		/// </summary>
		public event Action<Ability> OnCancel;
		/// <summary>
		/// Событие на срабатывание абилки.
		/// </summary>
		public event Action<Ability> OnExecute;
		/// <summary>
		/// Событие на завершение перезарядки абилки.
		/// </summary>
		public event Action<Ability> OnRecharged;
		/// <summary>
		/// Смена уровня прокачки абилки.
		/// </summary>
		public event Action<int, int> OnLevelChanged;

		public new AbilityDescriptor Descriptor
		{
			get
			{
				return base.Descriptor as AbilityDescriptor;
			}
		}

		public int Level
		{
			get
			{
				return level;
			}
			set
			{
				if (value < Descriptor.Levels.Length)
				{
					var prevLevel = level;
					level = value;
					OnLevelChange(prevLevel, level);
				}
			}
		}

		public AbilityDescriptor.Level CurrentLevel
		{
			get;
			private set;
		}

		public bool IsSelected
		{
			get;
			private set;
		}

		public float CurrentCooldown
		{
			get;
			private set;
		}

		public float TotalCooldown
		{
			get;
			private set;
		}

		public bool IsRecharging
		{
			get
			{
				return CurrentCooldown > 0f;
			}
		}

		/// <summary>
		/// Кто кастует эту абилку.
		/// </summary>
		public Character Caster
		{
			get;
			private set;
		}
		/// <summary>
		/// Юниты, на которые воздействует абилка.
		/// </summary>
		public Unit[] AffectedUnits
		{
			get;
			protected set;
		}
		/// <summary>
		/// Была ли абилка активирована.
		/// </summary>
		public bool IsActivated
		{
			get;
			protected set;
		}
		/// <summary>
		/// Точка активации абилки на поле боя.
		/// </summary>
		public Vec2 ActivationPoint
		{
			get;
			protected set;
		}

		public bool CanUpgradeLevel
		{
			get
			{
				return Level < Descriptor.Levels.Length - 1;
			}
		}

		public Ability(GameController gameController, Descriptor descriptor) : base(gameController, descriptor)
		{
			Level = 0;
			TotalCooldown = CurrentLevel.Cooldown;
		}

		public void FetchCaster(Character caster)
		{
			Caster = caster;
		}

		/// <summary>
		/// Выбрать абилку.
		/// </summary>
		public bool Select()
		{
			if (GameController.IsBattleStarted && !IsSelected)
			{
				IsSelected = true;
				OnSelect.SafeInvoke(this);
				return true;
			}
			return false;
		}
		/// <summary>
		/// Активировать абилку.
		/// </summary>
		/// <param name="point">Точка, где нужно активировать абилку.</param>
		/// <param name="clickedUnit">Юнит, на которого тыкнули. Если в пустое место, то null.</param>
		/// <returns></returns>
		public virtual bool Activate(Vec2 point, Unit clickedUnit)
		{
			if (CurrentCooldown > 0f)
			{
				return false;
			}
			ActivationPoint = point;
			return true;
		}
		/// <summary>
		/// Отменить выбор абилки.
		/// </summary>
		public bool Cancel()
		{
			IsActivated = false;
			if (GameController.IsBattleStarted && IsSelected)
			{
				IsSelected = false;
				OnCancel.SafeInvoke(this);
				return true;
			}
			return false;
		}

		public override void Destroy()
		{
			base.Destroy();

			Cancel();
		}

		/// <summary>
		/// Будет вызван при возможности активировать абилку.
		/// Допустим, если абилка активируется только на каком-то расстоянии, 
		/// то будет вызван, когда кастер доберется до нужного расстояния.
		/// </summary>
		protected void Execute()
		{
			CurrentCooldown = TotalCooldown;
			if (AffectedUnits != null)
			{
				// по всем подверженным абилке юнитам накладываем модификаторы абилки.
				foreach (var unit in AffectedUnits)
				{
					// если юнит еще жив.
					if (unit != null && unit.HP > 0)
					{
						foreach (var modifier in CurrentLevel.Modifiers)
						{
							unit.AddModifier(modifier);
						}
					}
				}
			}
			OnExecute.SafeInvoke(this);
			IsActivated = false;
			Cancel();
		}

		protected virtual void OnLevelChange(int previousLevel, int newLevel)
		{
			CurrentLevel = Descriptor.Levels[Level];
			radiusSqr = CurrentLevel.Radius * CurrentLevel.Radius;

			OnLevelChanged.SafeInvoke(previousLevel, newLevel);
		}

		protected override void Update(float dt)
		{
			base.Update(dt);

			// кулдаун на перезарядку абилки.
			if (CurrentCooldown > 0f)
			{
				CurrentCooldown -= dt;

				if (CurrentCooldown <= 0f)
				{
					CurrentCooldown = 0f;
					OnRecharged.SafeInvoke(this);
				}
			}
		}
	}
}
