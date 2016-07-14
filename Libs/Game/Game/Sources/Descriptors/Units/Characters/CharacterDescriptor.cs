using Newtonsoft.Json;

namespace Game.Descriptors
{
	public abstract class CharacterDescriptor : UnitDescriptor
	{
		public new class Level : UnitDescriptor.Level
		{
			[JsonProperty]
			public int HP
			{
				get;
				private set;
			}
			[JsonProperty]
			public float Armor // 0..1
			{
				get;
				private set;
			}
			[JsonProperty]
			public int Attack
			{
				get;
				private set;
			}
			[JsonProperty]
			public float AttackSpeed // per second
			{
				get;
				private set;
			}
			[JsonProperty]
			public float AttackRange
			{
				get;
				private set;
			}
		}

		[JsonIgnore]
		public new Level[] Levels
		{
			get
			{
				return GetLevelsImpl<Level>();
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
	}
}