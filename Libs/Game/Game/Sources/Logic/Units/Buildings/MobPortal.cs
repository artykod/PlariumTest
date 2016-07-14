namespace Game.Logics.Buildings
{
	using Descriptors;
	using Descriptors.Buildings;

	public class MobPortal : Barracks
	{
		private float waveTime;
		private float timeBetweenWaves;

		public new MobPortalDescriptor Descriptor
		{
			get
			{
				return base.Descriptor as MobPortalDescriptor;
			}
		}

		public new MobPortalDescriptor.Level CurrentLevel
		{
			get
			{
				return GetCurrentLevelImpl<MobPortalDescriptor.Level>();
			}
		}

		protected override T GetCurrentLevelImpl<T>()
		{
			return Descriptor.Levels[Level] as T;
		}

		public bool IsWavesEnds
		{
			get;
			private set;
		}

		public MobPortal(GameController gameController, Descriptor descriptor) : base(gameController, descriptor)
		{
			Level = 0;
		}

		protected override void LevelChanged(int previousLevel, int newLevel)
		{
			base.LevelChanged(previousLevel, newLevel);

			waveTime = Descriptor.Levels[Level].WaveDuration;
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
				timeBetweenWaves = Descriptor.BetweenWavesTime;

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
