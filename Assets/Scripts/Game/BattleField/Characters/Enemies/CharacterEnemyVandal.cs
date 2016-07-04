using UnityEngine;
using System.Collections;
using System;

[ObjectId("CharacterVandal")]
public class CharacterEnemyVandal : CharacterEnemy
{
	private string[] visualPrefabNames = {
		"Vandal1",
		"Vandal2",
		"Vandal3",
	};

	protected override string VisualPrefabName
	{
		get
		{
			return GameRandom.FromArray(visualPrefabNames);
		}
	}
}
