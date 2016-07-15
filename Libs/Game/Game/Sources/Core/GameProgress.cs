using System;
using Newtonsoft.Json;

namespace Game
{
	/// <summary>
	/// Класс для хранения прогресса в игре.
	/// </summary>
	public class GameProgress
	{
		[JsonProperty]
		public int Gold
		{
			get;
			private set;
		}

		public event Action<int> OnGoldChanged;

		public void AddGold(int gold)
		{
			Gold += gold;
			OnGoldChanged.SafeInvoke(Gold);
		}

		public bool Buy(int price)
		{
			if (Gold >= price)
			{
				Gold -= price;
				OnGoldChanged.SafeInvoke(Gold);
				return true;
			}
			return false;
		}
	}
}
