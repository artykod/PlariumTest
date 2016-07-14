using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Game
{
	using Descriptors;
	using Logics;
	using Logics.Maps;

	public class GameController : BaseController
	{
		private const string GAME_PROGRESS_KEY = "gameProgress";

		private Dictionary<string, Descriptor> descriptors = new Dictionary<string, Descriptor>();
		private List<Logic> justCreatedLogics = new List<Logic>();
		private LinkedList<Logic> allLogics = new LinkedList<Logic>();
		private LinkedList<Unit> selectedUnits = new LinkedList<Unit>();
		private float beforeGameTime;
		private bool isPreInitDone;

		public event Action<Logic> OnLogicCreate;
		public event Action<Logic> OnLogicDestroy;
		public event Action OnGameStart;
		public event Action<bool> OnGameEnd;

		public Map Map
		{
			get;
			private set;
		}

		public bool IsBattleStarted
		{
			get;
			private set;
		}

		public float BeforeGameTime
		{
			get
			{
				return beforeGameTime;
			}
		}

		public GameProgress GameProgress
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

			LoadProgress();
		}

		public void LoadProgress()
		{
			var data = Storage.LoadValueByKey(GAME_PROGRESS_KEY);
			if (!string.IsNullOrEmpty(data))
			{
				GameProgress = JsonConvert.DeserializeObject<GameProgress>(data);
			}
			else
			{
				GameProgress = new GameProgress();
			}
		}

		public void SaveProgress()
		{
			var data = JsonConvert.SerializeObject(GameProgress);
			Storage.SaveValueByKey(GAME_PROGRESS_KEY, data);
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
			if (IsRunned)
			{
				return;
			}

			Map = CreateLogicByDescriptor<Map>(FindDescriptorById<MapDescriptor>(mapId));
			Map.OnBattleDone += OnBattleDone;

			beforeGameTime = Map.Descriptor.DelayBeforeGame;
			IsBattleStarted = false;

			Run();
		}

		public void Surrender()
		{
			if (IsRunned)
			{
				OnBattleDone(false);
			}
		}

		private void OnBattleDone(bool isPlayerWin)
		{
			Map.OnBattleDone -= OnBattleDone;
			IsBattleStarted = false;
			Stop();
			OnGameEnd.SafeInvoke(isPlayerWin);

			SaveProgress();
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
		}

		protected override void Update()
		{
			if (beforeGameTime < 0f)
			{
				base.Update();
			}
			else
			{
				beforeGameTime -= TimeController.deltaTime;
				if (beforeGameTime < 0f)
				{
					IsBattleStarted = true;
					OnGameStart.SafeInvoke();
				}
			}

			if (!isPreInitDone)
			{
				isPreInitDone = true;
			}
			else
			{
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
								var len = d.LengthSqr;
								var size = unit1.Descriptor.Size + unit2.Descriptor.Size;

								if (len < size * size)
								{
									len = (float)Math.Sqrt(len);
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
		}

		protected override void End()
		{
			base.End();

			var toRemove = new LinkedList<Logic>(allLogics);

			allLogics = new LinkedList<Logic>();
			justCreatedLogics.Clear();
			selectedUnits.Clear();

			foreach (var i in toRemove)
			{
				i.Destroy();
			}
		}
	}
}
