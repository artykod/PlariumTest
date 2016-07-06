using Newtonsoft.Json;

namespace Game.Descriptors.Characters
{
	public class Hero : CharacterGeneric<HeroLevel>
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
	}
}