using Newtonsoft.Json;

public class DescriptorCharacterHero : DescriptorBuilding<DescriptorCharacterHero.Level>
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
