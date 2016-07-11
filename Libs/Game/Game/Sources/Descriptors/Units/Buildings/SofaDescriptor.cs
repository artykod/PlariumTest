using Newtonsoft.Json;

namespace Game.Descriptors.Buildings
{
	public class SofaDescriptor : BuildingGenericDescriptor<SofaLevel>
	{
	}

	public class SofaLevel : BuildingLevel
	{
		[JsonProperty]
		public int HP
		{
			get;
			private set;
		}
	}
}