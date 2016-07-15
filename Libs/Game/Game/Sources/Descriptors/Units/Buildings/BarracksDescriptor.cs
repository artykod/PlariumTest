using Newtonsoft.Json;

namespace Game.Descriptors.Buildings
{
	using Characters;

	public abstract class BarracksDescriptor : BuildingDescriptor
	{
		public new class Level : BuildingDescriptor.Level
		{
			/// <summary>
			/// Кол-во производимых юнитов в секунду.
			/// </summary>
			[JsonProperty]
			public float UnitsPerSecond
			{
				get;
				private set;
			}
			/// <summary>
			/// Уровень прокачки казармы определяет уровень прокачки юнитов.
			/// </summary>
			[JsonProperty]
			private int LevelOfUnits
			{
				get;
				set;
			}
			/// <summary>
			/// Ссылки на дескрипторы уровня прокачки юнита.
			/// </summary>
			[JsonIgnore]
			public MobDescriptor.Level MobsLevel
			{
				get;
				private set;
			}

			public void PostInit(BarracksDescriptor barracks)
			{
				MobsLevel = barracks.Unit.Levels[LevelOfUnits - 1];
			}
		}

		[JsonIgnore]
		public new Level[] Levels
		{
			get
			{
				return GetLevelsImpl<Level>();
			}
		}

		/// <summary>
		/// Идентификатор дескриптора генерируемых юнитов.
		/// </summary>
		[JsonProperty]
		private string UnitId
		{
			get;
			set;
		}
		/// <summary>
		/// Ссылка на дескриптор генерируемых юнитов.
		/// </summary>
		[JsonIgnore]
		public MobDescriptor Unit
		{
			get;
			private set;
		}

		public override void PostInit()
		{
			base.PostInit();

			Unit = GameController.FindDescriptorById<MobDescriptor>(UnitId);

			foreach (var i in Levels)
			{
				i.PostInit(this);
			}
		}

		protected override T[] GetLevelsImpl<T>()
		{
			return Levels as T[];
		}
	}
}