using System;

public class ObjectId : Attribute
{
	public string Id
	{
		get;
		private set;
	}

	public ObjectId(string id)
	{
		Id = id;
	}
}
