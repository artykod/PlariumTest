namespace Game.Logics.Buildings
{
	using Descriptors;
	using Descriptors.Buildings;

	/// <summary>
	/// Диван разработчиков.
	/// </summary>
	public class Sofa : Building
	{
		public new SofaDescriptor Descriptor
		{
			get
			{
				return base.Descriptor as SofaDescriptor;
			}
		}

		public new SofaDescriptor.Level CurrentLevel
		{
			get
			{
				return GetCurrentLevelImpl<SofaDescriptor.Level>();
			}
		}

		protected override T GetCurrentLevelImpl<T>()
		{
			return Descriptor.Levels[Level] as T;
		}

		public Sofa(GameController gameController, Descriptor descriptor) : base(gameController, descriptor)
		{
			IsImmortal = false;
		}

		protected override void LevelChanged(int previousLevel, int newLevel)
		{
			base.LevelChanged(previousLevel, newLevel);

			TotalHP = HP = Descriptor.Levels[Level].HP;
		}
	}
}
