using System.Collections.Generic;

namespace Game.Logics.Buildings
{
	using Descriptors;
	using Descriptors.Buildings;
	using Characters;

	/// <summary>
	/// Фонтан.
	/// </summary>
	public class Fountain : Building
	{
		private Descriptor heroDescriptor;
		private float respawnTime;
		private float healTime;
		/// <summary>
		/// Последний уровень героя. Сохраняется между смертями героя в пределах боя.
		/// </summary>
		private int lastHeroLevel;
		/// <summary>
		/// Последнее значение опыта героя. Сохраняется между смертями героя в пределах боя.
		/// </summary>
		private int lastHeroXP;

		public new FountainDescriptor Descriptor
		{
			get
			{
				return base.Descriptor as FountainDescriptor;
			}
		}

		public new FountainDescriptor.Level CurrentLevel
		{
			get
			{
				return GetCurrentLevelImpl<FountainDescriptor.Level>();
			}
		}

		protected override T GetCurrentLevelImpl<T>()
		{
			return Descriptor.Levels[Level] as T;
		}

		/// <summary>
		/// Активный герой. Если null, то герой умер и еще не зареспаунился.
		/// </summary>
		public Hero Hero
		{
			get;
			private set;
		}
		/// <summary>
		/// Текущий таймер респауна героя.
		/// </summary>
		public float HeroRespawnTimer
		{
			get
			{
				return respawnTime;
			}
		}
		/// <summary>
		/// Общее время респауна героя.
		/// </summary>
		public float HeroTotalRespawnTime
		{
			get;
			private set;
		}

		public Fountain(GameController gameController, Descriptor descriptor) : base(gameController, descriptor)
		{
		}

		/// <summary>
		/// Назначить главного героя.
		/// </summary>
		/// <param name="heroId">ид дескриптора героя.</param>
		public void FetchHeroId(string heroId)
		{
			heroDescriptor = GameController.FindDescriptorById<Descriptor>(heroId);
			EmitHero();
		}

		private void EmitHero()
		{
			Hero = GameController.CreateLogicByDescriptor<Hero>(heroDescriptor);
			Hero.AttachToTeam(Team);
			Hero.Position = Position + Vec2.FromAngle(GameRandom.Range(0f, 360f)) * (Hero.Descriptor.Size + Descriptor.Size);
			Hero.OnDestroy += OnHeroDie;
			Hero.Level = lastHeroLevel;
			Hero.AddXP(lastHeroXP);
		}

		private void OnHeroDie(Logic logic)
		{
			if (logic == Hero)
			{
				lastHeroLevel = Hero.Level;
				lastHeroXP = Hero.XP;
				HeroTotalRespawnTime = respawnTime = Hero.Descriptor.Levels[Hero.Level].RespawnTime;
				Hero.OnDestroy -= OnHeroDie;
				Hero = null;
			}
		}

		protected override void Update(float dt)
		{
			base.Update(dt);

			if (Hero == null)
			{
				if (respawnTime < 0f)
				{
					EmitHero();
				}
				else
				{
					respawnTime -= dt;
				}
			}

			if (healTime < 0f)
			{
				healTime = 1f;

				var closestUnits = new LinkedList<Unit>();
				// проход по всем юнитам такой же команды и восстановление жизней, если те находятся рядом с фонтаном.
				GameController.ForEachLogic<Unit>(unit =>
				{
					if (unit.Team == Team)
					{
						var distanceSqr = (Position - unit.Position).LengthSqr;
						var needDistance = (Descriptor.Size + unit.Descriptor.Size) * 1.5f;
						if (distanceSqr <= needDistance * needDistance)
						{
							unit.Heal((int)(unit is Hero ? Descriptor.Levels[Level].HealSpeedHero : Descriptor.Levels[Level].HealSpeedMinion));
						}
					}

					return false;
				}));
			}
			else
			{
				healTime -= dt;
			}
		}
	}
}
