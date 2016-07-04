using UnityEngine;
using System.Collections;
using System;

[ObjectId("BarracksArchers")]
public class BuildingBarracksArchers : BuildingBarracks
{
	protected override string PrefabName
	{
		get
		{
			return "BarracksArchers";
		}
	}

	protected override Type CharactersType
	{
		get
		{
			return typeof(CharacterMinionArcher);
		}
	}
}
