namespace Game.Logics.Buildings
{
	using Descriptors;
	using Logics.Characters;

	public class Fountain : Building
	{
		public Hero Hero
		{
			get;
			private set;
		}

		private Descriptor heroDescriptor;

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
		}

		private void OnHeroDie(Logic logic)
		{
			if (logic == Hero)
			{
				var respawnTime = Hero.Descriptor.Levels[Hero.Level].RespawnTime;
				Hero.OnDestroy -= OnHeroDie;
				Hero = null;
				TimeController.StartCoroutine(WaitForHeroEmit(respawnTime));
			}
		}

		private System.Collections.IEnumerator WaitForHeroEmit(float delay)
		{
			yield return new TimeController.WaitForSeconds(delay);
			EmitHero();
		}
	}
}
