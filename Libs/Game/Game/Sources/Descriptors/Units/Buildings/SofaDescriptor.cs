using Newtonsoft.Json;

namespace Game.Descriptors.Buildings
{
	public class SofaDescriptor : BuildingDescriptor
	{
		public new class Level : BuildingDescriptor.Level
		{
			[JsonProperty]
			public int HP
			{
				get;
				private set;
			}
		}

		[JsonProperty]
		public new Level[] Levels
		{
			get;
			private set;
		}

		protected override T[] GetLevelsImpl<T>()
		{
			return Levels as T[];
		}
	}
}