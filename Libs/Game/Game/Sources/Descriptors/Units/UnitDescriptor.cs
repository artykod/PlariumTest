using Newtonsoft.Json;

namespace Game.Descriptors
{
	public abstract class UnitDescriptor : Descriptor
	{
		[JsonIgnore]
		public abstract UnitLevel[] UnitLevels
		{
			get;
		}

		[JsonProperty]
		public float Size
		{
			get;
			private set;
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

		[JsonProperty]
		public float Speed
		{
			get;
			private set;
		}
	}
}