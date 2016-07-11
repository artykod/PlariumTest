namespace Game.Logics.Buildings
{
	using Descriptors;
	using Descriptors.Buildings;
	using Characters;

	public class Barracks : Building
	{
		private float mobEmitTimeCurrent;
		private float mobEmitFrequency;

		public new BarracksDescriptor Descriptor
		{
			get
			{
				return base.Descriptor as BarracksDescriptor;
			}
		}

		public Barracks(GameController gameController, Descriptor descriptor) : base(gameController, descriptor)
		{
		}

		protected override void LevelChanged(int previousLevel, int newLevel)
		{
			base.LevelChanged(previousLevel, newLevel);

			mobEmitTimeCurrent = mobEmitFrequency = 1f / Descriptor.Levels[newLevel].UnitsPerSecond;
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

		private void EmitMob()
		{
			var mob = GameController.CreateLogicByDescriptor<Character>(Descriptor.Unit);
			if (mob != null)
			{
				mob.AttachToTeam(Team);
				mob.Position = Position + Vec2.FromAngle(GameRandom.Range(0f, 360f)) * (Descriptor.Size + mob.Descriptor.Size);
				mob.Direction = new Vec2(GameRandom.value, GameRandom.value);
			}
		}
	}
}
