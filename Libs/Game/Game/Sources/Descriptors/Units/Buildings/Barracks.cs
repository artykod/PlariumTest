using Newtonsoft.Json;

namespace Game.Descriptors.Buildings
{
	using Characters;

	public class Barracks : BuildingGeneric<BarracksLevel>
	{
		[JsonProperty]
		private string UnitId
		{
			get;
			set;
		}

		[JsonIgnore]
		public Mob Unit
		{
			get;
			private set;
		}

		public override void PostInit()
		{
			base.PostInit();

			Unit = GetInstance(UnitId) as Mob;

			foreach (var i in Levels)
			{
				i.PostInit(this);
			}
		}
	}

	public class BarracksLevel : BuildingLevel
	{
		[JsonProperty]
		public int UnitsPerSecond
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

		public void PostInit(Barracks barracks)
		{
			MobsLevel = barracks.Unit.Levels[LevelOfUnits - 1];
		}
	}
}