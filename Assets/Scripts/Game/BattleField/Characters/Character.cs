using UnityEngine;
using System.Collections;
using System;

[PathInResources(Constants.Paths.Objects.CHARACTERS)]
public abstract class Character : DynamicObject
{
	[PathInResources(Constants.Paths.Visuals.CHARACTERS)]
	protected class CharacterVisualDescriptor : VisualDescriptor
	{
	}

	protected override VisualDescriptor BuildVisualDescriptor()
	{
		return new CharacterVisualDescriptor().SetPrefabName(VisualPrefabName);
	}

	protected abstract string VisualPrefabName
	{
		get;
	}
}
