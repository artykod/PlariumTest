using Newtonsoft.Json;

namespace Game.Descriptors.Characters
{
	public class MobDescriptor : CharacterDescriptor
	{
		public new class Level : CharacterDescriptor.Level
		{
			[JsonProperty]
			public int Gold
			{
				get;
				private set;
			}
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