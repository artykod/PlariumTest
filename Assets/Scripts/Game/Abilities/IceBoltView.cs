using UnityEngine;
using Game.Logics.Abilities;

public class IceBoltView : AbilityView
{
	protected override void OnExecute(Ability ability)
	{
		base.OnExecute(ability);
		
		if (ability != null)
		{
			var shell = PrefabTool.CreateInstance<Shell>("IceArrow");
			shell.Fire(ability.Caster, new Vector3(ability.ActivationPoint.x, 1f, ability.ActivationPoint.y), 0.25f);
		}
	}
}
