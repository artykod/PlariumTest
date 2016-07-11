using Newtonsoft.Json;

namespace Game.Descriptors.Buildings
{
	public abstract class BuildingDescriptor : UnitDescriptor
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

	public abstract class BuildingGenericDescriptor<TLevel> : BuildingDescriptor where TLevel : BuildingLevel
	{
		[JsonProperty]
		public TLevel[] Levels
		{
			get;
			set;
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