namespace Game.Logics.Buildings
{
	using Descriptors;

	public abstract class Building : Unit
	{
		public Building(GameController gameController, Descriptor descriptor) : base(gameController, descriptor)
		{
			IsStatic = true;
		}
	}
}
