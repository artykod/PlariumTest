using UnityEngine;
using System.Collections;
using System;

[ObjectId("BarracksWarriors")]
public class BuildingBarracksWarriors : BuildingBarracks
{
	protected override string PrefabName
	{
		get
		{
			return "BarracksWarriors";
		}
	}

	protected override Type CharactersType
	{
		get
		{
			return typeof(CharacterMinionWarrior);
		}
	}
}
