using Newtonsoft.Json;

public abstract class DescriptorCharacter<TLevel> : Descriptor where TLevel : DescriptorCharacter<TLevel>.Level
{
	public abstract class Level {
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
		public float Speed
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
	public TLevel[] Levels
	{
		get;
		private set;
	}
}
