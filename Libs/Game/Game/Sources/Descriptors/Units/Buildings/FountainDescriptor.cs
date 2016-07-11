using Newtonsoft.Json;

namespace Game.Descriptors.Buildings
{
	public class FountainDescriptor : BuildingGenericDescriptor<FountainLevel>
	{
	}

	public class FountainLevel : BuildingLevel
	{
		[JsonProperty]
		public int HealSpeedHero // per second
		{
			get;
			private set;
		}

		[JsonProperty]
		public int HealSpeedMinion // per second
		{
			get;
			private set;
		}
	}
}