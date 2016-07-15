using Newtonsoft.Json;

namespace Game.Descriptors.Characters
{
	public class MobDescriptor : CharacterDescriptor
	{
		public new class Level : CharacterDescriptor.Level
		{
			/// <summary>
			/// Получаемое за убийство золото.
			/// </summary>
			[JsonProperty]
			public int Gold
			{
				get;
				private set;
			}
			/// <summary>
			/// Получаемый за убийство опыт.
			/// </summary>
			[JsonProperty]
			public int XP
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