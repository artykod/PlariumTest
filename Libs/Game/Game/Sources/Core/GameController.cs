using System;
using System.Collections;
using System.Collections.Generic;

namespace Game
{
	using Descriptors;
	using Logics;
	using Logics.Maps;

	public class GameController : BaseController
	{
		private Dictionary<string, Descriptor> descriptors = new Dictionary<string, Descriptor>();
		private List<Logic> justCreatedLogics = new List<Logic>();
		private LinkedList<Logic> allLogics = new LinkedList<Logic>();
		private LinkedList<Unit> selectedUnits = new LinkedList<Unit>();
		private string lastPlayedMap;

		public Action<Logic> OnLogicCreate;
		public Action<Logic> OnLogicDestroy;

		public Map Map
		{
			get;
			private set;
		}

		public bool IsRunned
		{
			get;
			private set;
		}

		public GameController(string[] descriptorsFilesContent)
		{
			foreach (var content in descriptorsFilesContent)
			{
				try
				{
					var descriptor = Descriptor.Parse(content);
					if (descriptor != null)
					{
						descriptors[descriptor.Id] = descriptor;
						descriptor.Init(this);
					}
				}
				catch (Exception e)
				{
					Debug.LogException(e);
				}
			}
			foreach (var desc in descriptors)
			{
				try
				{
					desc.Value.PostInit();
				}
				catch (Exception e)
				{
					Debug.LogException(e);
				}
			}

			TimeController.StartCoroutine(JustCreatedLogicsTracker());
		}

		public T FindDescriptorById<T>(string descriptorId) where T : Descriptor
		{
			return descriptors[descriptorId] as T;
		}

		public T CreateLogicByDescriptor<T>(Descriptor descriptor) where T : Logic
		{
			if (descriptor == null || string.IsNullOrEmpty(descriptor.LogicId))
			{
				return null;
			}

			var type = TypeTool.GetTypeByNameFromThisAssembly("Game.Logics." + descriptor.LogicId);
			if (type != null)
			{
				var logic = Activator.CreateInstance(type, this, descriptor) as T;
				if (logic != null)
				{
					justCreatedLogics.Add(logic);
				}
				return logic;
			}
			else
			{
				return null;
			}
		}

		public void RunWithMapId(string mapId)
		{
			Stop();

			lastPlayedMap = mapId;
			Map = CreateLogicByDescriptor<Map>(FindDescriptorById<MapDescriptor>(mapId));
			Map.Sofa.OnDestroy += OnGameEnd;

			TimeController.StartCoroutine(WaitGameRun());
		}

		private IEnumerator WaitGameRun()
		{
			yield return new TimeController.WaitForSeconds(2f);
			Run();
		}

		private IEnumerator JustCreatedLogicsTracker()
		{
			while (true)
			{
				yield return null;

				var newLogicsCount = justCreatedLogics.Count;
				if (newLogicsCount > 0)
				{
					for (int i = newLogicsCount - 1; i >= 0; i--)
					{
						var logic = justCreatedLogics[i];
						allLogics.AddLast(logic);
						logic.OnDestroy += LogicDestroy;
						logic.Start();
						OnLogicCreate.SafeInvoke(logic);
					}
					justCreatedLogics.Clear();
				}
			}
		}

		public void ForEachLogic<T>(Func<T, bool> func) where T : Logic
		{
			foreach (var i in allLogics)
			{
				try
				{
					var l = i as T;
					if (l != null)
					{
						if (func(l))
						{
							break;
						}
					}
				}
				catch (Exception e)
				{
					Debug.LogException(e);
				}
			}
		}

		public void SelectUnits(Unit[] units)
		{
			foreach (var i in selectedUnits)
			{
				if (i != null)
				{
					i.IsSelected = false;
				}
			}

			selectedUnits.Clear();

			foreach (var i in units)
			{
				if (i != null)
				{
					i.IsSelected = true;
					selectedUnits.AddLast(i);
				}
			}
		}

		private void OnGameEnd(Logic logic)
		{
			Debug.Log("Sofa dead. Game end.");

			RunWithMapId(lastPlayedMap);
		}

		private void LogicDestroy(Logic logic)
		{
			if (logic != null)
			{
				logic.OnDestroy -= LogicDestroy;
				OnLogicDestroy.SafeInvoke(logic);

				if (allLogics.Count > 0)
				{
					allLogics.Remove(logic);
				}

				if (logic is Unit)
				{
					selectedUnits.Remove(logic as Unit);
				}
			}
		}

		protected override void Start()
		{
			base.Start();

			IsRunned = true;
		}

		protected override void Update()
		{
			base.Update();

			foreach (var i in allLogics)
			{
				foreach (var j in allLogics)
				{
					if (i != j)
					{
						var unit1 = i as Unit;
						var unit2 = j as Unit;
						if (unit1 != null && unit2 != null)
						{
							var d = unit1.Position - unit2.Position;
							var len = 0f;
							if (Math.Abs(d.x) > 0.001f || Math.Abs(d.y) > 0.001f)
							{
								len = d.Length;
							}
							var size = unit1.Descriptor.Size + unit2.Descriptor.Size;

							if (len < size)
							{
								var mult = (size - len);
								if (!unit1.IsStatic && !unit2.IsStatic)
								{
									mult *= 0.5f;
								}
								if (len > 0f)
								{
									mult /= len;
								}
								d *= mult;

								if (!unit1.IsStatic)
								{
									unit1.Position += d;
								}

								if (!unit2.IsStatic)
								{
									unit2.Position -= d;
								}
							}
						}
					}
				}
			}
		}

		protected override void End()
		{
			base.End();

			var toRemove = new LinkedList<Logic>(allLogics);
			allLogics = new LinkedList<Logic>();
			foreach (var i in toRemove)
			{
				i.Destroy();
			}

			IsRunned = false;
		}
	}
}
