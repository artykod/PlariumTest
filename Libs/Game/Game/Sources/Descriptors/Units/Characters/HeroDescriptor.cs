using Newtonsoft.Json;

namespace Game.Descriptors.Characters
{
	using Abilities;

	public class HeroDescriptor : CharacterDescriptor
	{
		public new class Level : CharacterDescriptor.Level
		{
			/// <summary>
			/// Время респауна героя после смерти (в секундах).
			/// </summary>
			[JsonProperty]
			public float RespawnTime
			{
				get;
				private set;
			}
			/// <summary>
			/// Значение опыта, необходимое для получения опыта.
			/// </summary>
			[JsonProperty]
			public int TargetXP {
				get;
				private set;
			}
		}

		[JsonProperty]
		public string IconId
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

		[JsonProperty]
		private string[] AbilitiesIds
		{
			get;
			set;
		}

		[JsonIgnore]
		public AbilityDescriptor[] Abilities
		{
			get;
			private set;
		}

		protected override T[] GetLevelsImpl<T>()
		{
			return Levels as T[];
		}

		public override void PostInit()
		{
			base.PostInit();

			Abilities = new AbilityDescriptor[AbilitiesIds.Length];
			for (int i = 0; i < Abilities.Length; i++)
			{
				Abilities[i] = GameController.FindDescriptorById<AbilityDescriptor>(AbilitiesIds[i]);
			}
		}
	}
}