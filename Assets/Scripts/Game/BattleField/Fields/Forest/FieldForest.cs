using System;
using UnityEngine;

public class FieldForest : Field
{
	[SerializeField]
	private Transform sofaRoot;
	[SerializeField]
	private Transform warriorsRoot;
	[SerializeField]
	private Transform archersRoot;
	[SerializeField]
	private Transform fountainsRoot;
	[SerializeField]
	private Transform[] enemiesRoots;

	protected override string PrefabName
	{
		get
		{
			return "FieldForest";
		}
	}

	protected override void Awake()
	{
		base.Awake();

		PrefabTool.CreateInstance<BuildingSofa>().DropTo(sofaRoot);
		PrefabTool.CreateInstance<BuildingBarracksWarriors>().DropTo(warriorsRoot);
		PrefabTool.CreateInstance<BuildingBarracksArchers>().DropTo(archersRoot);
		PrefabTool.CreateInstance<BuildingFountain>().DropTo(fountainsRoot);
		foreach (var i in enemiesRoots)
		{
			PrefabTool.CreateInstance<BuildingBarracksArchers>().DropTo(i);
		}
	}
}
