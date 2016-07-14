using System;

namespace Game.Logics
{
	using Descriptors;

	public abstract class Unit : Logic
	{
		private TimeController.Coroutine updater;
		protected int currentLevel;
		private Vec2 direction;
		private bool isSelected;

		public event Action<Unit, bool> OnSelection;
		public event Action<int, int> OnLevelChanged;

		public Vec2 Position
		{
			get;
			set;
		}

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

		public float Velocity
		{
			get;
			protected set;
		}

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

		public int HP
		{
			get;
			protected set;
		}

		public int TotalHP
		{
			get;
			protected set;
		}

		public bool IsImmortal
		{
			get;
			protected set;
		}

		public string Team
		{
			get;
			private set;
		}

		public bool IsStatic
		{
			get;
			protected set;
		}

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

		public void AttachToTeam(string team)
		{
			Team = team;
		}

		public bool TakeDamage(int damageValue)
		{
			if (!IsImmortal)
			{
				var damage = ComputeDamage(damageValue);
				HP -= damage;
				if (HP <= 0)
				{
					Destroy();
					return true;
				}
			}

			return false;
		}

		public void Heal(int healValue)
		{
			if (!IsImmortal)
			{
				HP += healValue;
				if (HP >= TotalHP)
				{
					HP = TotalHP;
				}
			}
		}

		protected virtual int ComputeDamage(int damageValue)
		{
			return damageValue;
		}

		protected override void Update(float dt)
		{
			base.Update(dt);

			if (CanMove)
			{
				Position += Direction * Velocity * dt;
			}
		}

		protected virtual void LevelChanged(int previousLevel, int newLevel)
		{
			Velocity = Descriptor.Levels[Level].Speed;

			if (previousLevel != newLevel)
			{
				OnLevelChanged.SafeInvoke(previousLevel, newLevel);
			}
		}
	}
}
