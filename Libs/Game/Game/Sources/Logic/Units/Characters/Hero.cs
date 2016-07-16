namespace Game.Logics.Characters
{
	using Descriptors;
	using Descriptors.Characters;
	using Abilities;

	/// <summary>
	/// Главный персонаж.
	/// </summary>
	public class Hero : Character
	{
		/// <summary>
		/// Эта цель используется для отправки героя к произвольной точке на карте.
		/// Объект без дескриптора, чисто для сохранения общей логики движения персонажей.
		/// </summary>
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

		/// <summary>
		/// Текущий опыт.
		/// </summary>
		public int XP
		{
			get;
			private set;
		}
		/// <summary>
		/// Максимальный опыт на текущем уровне прокачки.
		/// </summary>
		public int TotalXP
		{
			get;
			private set;
		}
		/// <summary>
		/// Очки для прокачки абилок.
		/// </summary>
		public int UpgradesPoints
		{
			get;
			private set;
		}

		public Hero(GameController gameController, Descriptor descriptor) : base(gameController, descriptor)
		{
			Abilities = new Ability[Descriptor.Abilities.Length];
			for (int i = 0; i < Abilities.Length; i++)
			{
				Abilities[i] = gameController.CreateLogicByDescriptor<Ability>(Descriptor.Abilities[i]);
				Abilities[i].FetchCaster(this);
			}

			moveTarget = new MoveTarget(gameController, Descriptor);
			SetTargetUnit(null);
		}

		public void RestoreFromHero(Hero hero)
		{
			Level = hero.Level;
			XP = hero.XP;
			UpgradesPoints = hero.UpgradesPoints;
			for (int i = 0; i < hero.Abilities.Length; i++)
			{
				Abilities[i].Level = hero.Abilities[i].Level;
			}
		}

		public bool UpgradeAbility(Ability ability)
		{
			if (UpgradesPoints > 0 && ability.CanUpgradeLevel)
			{
				ability.Level++;
				UpgradesPoints--;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Отправить героя в точку на карте.
		/// </summary>
		/// <param name="position">точку, куда должен прийти герой.</param>
		public void MoveTo(Vec2 position)
		{
			moveTarget.Position = position;
			SetTargetUnit(moveTarget);
		}
		/// <summary>
		/// Добавить опыт.
		/// Если значение будет больше максимального текущего уровня, то уровень будет повышен.
		/// </summary>
		/// <param name="xp">значение опыта.</param>
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
					UpgradesPoints++;
				}
			}
		}

		protected override void LevelChanged(int previousLevel, int newLevel)
		{
			base.LevelChanged(previousLevel, newLevel);

			TotalXP = (Level == Descriptor.Levels.Length - 1) ? 0 : Descriptor.Levels[Level + 1].TargetXP;
			XP = 0;
		}
	}
}
