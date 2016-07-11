using Newtonsoft.Json;

namespace Game.Descriptors
{
	public abstract class MapDescriptor : Descriptor
	{
		public class Marker
		{
			[JsonProperty]
			public float X
			{
				get;
				private set;
			}

			[JsonProperty]
			public float Y
			{
				get;
				private set;
			}

			[JsonProperty]
			public string Team
			{
				get;
				private set;
			}

			[JsonProperty]
			private string ObjectId
			{
				get;
				set;
			}

			[JsonIgnore]
			public Descriptor Object
			{
				get;
				private set;
			}

			public void PostInit(GameController gameController)
			{
				Object = gameController.FindDescriptorById<Descriptor>(ObjectId);
			}
		}

		[JsonProperty]
		public string ViewId
		{
			get;
			private set;
		}

		[JsonProperty]
		public string Name
		{
			get;
			private set;
		}

		[JsonProperty]
		public string Description
		{
			get;
			private set;
		}

		[JsonProperty]
		public float Width
		{
			get;
			private set;
		}

		[JsonProperty]
		public float Height
		{
			get;
			private set;
		}

		[JsonProperty]
		public Marker[] Markers
		{
			get;
			private set;
		}

		public override void PostInit()
		{
			base.PostInit();

			foreach (var i in Markers)
			{
				if (i != null)
				{
					i.PostInit(GameController);
				}
			}
		}
	}
}