using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Game
{
	using Descriptors;
	using Logics;
	using Logics.Maps;

	/// <summary>
	/// Игровой контроллер.
	/// Управляет полем боя, загрузкой карт.
	/// </summary>
	public class GameController : BaseController
	{
		private const string GAME_PROGRESS_KEY = "gameProgress";

		/// <summary>
		/// Контейнер со всеми дескрипторами сущностей.
		/// По нему ищет и выдает всем запрашивающим дескрипторы.
		/// </summary>
		private Dictionary<string, Descriptor> descriptors = new Dictionary<string, Descriptor>();
		/// <summary>
		/// Список только что созданной логики.
		/// На следующий кадр после создания экземпляров будет разослано событие об их создании.
		/// После этого список очистится.
		/// </summary>
		private List<Logic> justCreatedLogics = new List<Logic>();
		/// <summary>
		/// Список всех активных логик.
		/// </summary>
		private LinkedList<Logic> allLogics = new LinkedList<Logic>();
		/// <summary>
		/// Список выделенных юнитов.
		/// Хранится для того, чтобы при новом выделении убрать выделение с прошлых выделенных.
		/// </summary>
		private LinkedList<Unit> selectedUnits = new LinkedList<Unit>();
		/// <summary>
		/// Стартовая задержка перед боем.
		/// </summary>
		private float beforeGameTime;
		/// <summary>
		/// Вещание событий о создании логики идут после первого кадра игры.
		/// </summary>
		private bool isPreInitDone;

		/// <summary>
		/// Событие на создание логики.
		/// </summary>
		public event Action<Logic> OnLogicCreate;
		/// <summary>
		/// Событие на удаление логики.
		/// </summary>
		public event Action<Logic> OnLogicDestroy;
		/// <summary>
		/// Событие на старт боя (после стартовой задержки).
		/// </summary>
		public event Action OnGameStart;
		/// <summary>
		/// Событие на окончание боя.
		/// В параметре приходит результат боя: true - выиграл игрок, false - игрок проиграл.
		/// </summary>
		public event Action<bool> OnGameEnd;

		/// <summary>
		/// Логика активной карты.
		/// </summary>
		public Map Map
		{
			get;
			private set;
		}
		/// <summary>
		/// Запущен ли бой.
		/// </summary>
		public bool IsBattleStarted
		{
			get;
			private set;
		}
		/// <summary>
		/// Текущее значение задержки перед боем.
		/// </summary>
		public float BeforeGameTime
		{
			get
			{
				return beforeGameTime;
			}
		}
		/// <summary>
		/// Текущий прогресс игрока.
		/// </summary>
		public GameProgress GameProgress
		{
			get;
			private set;
		}

		/// <summary>
		/// Создание игрового контроллера.
		/// </summary>
		/// <param name="descriptorsFilesContent">список json всех дескрипторов сущностей.</param>
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

		/// <summary>
		/// Загрузить прогресс из постоянного хранилища игры.
		/// </summary>
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
		/// <summary>
		/// Сохранить текущий прогресс в хранилище игры.
		/// </summary>
		public void SaveProgress()
		{
			var data = JsonConvert.SerializeObject(GameProgress);
			Storage.SaveValueByKey(GAME_PROGRESS_KEY, data);
		}

		/// <summary>
		/// Поиск дескриптора по его строковому идентификатору.
		/// </summary>
		/// <typeparam name="T">тип искомого дескриптора.</typeparam>
		/// <param name="descriptorId">строковый идентификатор.</param>
		/// <returns>найденный дескриптор</returns>
		public T FindDescriptorById<T>(string descriptorId) where T : Descriptor
		{
			return descriptors[descriptorId] as T;
		}
		/// <summary>
		/// Создание экземпляра логики по дескриптору.
		/// </summary>
		/// <typeparam name="T">тип создаваемой логики.</typeparam>
		/// <param name="descriptor">дескриптор.</param>
		/// <returns>созданный экземпляр логики.</returns>
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

		/// <summary>
		/// Запускает контроллер и загружает карту по ее идентификатору (Id дескриптора).
		/// </summary>
		/// <param name="mapId">id дескриптора карты.</param>
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

		/// <summary>
		/// Игрок сдался.
		/// </summary>
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

		/// <summary>
		/// Проход по всем логикам заданного типа.
		/// Проход по коллекции идет напрямую поэтому нельзя создавать/удалять новые сущности внутри колбека.
		/// Рекоммендуется сохранять нужные экземпляры в свою коллекцию и после этого ими оперировать.
		/// </summary>
		/// <typeparam name="T">тип искомых логик.</typeparam>
		/// <param name="func">колбек на обработку найденной логики.</param>
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
		/// <summary>
		/// Выделить юнитов.
		/// </summary>
		/// <param name="units">список выделяемых юнитов.</param>
		public void SelectUnits(Unit[] units)
		{
			// прошлые выделенные юниты сбрасывают выделение.
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
		/// <summary>
		/// Выделить юнита.
		/// </summary>
		/// <param name="unit">выделяемый юнит.</param>
		public void SelectUnit(Unit unit)
		{
			SelectUnits(new Unit[] { unit });
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
				// пропуск первого кадра после старта контроллера.
				isPreInitDone = true;
			}
			else
			{
				var newLogicsCount = justCreatedLogics.Count;
				if (newLogicsCount > 0)
				{
					// т.к. во время вещания события создания могут быть созданы другие сущности
					var copy = justCreatedLogics;
					justCreatedLogics = new List<Logic>();
					for (int i = newLogicsCount - 1; i >= 0; i--)
					{
						var logic = copy[i];
						allLogics.AddLast(logic);
						logic.OnDestroy += LogicDestroy;
						logic.Start();
						OnLogicCreate.SafeInvoke(logic);
					}
				}

				// для того, чтобы юниты не налезали друг на друга реализована простетская физика на кружочках.
				// дескриптор юнита содержит радиус, по этим радиусам они рассталкивают друг друга, если соприкасаются.
				// TODO оптимизацию с разбиением
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

									// есть статичные юниты (здания например)
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
