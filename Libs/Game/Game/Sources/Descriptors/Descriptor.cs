using Newtonsoft.Json;

namespace Game.Descriptors
{
	public abstract class Descriptor
	{
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

		[JsonProperty]
		public string Id
		{
			get;
			private set;
		}

		[JsonProperty]
		public string LogicId
		{
			get;
			private set;
		}

		public virtual void Init(GameController gameController)
		{
			GameController = gameController;
		}

		public virtual void PostInit()
		{
		}

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