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

		public new MobDescriptor.Level CurrentLevel
		{
			get
			{
				return GetCurrentLevelImpl<MobDescriptor.Level>();
			}
		}

		protected override T GetCurrentLevelImpl<T>()
		{
			return Descriptor.Levels[Level] as T;
		}

		public Mob(GameController gameController, Descriptor descriptor) : base(gameController, descriptor)
		{
		}
	}
}
