using Newtonsoft.Json;

namespace Game.Descriptors.Characters
{
	public class HeroDescriptor : CharacterGenericDescriptor<HeroLevel>
	{
		
	}

	public class HeroLevel : CharacterLevel
	{
		[JsonProperty]
		public int TargetXP
		{
			get;
			private set;
		}

		[JsonProperty]
		public float RespawnTime // seconds
		{
			get;
			private set;
		}
	}
}