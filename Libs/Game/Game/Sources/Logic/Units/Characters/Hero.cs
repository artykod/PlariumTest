namespace Game.Logics.Characters
{
	using Descriptors;
	using Descriptors.Characters;

	public class Hero : Character
	{
		public new HeroDescriptor Descriptor
		{
			get
			{
				return base.Descriptor as HeroDescriptor;
			}
		}

		public override CharacterLevel CurrentLevel
		{
			get
			{
				return Descriptor.Levels[Level];
			}
		}

		public Hero(GameController gameController, Descriptor descriptor) : base(gameController, descriptor)
		{
		}
	}
}
