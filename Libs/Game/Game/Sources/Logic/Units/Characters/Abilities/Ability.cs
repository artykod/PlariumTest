using System;

namespace Game.Logics.Abilities
{
	using Descriptors;
	using Descriptors.Abilities;
	using Characters;

	public abstract class Ability : Logic
	{
		protected float radiusSqr;
		private int level;

		public event Action<Ability> OnSelect;
		public event Action<Ability> OnCancel;
		public event Action<Ability> OnExecute;
		public event Action<Ability> OnRecharged;
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
				var prevLevel = level;
				level = value;
				OnLevelChange(prevLevel, level);
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

		public Character Caster
		{
			get;
			private set;
		}

		public Unit[] AffectedUnits
		{
			get;
			protected set;
		}

		public bool IsActivated
		{
			get;
			protected set;
		}

		public Vec2 ActivationPoint
		{
			get;
			protected set;
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

		public virtual bool Activate(Vec2 point, Unit clickedUnit)
		{
			ActivationPoint = point;
			return true;
		}

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

		protected void Execute()
		{
			CurrentCooldown = TotalCooldown;
			if (AffectedUnits != null)
			{
				foreach (var unit in AffectedUnits)
				{
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
