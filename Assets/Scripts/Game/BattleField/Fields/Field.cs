using System;
using UnityEngine;

[PathInResources(Constants.Paths.Objects.FIELDS)]
public abstract class Field : StaticObject
{
	[PathInResources(Constants.Paths.Visuals.FIELDS)]
	protected class FieldVisualDescriptor : VisualDescriptor
	{
	}

	protected override VisualDescriptor BuildVisualDescriptor()
	{
		return new FieldVisualDescriptor().SetPrefabName(PrefabName);
	}

	protected abstract string PrefabName
	{
		get;
	}
}
