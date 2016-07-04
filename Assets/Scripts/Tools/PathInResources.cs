using System;

public class PathInResources : Attribute
{
	public string Path
	{
		get;
		private set;
	}

	public PathInResources(string path)
	{
		Path = path;
	}
}