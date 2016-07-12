namespace Game.Logics.Buildings
{
	using Descriptors;

	public class MinionBarracks : Barracks
	{
		public MinionBarracks(GameController gameController, Descriptor descriptor) : base(gameController, descriptor)
		{
		}

		protected override void Update(float dt)
		{
			base.Update(dt);

			if (mobEmitTimeCurrent <= 0f)
			{
				mobEmitTimeCurrent = mobEmitFrequency;
				EmitMob();
			}
			else
			{
				mobEmitTimeCurrent -= dt;
			}
		}
	}
}
