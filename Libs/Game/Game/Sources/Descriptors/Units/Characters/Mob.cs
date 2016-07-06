using Newtonsoft.Json;

namespace Game.Descriptors.Characters
{
	public class Mob : CharacterGeneric<MobLevel>
	{
		
	}

	public class MobLevel : CharacterLevel
	{
		[JsonProperty]
		public int Gold
		{
			get;
			private set;
		}
		[JsonProperty]
		public int XP
		{
			get;
			private set;
		}
	}
}