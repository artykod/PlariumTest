using Newtonsoft.Json;

namespace Game.Descriptors.Abilities
{
	public class AbilityDescriptor : Descriptor
	{
		public class Level
		{
			/// <summary>
			/// Радиус действия абилки.
			/// </summary>
			[JsonProperty]
			public float Radius
			{
				get;
				private set;
			}
			/// <summary>
			/// Время перезарядки абилки в секундах.
			/// </summary>
			[JsonProperty]
			public float Cooldown
			{
				get;
				private set;
			}
			/// <summary>
			/// Модификаторы абилки. Будут наложены на целевых юнитов.
			/// </summary>
			[JsonProperty]
			public Modificator[] Modificators
			{
				get;
				private set;
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

		[JsonProperty]
		public string ViewId
		{
			get;
			private set;
		}

		[JsonProperty]
		public string IconId
		{
			get;
			private set;
		}

		[JsonProperty]
		public Level[] Levels
		{
			get;
			private set;
		}
	}
}