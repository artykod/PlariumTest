using UnityEngine;
using Game.Logics.Abilities;

public class MeteorShowerView : AbilityView
{
	protected override void OnExecute(Ability ability)
	{
		base.OnExecute(ability);

		var meteorShower = ability as MeteorShower;
		if (meteorShower != null)
		{
			var from = new Vector3(Core.Instance.GameController.Map.Descriptor.Width / 2f, 30f, Core.Instance.GameController.Map.Descriptor.Height / 2f);
			var radius = meteorShower.CurrentLevel.Radius / 2f;
			for (int i = 0; i < 20; i++)
			{
				var shell = PrefabTool.CreateInstance<Shell>("Fireball");
				shell.Fire(from, new Vector3(meteorShower.ActivationPoint.x + GameRandom.Range(-radius, radius), 0f, meteorShower.ActivationPoint.y + GameRandom.Range(-radius, radius)), GameRandom.Range(0.25f, 1f));
			}
		}
	}
}
