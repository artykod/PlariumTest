using System.Collections.Generic;

namespace Game.Logics.Abilities
{
	using Descriptors;
	using Characters;

	public class MeteorShower : Ability
	{
		public MeteorShower(GameController gameController, Descriptor descriptor) : base(gameController, descriptor)
		{
		}

		public override bool Activate(Vec2 point, Unit clickedUnit)
		{
			base.Activate(point, clickedUnit);

			if (base.Activate(point, clickedUnit))
			{
				IsActivated = true;

				var units = new List<Unit>();
				GameController.ForEachLogic<Character>(unit =>
				{
					if (unit.HP > 0 && unit.Team != Caster.Team && (unit.Position - point).LengthSqr < radiusSqr)
					{
						units.Add(unit);
					}
					return false;
				});
				AffectedUnits = units.ToArray();

				Execute();
			}

			return IsActivated;
		}
	}
}
