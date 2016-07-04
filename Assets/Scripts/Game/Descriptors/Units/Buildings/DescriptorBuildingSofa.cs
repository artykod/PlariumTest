using Newtonsoft.Json;

public class DescriptorBuildingSofa : DescriptorBuilding<DescriptorBuildingSofa.Level>
{
	public class Level
	{
		[JsonProperty]
		public int HP
		{
			get;
			private set;
		}
	}
}
