using System.Collections.Generic;

namespace Game.Logics.Buildings
{
	using Descriptors;
	using Descriptors.Characters;
	using Descriptors.Buildings;
	using Characters;

	/// <summary>
	/// Фонтан.
	/// </summary>
	public class Fountain : Building
	{
		private HeroDescriptor heroDescriptor;
		/// <summary>
		/// Время респауна главного героя.
		/// </summary>
		private float respawnTime;
		/// <summary>
		/// Таймер восстановления жизней юнитов рядом с фонтаном.
		/// </summary>
		private float healTime;
		/// <summary>
		/// Последнее состояние героя, т.к. требуется восстанавливать прокачку героя между его смертями.
		/// </summary>
		private Hero previousHeroState;

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
			heroDescriptor = GameController.FindDescriptorById<HeroDescriptor>(heroId);
			EmitHero();
		}

		private void EmitHero()
		{
			Hero = GameController.CreateLogicByDescriptor<Hero>(heroDescriptor);
			Hero.AttachToTeam(Team);
			Hero.Position = Position + new Vec2(0f, 2f);
			Hero.OnDestroy += OnHeroDie;
			Hero.MoveTo(Hero.Position);

			if (previousHeroState != null)
			{
				Hero.RestoreFromHero(previousHeroState);
			}
		}

		private void OnHeroDie(Logic logic)
		{
			if (logic == Hero)
			{
				previousHeroState = Hero;
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
				});
			}
			else
			{
				healTime -= dt;
			}
		}
	}
}
