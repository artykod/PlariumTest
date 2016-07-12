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

		public override CharacterLevel CurrentLevel
		{
			get
			{
				return Descriptor.Levels[Level];
			}
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
	}
}
