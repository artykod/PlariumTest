using Newtonsoft.Json;

namespace Game.Descriptors.Buildings
{
	public class MinionBarracksDescriptor : BarracksDescriptor
	{
		public new class Level : BarracksDescriptor.Level
		{
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