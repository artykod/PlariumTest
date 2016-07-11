namespace Game.Logics.Characters
{
	using Descriptors;
	using Maps;
	using Buildings;

	public abstract class Character : Unit
	{
		public new CharacterDescriptor Descriptor
		{
			get
			{
				return base.Descriptor as CharacterDescriptor;
			}
		}

		public abstract CharacterLevel CurrentLevel
		{
			get;
		}

		private Map map;
		private MapDescriptor mapDescriptor;
		private float attackCooldown;
		private Unit targetUnit;
		private float newTargetSearchCooldown;
		private float targetDirectionOffset;
		private float directionOffsetLerp;

		public Character(GameController gameController, Descriptor descriptor) : base(gameController, descriptor)
		{
			map = GameController.Map;
			mapDescriptor = map.Descriptor;
			IsImmortal = false;
		}

		protected override void LevelChanged(int previousLevel, int newLevel)
		{
			base.LevelChanged(previousLevel, newLevel);

			HP = CurrentLevel.HP;
		}

		protected override void Update(float dt)
		{
			base.Update(dt);

			if (Position.x < 0f && Direction.x < 0f || Position.x > mapDescriptor.Width && Direction.x > 0f)
			{
				Direction = new Vec2(-Direction.x, Direction.y);
			}

			if (Position.y < 0f && Direction.y < 0f || Position.y > mapDescriptor.Height && Direction.y > 0f)
			{
				Direction = new Vec2(Direction.x, -Direction.y);
			}

			if (attackCooldown > 0f)
			{
				attackCooldown -= dt;
			}

			if (targetUnit != null)
			{
				if (targetUnit.HP <= 0)
				{
					targetUnit = null;
					newTargetSearchCooldown = 0f;
				}

				if (newTargetSearchCooldown <= 0f)
				{
					targetUnit = map.FindClosestEnemyUnit(this);
					newTargetSearchCooldown = GameRandom.Range(1f, 5f);
					targetDirectionOffset = GameRandom.Range(-0.5f, 0.5f);
				}
				else
				{
					newTargetSearchCooldown -= dt;
				}
			}

			directionOffsetLerp += (targetDirectionOffset - directionOffsetLerp) * 0.1f;

			var targetDistance = 1f;
			if (targetUnit != null)
			{
				targetDistance = CurrentLevel.AttackRange + targetUnit.Descriptor.Size + Descriptor.Size;
			}
			else
			{
				targetUnit = map.Fountain;
			}

			if (targetUnit != null)
			{
				var toTarget = targetUnit.Position - Position;
				Direction = toTarget;

				var currentDistance = toTarget.Length;
				CanMove = currentDistance > targetDistance;

				if (CanMove)
				{
					var perp = new Vec2(-Direction.y, Direction.x) * directionOffsetLerp;
					Direction += perp;
				}

				if (!CanMove && !(targetUnit is Fountain))
				{
					if (attackCooldown <= 0f)
					{
						targetUnit.TakeDamage(CurrentLevel.Attack);
						attackCooldown = 1f / CurrentLevel.AttackSpeed;
					}
				}
			}
		}
	}
}
