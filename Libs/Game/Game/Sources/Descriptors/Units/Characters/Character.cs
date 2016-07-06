using Newtonsoft.Json;

namespace Game.Descriptors
{
	public abstract class Character : Unit
	{
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

	public abstract class CharacterGeneric<TLevel> : Character where TLevel : CharacterLevel
	{
		[JsonProperty]
		public TLevel[] Levels
		{
			get;
			private set;
		}

		[JsonIgnore]
		public override UnitLevel[] UnitLevels
		{
			get
			{
				return Levels as UnitLevel[];
			}
		}
	}

	public class CharacterLevel : UnitLevel
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
	}
}