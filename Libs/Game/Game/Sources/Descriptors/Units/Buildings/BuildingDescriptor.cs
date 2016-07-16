using Newtonsoft.Json;

namespace Game.Descriptors.Buildings
{
	public abstract class BuildingDescriptor : UnitDescriptor
	{
		public new class Level : UnitDescriptor.Level
		{
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