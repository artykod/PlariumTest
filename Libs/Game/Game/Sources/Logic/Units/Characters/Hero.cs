namespace Game.Logics.Characters
{
	using Descriptors;
	using Descriptors.Characters;

	public class Hero : Character
	{
		private MoveTarget moveTarget;

		public new HeroDescriptor Descriptor
		{
			get
			{
				return base.Descriptor as HeroDescriptor;
			}
		}

		public new HeroDescriptor.Level CurrentLevel
		{
			get
			{
				return GetCurrentLevelImpl<HeroDescriptor.Level>();
			}
		}

		protected override T GetCurrentLevelImpl<T>()
		{
			return Descriptor.Levels[Level] as T;
		}

		public int XP
		{
			get;
			private set;
		}

		public int TotalXP
		{
			get;
			private set;
		}

		public Hero(GameController gameController, Descriptor descriptor) : base(gameController, descriptor)
		{
			moveTarget = new MoveTarget(gameController, Descriptor);
			SetTargetUnit(null);
		}

		public void MoveTo(Vec2 position)
		{
			moveTarget.Position = position;
			SetTargetUnit(moveTarget);
		}

		public void AddXP(int xp)
		{
			if (TotalXP <= 0)
			{
				XP = TotalXP;
			}
			else
			{
				XP += xp;
				if (XP >= TotalXP)
				{
					XP = TotalXP;
					Level++;
				}
			}
		}

		protected override void LevelChanged(int previousLevel, int newLevel)
		{
			base.LevelChanged(previousLevel, newLevel);

			TotalXP = Level == Descriptor.Levels.Length - 1 ? 0 : Descriptor.Levels[Level + 1].CostOfObtain;
			XP = 0;
		}
	}
}
