using Newtonsoft.Json;

namespace Game.Descriptors.Buildings
{
	using Characters;

	public abstract class BarracksDescriptor : BuildingDescriptor
	{
		public new class Level : BuildingDescriptor.Level
		{
			[JsonProperty]
			public float UnitsPerSecond
			{
				get;
				private set;
			}

			[JsonProperty]
			private int LevelOfUnits
			{
				get;
				set;
			}

			[JsonIgnore]
			public MobDescriptor.Level MobsLevel
			{
				get;
				private set;
			}

			public void PostInit(BarracksDescriptor barracks)
			{
				MobsLevel = barracks.Unit.Levels[LevelOfUnits - 1];
			}
		}

		[JsonIgnore]
		public new Level[] Levels
		{
			get
			{
				return GetLevelsImpl<Level>();
			}
		}

		[JsonProperty]
		private string UnitId
		{
			get;
			set;
		}

		[JsonIgnore]
		public MobDescriptor Unit
		{
			get;
			private set;
		}

		public override void PostInit()
		{
			base.PostInit();

			Unit = GameController.FindDescriptorById<MobDescriptor>(UnitId);

			foreach (var i in Levels)
			{
				i.PostInit(this);
			}
		}

		protected override T[] GetLevelsImpl<T>()
		{
			return Levels as T[];
		}
	}
}