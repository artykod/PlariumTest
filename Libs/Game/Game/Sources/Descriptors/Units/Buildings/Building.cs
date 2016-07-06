using Newtonsoft.Json;

namespace Game.Descriptors.Buildings
{
	public abstract class Building : Unit
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

	public abstract class BuildingGeneric<TLevel> : Building where TLevel : BuildingLevel
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

	public class BuildingLevel : UnitLevel
	{

	}
}