namespace Game.Logics.Buildings
{
	using Descriptors;
	using Descriptors.Buildings;

	/// <summary>
	/// Казарма миньонов.
	/// </summary>
	public class MinionBarracks : Barracks
	{
		public new MinionBarracksDescriptor.Level CurrentLevel
		{
			get
			{
				return GetCurrentLevelImpl<MinionBarracksDescriptor.Level>();
			}
		}

		protected override T GetCurrentLevelImpl<T>()
		{
			return Descriptor.Levels[Level] as T;
		}

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
