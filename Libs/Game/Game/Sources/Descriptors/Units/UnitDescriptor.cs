using Newtonsoft.Json;

namespace Game.Descriptors
{
	public abstract class UnitDescriptor : Descriptor
	{
		public class Level
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

		[JsonIgnore]
		public Level[] Levels
		{
			get
			{
				return GetLevelsImpl<Level>();
			}
		}

		[JsonProperty]
		public float Size
		{
			get;
			private set;
		}

		protected abstract T[] GetLevelsImpl<T>() where T : Level;
	}
}