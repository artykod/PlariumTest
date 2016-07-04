using UnityEngine;
using System;
using System.Collections;

public abstract class BuildingBarracks : Building
{
	protected abstract Type CharactersType
	{
		get;
	}

	protected override void Start()
	{
		base.Start();

		//StartCoroutine(EmitMinions());
	}

	private IEnumerator EmitMinions()
	{
		while (true)
		{
			var character = PrefabTool.CreateInstance<CharacterMinion>(CharactersType);
			if (character != null)
			{
				character.transform.position = transform.position;
			}

			yield return new WaitForSeconds(1f);
		}
	}
}
