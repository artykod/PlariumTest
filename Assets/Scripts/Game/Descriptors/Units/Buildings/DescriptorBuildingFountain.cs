using Newtonsoft.Json;

public class DescriptorBuildingFountain : DescriptorBuilding<DescriptorBuildingFountain.Level>
{
	public class Level : LevelBase
	{
		[JsonProperty]
		public int HealSpeedHero
		{
			get;
			private set;
		}

		[JsonProperty]
		public int HealSpeedMinion
		{
			get;
			private set;
		}
	}
}
