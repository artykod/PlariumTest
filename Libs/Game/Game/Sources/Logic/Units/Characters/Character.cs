using System;

namespace Game.Logics.Characters
{
	using Descriptors;
	using Maps;

	public abstract class Character : Unit
	{
		public new CharacterDescriptor Descriptor
		{
			get
			{
				return base.Descriptor as CharacterDescriptor;
			}
		}

		public new CharacterDescriptor.Level CurrentLevel
		{
			get
			{
				return GetCurrentLevelImpl<CharacterDescriptor.Level>();
			}
		}

		public Unit TargetUnit
		{
			get
			{
				return targetUnit;
			}
		}

		private Map map;
		private MapDescriptor mapDescriptor;
		private float attackCooldown;
		private Unit targetUnit;
		private float newTargetSearchCooldown;
		private float targetDirectionOffset;
		private float directionOffsetLerp;
		private bool needAutoSearchTarget;

		public Character(GameController gameController, Descriptor descriptor) : base(gameController, descriptor)
		{
			map = GameController.Map;
			mapDescriptor = map.Descriptor;
			IsImmortal = false;
			needAutoSearchTarget = true;
		}

		public void SetTargetUnit(Unit target)
		{
			ChangeTargetUnit(target, false);
		}

		private void ChangeTargetUnit(Unit target, bool isAutoSearch)
		{
			needAutoSearchTarget = isAutoSearch;
			targetUnit = target;
			targetDirectionOffset = GameRandom.Range(-0.5f, 0.5f);

			if (!needAutoSearchTarget)
			{
				newTargetSearchCooldown = 0f;
			}
		}

		protected override void LevelChanged(int previousLevel, int newLevel)
		{
			base.LevelChanged(previousLevel, newLevel);

			TotalHP = HP = CurrentLevel.HP;
		}

		protected override int ComputeDamage(int damageValue)
		{
			var damage = base.ComputeDamage(damageValue);

			damage -= (int)(damage * CurrentLevel.Armor);

			if (damage < 0)
			{
				damage = 0;
			}

			return damage;
		}

		protected override void Update(float dt)
		{
			base.Update(dt);

			if (Position.x < 0f || Position.y < 0f || Position.x > mapDescriptor.Width || Position.y > mapDescriptor.Height)
			{
				Position = new Vec2(
					Math.Max(0f, Math.Min(mapDescriptor.Width, Position.x)),
					Math.Max(0f, Math.Min(mapDescriptor.Height, Position.y)));
			}

			if (attackCooldown > 0f)
			{
				attackCooldown -= dt;
			}

			if (needAutoSearchTarget)
			{
				if (newTargetSearchCooldown <= 0f)
				{
					var found = map.FindClosestEnemyUnit(this);
					if (found != null)
					{
						ChangeTargetUnit(found, true);
						newTargetSearchCooldown = GameRandom.Range(1f, 5f);
					}
				}
				else
				{
					newTargetSearchCooldown -= dt;
				}
			}

			if (targetUnit != null)
			{
				if (!targetUnit.IsImmortal && targetUnit.HP <= 0)
				{
					ChangeTargetUnit(null, true);
					newTargetSearchCooldown = 0f;
				}
			}

			directionOffsetLerp += (targetDirectionOffset - directionOffsetLerp) * 0.1f;

			var targetDistance = 1f;
			if (targetUnit != null)
			{
				targetDistance = (targetUnit.Team != Team ? CurrentLevel.AttackRange : 0f) +
					(targetUnit is MoveTarget ? 0f : targetUnit.Descriptor.Size + Descriptor.Size);
			}
			else if (map.Fountain.Team == Team)
			{
				targetUnit = map.Fountain;
			}
			else
			{
				CanMove = false;
				ChangeTargetUnit(null, true);
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

				if (!CanMove && targetUnit != map.Fountain)
				{
					if (attackCooldown <= 0f && !targetUnit.IsImmortal)
					{
						attackCooldown = 1f / CurrentLevel.AttackSpeed;
						if (targetUnit.TakeDamage(CurrentLevel.Attack))
						{
							var targetMob = targetUnit as Mob;
							if (targetMob != null && map.Fountain.Hero != null && Team == map.Fountain.Team)
							{
								var mobLevel = targetMob.Descriptor.Levels[targetMob.Level];
								map.Fountain.Hero.AddXP(mobLevel.XP);
								GameController.GameProgress.AddGold(mobLevel.Gold);
							}
						}
					}
				}
			}
		}
	}
}
