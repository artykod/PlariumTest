using Newtonsoft.Json;

namespace Game.Descriptors.Buildings
{
	using Characters;

	public class BarracksDescriptor : BuildingGenericDescriptor<BarracksLevel>
	{
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
	}

	public class BarracksLevel : BuildingLevel
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
		public MobLevel MobsLevel
		{
			get;
			private set;
		}

		public void PostInit(BarracksDescriptor barracks)
		{
			MobsLevel = barracks.Unit.Levels[LevelOfUnits - 1];
		}
	}
}