using Newtonsoft.Json;

namespace Game.Descriptors.Buildings
{
	public class FountainDescriptor : BuildingDescriptor
	{
		public new class Level : BuildingDescriptor.Level
		{
			[JsonProperty]
			public int HealSpeedHero // per second
			{
				get;
				private set;
			}

			[JsonProperty]
			public int HealSpeedMinion // per second
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