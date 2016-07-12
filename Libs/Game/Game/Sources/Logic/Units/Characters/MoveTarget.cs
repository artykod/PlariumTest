namespace Game.Logics.Characters
{
	using Descriptors;

	public class MoveTarget : Unit
	{
		public MoveTarget(GameController gameController, Descriptor descriptor) : base(gameController, descriptor)
		{
			IsImmortal = true;
			IsStatic = true;
		}
	}
}
