using Game.Logics;
using Game.Logics.Abilities;

[PathInResources(Constants.Paths.Views.ALL + "Abilities/")]
public class AbilityView : View
{
	protected Ability ability;

	public override void FetchLogic(Logic logic)
	{
		base.FetchLogic(logic);

		ability = logic as Ability;
		if (ability != null)
		{
			ability.OnExecute += OnExecute;
		}
	}

	protected virtual void OnExecute(Ability ability)
	{
	}
}
