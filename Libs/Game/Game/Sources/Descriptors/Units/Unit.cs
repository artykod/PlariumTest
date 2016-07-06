using Newtonsoft.Json;

namespace Game.Descriptors
{
	public abstract class Unit : Descriptor
	{
		[JsonIgnore]
		public abstract UnitLevel[] UnitLevels
		{
			get;
		}
	}

	public class UnitLevel
	{
		[JsonProperty]
		public string ViewId
		{
			get;
			private set;
		}

		[JsonProperty]
		public int CostOfObtain
		{
			get;
			private set;
		}
	}
}