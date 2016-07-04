using UnityEngine;
using System.Collections;
using System;

[ObjectId("Sofa")]
public class BuildingSofa : Building
{
	protected override string PrefabName
	{
		get
		{
			return "Sofa";
		}
	}
}
