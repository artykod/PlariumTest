using Newtonsoft.Json;

public class DescriptorBuildingBarracks : DescriptorBuilding<DescriptorBuildingBarracks.Level>
{
	public class Level : LevelBase
	{
		[JsonProperty]
		public int UnitsPerSecond
		{
			get;
			private set;
		}
	}

	[JsonProperty]
	public string UnitId
	{
		get;
		private set;
	}
}
