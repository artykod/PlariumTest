namespace Game.Logics.Abilities
{
	using Descriptors;
	using Characters;

	public class IceBolt : Ability
	{
		private Character targetUnit;

		public IceBolt(GameController gameController, Descriptor descriptor) : base(gameController, descriptor)
		{
		}

		public override bool Activate(Vec2 point, Unit clickedUnit)
		{
			if (base.Activate(point, clickedUnit))
			{
				var character = clickedUnit as Character;
				// абилка применима только к мобам противника.
				if (character != null && character.Team != Caster.Team)
				{
					targetUnit = character;
					Caster.SetTargetUnit(targetUnit);
					IsActivated = true;
				}
			}

			return IsActivated;
		}

		protected override void Update(float dt)
		{
			base.Update(dt);
			
			if (targetUnit != null)
			{
				if (targetUnit.HP > 0)
				{
					// если кастер дальше, чем расстояние для активации абилки, то ждем пока дойдет до него.
					var distance = (targetUnit.Position - Caster.Position).LengthSqr;
					if (distance <= radiusSqr)
					{
						ActivationPoint = targetUnit.Position;
						AffectedUnits = new Unit[] { targetUnit };
						targetUnit = null;

						Execute();
					}
				}
				else
				{
					targetUnit = null;
					Cancel();
				}
			}
		}
	}
}
