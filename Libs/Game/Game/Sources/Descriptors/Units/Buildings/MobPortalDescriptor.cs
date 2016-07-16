using Newtonsoft.Json;

namespace Game.Descriptors.Buildings
{
	public class MobPortalDescriptor : BarracksDescriptor
	{
		/// <summary>
		/// Уровень прокачки портала мобов служит волной.
		/// </summary>
		public new class Level : BarracksDescriptor.Level
		{
			/// <summary>
			/// Длительность волны в секундах.
			/// В это время будет происходить генерация мобов с заданной частотой в родительских настройках.
			/// </summary>
			[JsonProperty]
			public float WaveDuration
			{
				get;
				private set;
			}
		}

		/// <summary>
		/// Промежуток между волнами в секундах.
		/// </summary>
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