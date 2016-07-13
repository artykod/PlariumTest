using System.Collections.Generic;

namespace Game.Logics.Buildings
{
	using Descriptors;
	using Descriptors.Buildings;
	using Logics.Characters;

	public class Fountain : Building
	{
		private Descriptor heroDescriptor;
		private float respawnTime;
		private float healTime;
		private int lastHeroLevel;
		private int lastHeroXP;

		public new FountainDescriptor Descriptor
		{
			get
			{
				return base.Descriptor as FountainDescriptor;
			}
		}

		public Hero Hero
		{
			get;
			private set;
		}

		public float HeroRespawnTimer
		{
			get
			{
				return respawnTime;
			}
		}

		public float HeroTotalRespawnTime
		{
			get;
			private set;
		}

		public Fountain(GameController gameController, Descriptor descriptor) : base(gameController, descriptor)
		{
		}

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
				GameController.ForEachLogic<Unit>(unit =>
				{
					if (unit.Team == Team)
					{
						var distanceSqr = (Position - unit.Position).LengthSqr;
						var needDistance = (Descriptor.Size + unit.Descriptor.Size) * 1.5f;
						if (distanceSqr <= needDistance * needDistance)
						{
							unit.Heal(unit is Hero ? Descriptor.Levels[Level].HealSpeedHero : Descriptor.Levels[Level].HealSpeedMinion);
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
