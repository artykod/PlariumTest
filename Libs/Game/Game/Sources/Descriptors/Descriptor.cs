using System;
using Newtonsoft.Json;

namespace Game.Descriptors
{
	public abstract class Descriptor : Multiton<string, Descriptor>
	{
		private static readonly string assemblyName = typeof(Descriptor).AssemblyQualifiedName.Replace("Game.Descriptors.Descriptor, ", "");
		private static readonly string descriptorClassTemplate = "Game.Descriptors.{0}, " + assemblyName;

		private class ClassIdReader
		{
			[JsonProperty]
			public string ClassId
			{
				get;
				private set;
			}
		}

		[JsonProperty]
		public string Id
		{
			get;
			private set;
		}

		public virtual void Init()
		{
			SetInstance(Id, this);
		}

		public virtual void PostInit()
		{
		}

		public static Descriptor Deserialize(string json)
		{
			var classIdReader = JsonConvert.DeserializeObject<ClassIdReader>(json);
			var typeName = string.Format(descriptorClassTemplate, classIdReader.ClassId);
			var type = Type.GetType(typeName);

			return JsonConvert.DeserializeObject(json, type) as Descriptor;
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