using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using Game;
using Game.Logics;

namespace ConsoleTest
{
	public class Program
	{
		public bool IsDone
		{
			get;
			set;
		}
		
		private string srcPath = AppDomain.CurrentDomain.BaseDirectory + @"/../../TestJsonData/";

		private void Run()
		{
			try
			{
				DebugImpl.Instance = new ConsoleDebug();
				var timeController = new ConsoleTimeController();
				TimeControllerImpl.Instance = timeController;
				StorageImpl.Instance = new ConsoleStorage();

				var files = new string[] {
					"Units/Buildings/Sofa.json",
					"Units/Buildings/BarracksArchers.json",
					"Units/Buildings/BarracksWarriors.json",
					"Units/Buildings/Fountain.json",
					"Units/Buildings/PortalArchers.json",
					"Units/Buildings/PortalWarriors.json",
					"Units/Buildings/PortalBosses.json",

					"Units/Characters/MainCharacter.json",
					"Units/Characters/MinionArcher.json",
					"Units/Characters/MinionWarrior.json",
					"Units/Characters/MobArcher.json",
					"Units/Characters/MobWarrior.json",
					"Units/Characters/MobBoss.json",

					"Units/Characters/Abilities/IceBolt.json",
					"Units/Characters/Abilities/MeteorShower.json",

					"Maps/Forest.json",
				};

				var filesContent = new List<string>();

				foreach (var file in files)
				{
					try
					{
						using (var reader = new StreamReader(srcPath + @"Descriptors/" + file))
						{
							var content = reader.ReadToEnd();
							if (!string.IsNullOrEmpty(content))
							{
								filesContent.Add(content);
							}
						}
					}
					catch (Exception e)
					{
						Debug.LogError(e.Message);
					}
				}

				var gameController = new GameController(filesContent.ToArray());
				gameController.OnLogicCreate += OnLogicCreate;
				gameController.OnLogicDestroy += OnLogicDestroy;
				gameController.RunWithMapId("map.forest");
				gameController.Map.Fountain.FetchHeroId("unit.main_character");

				while (!IsDone)
				{
					timeController.Update();
					Thread.Sleep(timeController.FrameDurationMs);
				}
			}
			catch (Exception e)
			{
				Debug.LogError("Program.Run catch exception");
				Debug.LogException(e);
			}
		}

		private void OnLogicCreate(Logic logic)
		{
			Debug.Log("create logic {0} is {1}", logic, logic.GameController.Map);
		}

		private void OnLogicDestroy(Logic logic)
		{
			Debug.Log("destroy logic {0} is {1}", logic, logic.GameController.Map);
		}

		private static void Main(string[] args)
		{
			var program = new Program();
			Task.Factory.StartNew(program.Run);
			Console.ReadKey();
			program.IsDone = true;
			//Console.ReadKey();
		}
	}
}
