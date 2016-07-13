namespace Game.Logics.Buildings
{
	using Descriptors;

	public class MobPortal : Barracks
	{
		private float waveTime;
		private float timeBetweenWaves;

		public bool IsWavesEnds
		{
			get;
			private set;
		}

		public MobPortal(GameController gameController, Descriptor descriptor) : base(gameController, descriptor)
		{
			Level = 0;
			waveTime = 25f;
		}

		protected override void Update(float dt)
		{
			base.Update(dt);

			if (IsWavesEnds)
			{
				return;
			}

			if (waveTime > 0f)
			{
				timeBetweenWaves = 60f;

				if (mobEmitTimeCurrent <= 0f)
				{
					mobEmitTimeCurrent = mobEmitFrequency;
					EmitMob();
				}
				else
				{
					mobEmitTimeCurrent -= dt;
				}

				waveTime -= TimeController.deltaTime;
			}
			else
			{
				if (timeBetweenWaves < 0f)
				{
					if (Level + 1 < Descriptor.Levels.Length)
					{
						Level++;
						waveTime = 25f;
					}
					else
					{
						IsWavesEnds = true;
					}
				}
				else
				{
					timeBetweenWaves -= TimeController.deltaTime;
				}
			}
		}
	}
}
