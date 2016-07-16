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
			base.Activate(point, clickedUnit);
			
			var character = clickedUnit as Character;
			if (character != null && character.Team != Caster.Team)
			{
				targetUnit = character;
				Caster.SetTargetUnit(targetUnit);
				IsActivated = true;
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
