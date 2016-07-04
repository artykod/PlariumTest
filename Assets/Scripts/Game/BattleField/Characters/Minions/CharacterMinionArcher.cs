using UnityEngine;
using System.Collections;
using System;

public class CharacterMinionArcher : CharacterMinion
{
	protected override string VisualPrefabName
	{
		get
		{
			return "Archer";
		}
	}
}
