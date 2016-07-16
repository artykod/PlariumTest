using Game.Logics;
using Game.Logics.Characters;
using UnityEngine;

public class CharacterView : UnitView {
	protected override float RotationLerpSpeed
	{
		get
		{
			return 0.1f;
		}
	}

	public override void FetchLogic(Logic logic)
	{
		base.FetchLogic(logic);

		(logic as Character).OnAttack += OnAttack;
	}

	protected override void OnLogicDestroy(Logic logic)
	{
		base.OnLogicDestroy(logic);

		(logic as Character).OnAttack -= OnAttack;
	}

	private void OnAttack(Character self, Unit attackedUnit)
	{
		if (self.Descriptor.IsRangedAttack)
		{
			var shell = PrefabTool.CreateInstance<Shell>("Arrow");
			shell.Fire(new Vector3(self.Position.x, 1f, self.Position.y), new Vector3(attackedUnit.Position.x, 0f, attackedUnit.Position.y), 0.25f);
		}
	}
}
