using Newtonsoft.Json;

public abstract class DescriptorBuilding<TLevel> : Descriptor where TLevel : class
{
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
	public TLevel[] Levels
	{
		get;
		private set;
	}
}
