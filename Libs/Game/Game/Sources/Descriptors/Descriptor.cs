using Newtonsoft.Json;

namespace Game.Descriptors
{
	public abstract class Descriptor
	{
		/// <summary>
		/// Сначала будет считано имя класса дескриптора.
		/// Затем по нему произведена десериализация всех остальных данных.
		/// </summary>
		private class ClassIdReader
		{
			[JsonProperty]
			public string ClassId
			{
				get;
				private set;
			}
		}

		[JsonIgnore]
		public GameController GameController
		{
			get;
			private set;
		}

		/// <summary>
		/// Уникальный строковый идентификатор дескриптора (например, unit.mob.archer)
		/// </summary>
		[JsonProperty]
		public string Id
		{
			get;
			private set;
		}
		/// <summary>
		/// Идентификатор логики (например, имя класса).
		/// </summary>
		[JsonProperty]
		public string LogicId
		{
			get;
			private set;
		}

		/// <summary>
		/// Будет вызван в первую очередь после десериализации и передан игровой контроллер.
		/// </summary>
		/// <param name="gameController">активный игровой контроллер.</param>
		public virtual void Init(GameController gameController)
		{
			GameController = gameController;
		}
		/// <summary>
		/// Будет вызван после инициализации всех имеющихся дескрипторов.
		/// Полезен для установления связей между дескрипторами по id в json'ах.
		/// </summary>
		public virtual void PostInit()
		{
		}

		/// <summary>
		/// Распарсить дескриптор из json'а.
		/// </summary>
		/// <param name="json">json представление дескриптора.</param>
		/// <returns>распарсенный дескриптор.</returns>
		public static Descriptor Parse(string json)
		{
			var classIdReader = JsonConvert.DeserializeObject<ClassIdReader>(json);
			var classId = classIdReader.ClassId;
			var type = TypeTool.GetTypeByNameFromThisAssembly("Game.Descriptors." + classId + "Descriptor");
			var descriptor = JsonConvert.DeserializeObject(json, type) as Descriptor;
			return descriptor;
		}

		[JsonIgnore]
		public string JsonString
		{
			get
			{
				return JsonConvert.SerializeObject(this, Formatting.Indented);
			}
		}
	}
}