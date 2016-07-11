namespace Game.Logics.Characters
{
	using Descriptors;
	using Descriptors.Characters;

	public class Mob : Character
	{
		public new MobDescriptor Descriptor
		{
			get
			{
				return base.Descriptor as MobDescriptor;
			}
		}

		public override CharacterLevel CurrentLevel
		{
			get
			{
				return Descriptor.Levels[Level];
			}
		}

		public Mob(GameController gameController, Descriptor descriptor) : base(gameController, descriptor)
		{
		}
	}
}
