using Newtonsoft.Json;

namespace Game.Descriptors.Abilities
{
	public class AbilityDescriptor : Descriptor
	{
		public class Level
		{
			[JsonProperty]
			public float Radius
			{
				get;
				private set;
			}

			[JsonProperty]
			public float Cooldown
			{
				get;
				private set;
			}

			[JsonProperty]
			public Modificator[] Modificators
			{
				get;
				private set;
			}
		}

		[JsonProperty]
		public string Name
		{
			get;
			private set;
		}

		[JsonProperty]
		public string Description
		{
			get;
			private set;
		}

		[JsonProperty]
		public string ViewId
		{
			get;
			private set;
		}

		[JsonProperty]
		public string IconId
		{
			get;
			private set;
		}

		[JsonProperty]
		public Level[] Levels
		{
			get;
			private set;
		}
	}
}