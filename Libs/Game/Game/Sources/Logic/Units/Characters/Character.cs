using System;

namespace Game.Logics.Characters
{
	using Descriptors;
	using Descriptors.Abilities;
	using Maps;
	using Abilities;

	public abstract class Character : Unit
	{
		private Map map;
		private MapDescriptor mapDescriptor;
		private float attackCooldown;
		private Unit targetUnit;
		private float newTargetSearchCooldown;
		private float targetDirectionOffset;
		private float directionOffsetLerp;
		private bool needAutoSearchTarget;

		public event Action<Character, Unit> OnAttack;

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

		/// <summary>
		/// Юнит, являющийся целью этого юнита.
		/// Если null, то у юнита нет цели для передвижения к ней.
		/// В случае миньона он будет двигаться к фонтану.
		/// </summary>
		public Unit TargetUnit
		{
			get
			{
				return targetUnit;
			}
		}

		/// <summary>
		/// Текущее значение брони.
		/// </summary>
		public float Armor
		{
			get;
			private set;
		}
		/// <summary>
		/// Текущее значение атаки.
		/// </summary>
		public int Attack
		{
			get;
			private set;
		}
		/// <summary>
		/// Текущее значение скорости передвижения.
		/// </summary>
		public float AttackSpeed
		{
			get;
			private set;
		}
		/// <summary>
		/// Абилки персонажа.
		/// </summary>
		public Ability[] Abilities
		{
			get;
			protected set;
		}

		public Character(GameController gameController, Descriptor descriptor) : base(gameController, descriptor)
		{
			Abilities = new Ability[0];
			map = GameController.Map;
			mapDescriptor = map.Descriptor;
			IsImmortal = false;
			needAutoSearchTarget = true;
		}

		public void SetTargetUnit(Unit target)
		{
			ChangeTargetUnit(target, false);
		}

		public override void Destroy()
		{
			base.Destroy();

			foreach (var i in Abilities)
			{
				i.Destroy();
			}
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
			TotalHP = HP = CurrentLevel.HP;

			base.LevelChanged(previousLevel, newLevel);
		}

		protected override void UpdateStats()
		{
			base.UpdateStats();

			var originalArmor = CurrentLevel.Armor;
			var originalAttack = CurrentLevel.Attack;
			var originalAttackSpeed = CurrentLevel.AttackSpeed;

			Armor = originalArmor;
			Attack = originalAttack;
			AttackSpeed = originalAttackSpeed;

			foreach (var i in modifiers)
			{
				switch (i.Modifier.Kind)
				{
				case Modifier.Kinds.Armor:
					Armor = ModifyValue(originalArmor, i.Modifier);
					break;
				case Modifier.Kinds.Attack:
					Attack = (int)ModifyValue(originalAttack, i.Modifier);
					break;
				case Modifier.Kinds.AttackSpeed:
					AttackSpeed = ModifyValue(originalAttackSpeed, i.Modifier);
					break;
				}
			}
			
		}

		protected override int ComputeDamage(int damageValue)
		{
			var damage = base.ComputeDamage(damageValue);

			// урон гасится на значение брони.
			damage -= (int)(damage * Armor);

			if (damage < 0)
			{
				damage = 0;
			}

			return damage;
		}

		protected override void Update(float dt)
		{
			base.Update(dt);

			// юниты не могут выйти за пределы карты.
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
				// цель ищется не сразу, чтобы куча юнитов синхронно не меняло направление.
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
				// если целевой юнит умер, то нужно начать поиск новой цели.
				if (!targetUnit.IsImmortal && targetUnit.HP <= 0)
				{
					ChangeTargetUnit(null, true);
					newTargetSearchCooldown = 0f;
				}
			}

			// для небольшого смещения направления движения, чтобы юниты не ходили совсем уж ровно по прямой.
			directionOffsetLerp += (targetDirectionOffset - directionOffsetLerp) * 0.1f;

			// расчет расстояния для атаки цели.
			var targetDistance = 1f;
			if (targetUnit != null)
			{
				// если цель - произвольная точка на карте, то к ней нужно подойти вплотную.
				targetDistance = (targetUnit.Team != Team ? CurrentLevel.AttackRange : 0f) +
					(targetUnit is MoveTarget ? 0f : targetUnit.Descriptor.Size + Descriptor.Size);
			}
			// если юнит из команды игрока, то при отсутствии цели он идет к фонтану.
			else if (map.Fountain.Team == Team)
			{
				targetUnit = map.Fountain;
			}
			// если цели нет, то юнит стоит на месте.
			else
			{
				CanMove = false;
				ChangeTargetUnit(null, true);
			}

			// если есть к чему двигаться.
			if (targetUnit != null)
			{
				var toTarget = targetUnit.Position - Position;
				Direction = toTarget;

				var currentDistance = toTarget.Length;
				// подошел ли на нужное расстояние.
				CanMove = currentDistance > targetDistance;

				if (CanMove)
				{
					var perp = new Vec2(-Direction.y, Direction.x) * directionOffsetLerp;
					Direction += perp;
				}

				if (!CanMove && targetUnit != map.Fountain)
				{
					// если подошел к цели, то наносит дамаг ей.
					if (attackCooldown <= 0f && !targetUnit.IsImmortal)
					{
						attackCooldown = 1f / AttackSpeed;
						targetUnit.TakeDamage(Attack);
						OnAttack.SafeInvoke(this, targetUnit);
					}
				}
			}
		}

		protected override void OnDie()
		{
			base.OnDie();

			// если умирающий юнит моб, то начислить опыт/голду.
			var mob = this as Mob;
			if (mob != null && map.Fountain.Hero != null && Team != map.Fountain.Team)
			{
				var mobLevel = mob.CurrentLevel;
				// герою опыт.
				map.Fountain.Hero.AddXP(mobLevel.XP);
				// игроку голда в прогресс.
				GameController.GameProgress.AddGold(mobLevel.Gold);
			}
		}
	}
}
