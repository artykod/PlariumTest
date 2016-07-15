using Newtonsoft.Json;

namespace Game.Descriptors.Buildings
{
	public class FountainDescriptor : BuildingDescriptor
	{
		public new class Level : BuildingDescriptor.Level
		{
			/// <summary>
			/// На сколько восстанавливает жизни игрока в секунду.
			/// </summary>
			[JsonProperty]
			public int HealSpeedHero
			{
				get;
				private set;
			}
			/// <summary>
			/// На сколько восстанавливает жизни миньонов в секунду.
			/// </summary>
			[JsonProperty]
			public int HealSpeedMinion
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