using UnityEngine;
using System.Collections;
using System;

[PathInResources(Constants.Paths.Objects.BUILDINGS)]
public abstract class Building : StaticObject
{
	[PathInResources(Constants.Paths.Visuals.BUILDINGS)]
	protected class BuildingVisualDescriptor : VisualDescriptor
	{
	}

	protected override VisualDescriptor BuildVisualDescriptor()
	{
		return new BuildingVisualDescriptor().SetPrefabName(PrefabName);
	}

	protected abstract string PrefabName
	{
		get;
	}
}
