using System;
using System.Collections.Generic;

namespace Game.Logics
{
	using Descriptors;
	using Descriptors.Abilities;

	public abstract class Unit : Logic
	{
		protected class ModificatorData {
			public Modificator Modificator
			{
				get;
				private set;
			}
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

		private TimeController.Coroutine updater;
		protected int currentLevel;
		private Vec2 direction;
		private bool isSelected;
		protected List<ModificatorData> modificators = new List<ModificatorData>();

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

		public void AddModificator(Modificator modificator)
		{
			modificators.Add(new ModificatorData(modificator));
			UpdateStats();
		}

		public bool TakeDamage(int damageValue)
		{
			if (!IsImmortal)
			{
				var damage = ComputeDamage(damageValue);
				HP -= damage;
				return CheckHP();
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
