using Newtonsoft.Json;

namespace Game.Descriptors.Characters
{
	public class HeroDescriptor : CharacterDescriptor
	{
		public new class Level : CharacterDescriptor.Level
		{
			[JsonProperty]
			public int TargetXP
			{
				get;
				private set;
			}

			[JsonProperty]
			public float RespawnTime // seconds
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