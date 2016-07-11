using System.Collections.Generic;

namespace Game.Logics.Maps
{
	using Descriptors;
	using Buildings;

	public class Map : Logic
	{
		private List<Unit> hpUnits = new List<Unit>();

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
				}

				unit.AttachToTeam(marker.Team);
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

		private void OnUnitDestroy(Logic logic)
		{
			logic.OnDestroy -= OnUnitDestroy;
			hpUnits.Remove(logic as Unit);
		}
	}
}
