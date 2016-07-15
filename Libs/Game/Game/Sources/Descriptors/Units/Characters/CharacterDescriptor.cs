using Newtonsoft.Json;

namespace Game.Descriptors
{
	public abstract class CharacterDescriptor : UnitDescriptor
	{
		public new class Level : UnitDescriptor.Level
		{
			/// <summary>
			/// Кол-во жизней.
			/// </summary>
			[JsonProperty]
			public int HP
			{
				get;
				private set;
			}
			/// <summary>
			/// Значение брони. от 0 до 1.
			/// Происходит процентное поглащение наносимого урона.
			/// </summary>
			[JsonProperty]
			public float Armor
			{
				get;
				private set;
			}
			/// <summary>
			/// Наносимый юнитом дамаг.
			/// </summary>
			[JsonProperty]
			public int Attack
			{
				get;
				private set;
			}
			/// <summary>
			/// Кол-во ударов в секунду.
			/// </summary>
			[JsonProperty]
			public float AttackSpeed
			{
				get;
				private set;
			}
			/// <summary>
			/// Дальность атаки.
			/// </summary>
			[JsonProperty]
			public float AttackRange
			{
				get;
				private set;
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