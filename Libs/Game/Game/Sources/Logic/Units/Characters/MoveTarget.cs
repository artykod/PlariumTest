namespace Game.Logics.Characters
{
	using Descriptors;

	public class MoveTarget : Unit
	{
		protected override T GetCurrentLevelImpl<T>()
		{
			return Descriptor.Levels[Level] as T;
		}

		public MoveTarget(GameController gameController, Descriptor descriptor) : base(gameController, descriptor)
		{
			IsImmortal = true;
			IsStatic = true;
		}
	}
}
