using Newtonsoft.Json;

namespace Game.Descriptors.Abilities
{
	/// <summary>
	/// Модификатор, накладываемый на юнитов.
	/// </summary>
	public class Modificator
	{
		/// <summary>
		/// Типы модифицируемых параметров.
		/// </summary>
		public enum Kinds
		{
			HP,
			Armor,
			Attack,
			AttackSpeed,
			Speed,
		}
		/// <summary>
		/// Способ модификации.
		/// </summary>
		public enum Types
		{
			/// <summary>
			/// Добавляет к общему значению.
			/// </summary>
			Add,
			/// <summary>
			/// Сокращает общее значение на какой-то процент (0 - 0%, 1 - 100%)
			/// </summary>
			Percent,
		}
		/// <summary>
		/// Когда срабатывает модификатор.
		/// </summary>
		public enum Triggers
		{
			/// <summary>
			/// Сразу единоразово после чего сразу удаляется.
			/// </summary>
			Now,
			/// <summary>
			/// Меняет на какой-то промежуток времени. Удаляется после этого времени.
			/// </summary>
			During,
		}
		/// <summary>
		/// К каким игрокам применим.
		/// </summary>
		public enum AffectedUnits
		{
			/// <summary>
			/// К юнитам игрока. Например, модификатор хила.
			/// </summary>
			Player,
			/// <summary>
			/// К юнитам врага. Например, разовый дамаг.
			/// </summary>
			Enemy,
		}

		[JsonProperty]
		public Kinds Kind
		{
			get;
			private set;
		}

		[JsonProperty]
		public Types Type
		{
			get;
			private set;
		}

		[JsonProperty]
		public Triggers Trigger
		{
			get;
			private set;
		}

		/// <summary>
		/// Время действия модификатора.
		/// Учитывается при триггере During.
		/// </summary>
		[JsonProperty]
		public float TriggerTime
		{
			get;
			private set;
		}

		[JsonProperty]
		public AffectedUnits AffectedUnit
		{
			get;
			private set;
		}

		/// <summary>
		/// Значение модификатора (абсолютно/процент в зависимости от способа модификации).
		/// </summary>
		[JsonProperty]
		public float Value
		{
			get;
			private set;
		}
	}
}