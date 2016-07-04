using UnityEngine;
using System.Collections;
using System;

[ObjectId("Fountain")]
public class BuildingFountain : Building
{
	protected override string PrefabName
	{
		get
		{
			return "Fountain";
		}
	}
}
