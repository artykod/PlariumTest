using UnityEngine;
using Game.Logics.Abilities;

public class IceBoltView : AbilityView
{
	protected override void OnExecute(Ability ability)
	{
		base.OnExecute(ability);

		var iceBolt = ability as IceBolt;
		if (iceBolt != null)
		{
			var shell = PrefabTool.CreateInstance<Shell>("IceArrow");
			shell.Fire(iceBolt.Caster, new Vector3(iceBolt.ActivationPoint.x, 1f, iceBolt.ActivationPoint.y), 0.25f);
		}
	}
}
