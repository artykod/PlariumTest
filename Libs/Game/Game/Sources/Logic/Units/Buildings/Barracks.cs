using System.Collections.Generic;

namespace Game.Logics.Buildings
{
	using Descriptors;
	using Descriptors.Buildings;
	using Characters;

	public class Barracks : Building
	{
		protected float mobEmitTimeCurrent;
		protected float mobEmitFrequency;
		protected LinkedList<Character> childMobs = new LinkedList<Character>();

		public new BarracksDescriptor Descriptor
		{
			get
			{
				return base.Descriptor as BarracksDescriptor;
			}
		}

		public Barracks(GameController gameController, Descriptor descriptor) : base(gameController, descriptor)
		{
		}

		protected override void LevelChanged(int previousLevel, int newLevel)
		{
			base.LevelChanged(previousLevel, newLevel);

			mobEmitTimeCurrent = mobEmitFrequency = 1f / Descriptor.Levels[newLevel].UnitsPerSecond;

			foreach (var i in childMobs)
			{
				i.Level = Level;
			}
		}

		protected void EmitMob()
		{
			if (!GameController.IsBattleStarted)
			{
				return;
			}

			var mob = GameController.CreateLogicByDescriptor<Character>(Descriptor.Unit);
			if (mob != null)
			{
				childMobs.AddLast(mob);
				mob.OnDestroy += OnMobDestroy;

				mob.AttachToTeam(Team);
				mob.Position = Position + Vec2.FromAngle(GameRandom.Range(0f, 360f)) * (Descriptor.Size + mob.Descriptor.Size);
				mob.Direction = new Vec2(GameRandom.value, GameRandom.value);
				mob.Level = Level;
			}
		}

		private void OnMobDestroy(Logic logic)
		{
			logic.OnDestroy -= OnMobDestroy;

			var mob = logic as Character;
			if (mob != null)
			{
				childMobs.Remove(mob);
			}
		}
	}
}
