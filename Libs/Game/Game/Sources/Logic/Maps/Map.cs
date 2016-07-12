using System;
using System.Collections.Generic;

namespace Game.Logics.Maps
{
	using Descriptors;
	using Buildings;

	public class Map : Logic
	{
		private List<Unit> hpUnits = new List<Unit>();
		private List<MobPortal> mobPortals = new List<MobPortal>();
		private bool isBattleDone;

		public event Action<bool> OnBattleDone;

		public new MapDescriptor Descriptor
		{
			get
			{
				return base.Descriptor as MapDescriptor;
			}
		}

		public Fountain Fountain
		{
			get;
			private set;
		}

		public Sofa Sofa
		{
			get;
			private set;
		}

		public Map(GameController gameController, Descriptor descriptor) : base(gameController, descriptor)
		{
			GameController.OnLogicCreate += logic =>
			{
				var unit = logic as Unit;
				if (unit != null)
				{
					if (!unit.IsImmortal)
					{
						hpUnits.Add(unit);
						unit.OnDestroy += OnUnitDestroy;
					}
				}
			};

			foreach (var marker in Descriptor.Markers)
			{
				var unit = GameController.CreateLogicByDescriptor<Unit>(marker.Object);
				unit.Position = new Vec2(marker.X, marker.Y);

				if (unit is Fountain)
				{
					Fountain = unit as Fountain;
				}

				if (unit is Sofa)
				{
					Sofa = unit as Sofa;
					Sofa.OnDestroy += OnSofaDestroy;
				}

				if (unit is MobPortal)
				{
					mobPortals.Add(unit as MobPortal);
				}

				unit.AttachToTeam(marker.Team);
			}
		}

		private void OnSofaDestroy(Logic logic)
		{
			if (!isBattleDone)
			{
				isBattleDone = true;
				OnBattleDone.SafeInvoke(false);
			}
		}

		public Unit FindClosestEnemyUnit(Unit searcher)
		{
			var result = default(Unit);
			var minDistance = float.MaxValue;
			var center = searcher.Position;

			for (int i = 0; i < hpUnits.Count; i++)
			{
				var unit = hpUnits[i];
				if (unit.Team != searcher.Team)
				{
					var distance = Vec2.DistanceSqr(center, unit.Position);
					if (distance < minDistance)
					{
						minDistance = distance;
						result = unit;
					}
				}
			}

			return result;
		}

		protected override void Update(float dt)
		{
			base.Update(dt);

			if (!isBattleDone)
			{
				var allPortalsEnds = true;
				for (int i = 0, l = mobPortals.Count; i < l; i++)
				{
					if (!mobPortals[i].IsWavesEnds)
					{
						allPortalsEnds = false;
						break;
					}
				}

				if (allPortalsEnds)
				{
					var hasAnyEnemy = false;
					for (int i = 0, l = hpUnits.Count; i < l; i++)
					{
						if (hpUnits[i].Team != Sofa.Team)
						{
							hasAnyEnemy = true;
							break;
						}
					}

					if (!hasAnyEnemy)
					{
						isBattleDone = true;
						OnBattleDone.SafeInvoke(true);
					}
				}
			}
		}

		private void OnUnitDestroy(Logic logic)
		{
			var unit = logic as Unit;

			logic.OnDestroy -= OnUnitDestroy;
			hpUnits.Remove(unit);
		}
	}
}
