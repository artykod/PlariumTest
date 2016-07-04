using Newtonsoft.Json;

public abstract class Descriptor : Multiton<string, Descriptor>
{
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
}
