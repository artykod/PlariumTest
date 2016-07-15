using Newtonsoft.Json;

namespace Game.Descriptors
{
	public abstract class UnitDescriptor : Descriptor
	{
		/// <summary>
		/// Структура уровня прокачки юнита.
		/// </summary>
		public class Level
		{
			/// <summary>
			/// Идентификатор отображения текущего уровня.
			/// </summary>
			[JsonProperty]
			public string ViewId
			{
				get;
				private set;
			}
			/// <summary>
			/// Стоимость получения уровня (например, кол-во опыта или монет).
			/// </summary>
			[JsonProperty]
			public int CostOfObtain
			{
				get;
				private set;
			}
			/// <summary>
			/// Скорость передвижения юнита.
			/// </summary>
			[JsonProperty]
			public float Speed
			{
				get;
				private set;
			}
		}

		/// <summary>
		/// Список уровней прокачки юнита.
		/// </summary>
		[JsonIgnore]
		public Level[] Levels
		{
			get
			{
				return GetLevelsImpl<Level>();
			}
		}
		/// <summary>
		/// Размер юнита на поле.
		/// </summary>
		[JsonProperty]
		public float Size
		{
			get;
			private set;
		}

		/// <summary>
		/// Для возможности переопределения поля Levels на класс уровня конкретного типа дескриптора в угоду удобства использования api.
		/// </summary>
		/// <typeparam name="T">к какому типу уровня нужно привести список уровней прокачки.</typeparam>
		protected abstract T[] GetLevelsImpl<T>() where T : Level;
	}
}