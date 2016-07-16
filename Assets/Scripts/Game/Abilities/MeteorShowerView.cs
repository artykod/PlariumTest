using UnityEngine;
using Game.Logics.Abilities;

public class MeteorShowerView : AbilityView
{
	protected override void OnExecute(Ability ability)
	{
		base.OnExecute(ability);
		
		if (ability != null)
		{
			var from = new Vector3(Core.Instance.GameController.Map.Descriptor.Width / 2f, 30f, Core.Instance.GameController.Map.Descriptor.Height / 2f);
			var radius = ability.CurrentLevel.Radius / 2f;
			for (int i = 0; i < 20; i++)
			{
				var to = new Vector3(ability.ActivationPoint.x + GameRandom.Range(-radius, radius),
					0f, ability.ActivationPoint.y + GameRandom.Range(-radius, radius));
				var shell = PrefabTool.CreateInstance<Shell>("Fireball");
				shell.Fire(from, to, GameRandom.Range(0.25f, 1f), () => PrefabTool.CreateInstance<Explosion>().Fire(to));
			}
		}
	}
}
