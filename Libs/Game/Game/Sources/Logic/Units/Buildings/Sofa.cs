namespace Game.Logics.Buildings
{
	using Descriptors;
	using Descriptors.Buildings;

	public class Sofa : Building
	{
		public new SofaDescriptor Descriptor
		{
			get
			{
				return base.Descriptor as SofaDescriptor;
			}
		}

		public Sofa(GameController gameController, Descriptor descriptor) : base(gameController, descriptor)
		{
			IsImmortal = false;
		}

		protected override void LevelChanged(int previousLevel, int newLevel)
		{
			base.LevelChanged(previousLevel, newLevel);

			HP = Descriptor.Levels[Level].HP;
		}
	}
}
