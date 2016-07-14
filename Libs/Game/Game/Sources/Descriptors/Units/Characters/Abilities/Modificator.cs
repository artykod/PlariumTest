using Newtonsoft.Json;

namespace Game.Descriptors.Abilities
{
	public class Modificator
	{
		public enum Kinds
		{
			HP,
			Armor,
			Attack,
			AttackSpeed,
			Speed,
		}

		public enum Types
		{
			Add,
			Percent,
		}

		public enum Triggers
		{
			Now,
			During,
			Delay,
		}

		public enum AffectedUnits
		{
			Player,
			Enemy,
		}

		[JsonProperty]
		public Kinds Kind
		{
			get;
			private set;
		}

		[JsonProperty]
		public Types Type
		{
			get;
			private set;
		}

		[JsonProperty]
		public Triggers Trigger
		{
			get;
			private set;
		}

		[JsonProperty]
		public float TriggerTime
		{
			get;
			private set;
		}

		[JsonProperty]
		public AffectedUnits AffectedUnit
		{
			get;
			private set;
		}

		[JsonProperty]
		public float Value
		{
			get;
			private set;
		}
	}
}