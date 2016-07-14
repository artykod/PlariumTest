using Newtonsoft.Json;

namespace Game.Descriptors.Buildings
{
	public class MobPortalDescriptor : BarracksDescriptor
	{
		public new class Level : BarracksDescriptor.Level
		{
			[JsonProperty]
			public float WaveDuration
			{
				get;
				private set;
			}
		}

		[JsonProperty]
		public float BetweenWavesTime
		{
			get;
			private set;
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