using System;
using System.IO;

using Game.Descriptors;

namespace ConsoleTest
{
	public class Program
	{
		private string srcPath = AppDomain.CurrentDomain.BaseDirectory + @"/../../TestJsonData/";

		private void Run()
		{
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

				"Maps/Forest.json",
			};

			foreach (var file in files)
			{
				try
				{
					using (var reader = new StreamReader(srcPath + @"Descriptors/" + file))
					{
						Debug.Log(file);
						var descriptor = Descriptor.Deserialize(reader.ReadToEnd());
						Debug.Log("Deserialized");
						descriptor.Init();
						Debug.Log("Done");
					}
				}
				catch (Exception e)
				{
					Debug.LogError(e.Message);
				}
			}

			Descriptor.ForEach((desc) =>
			{
				try
				{
					desc.PostInit();
				}
				catch (Exception e)
				{
					Debug.LogError("{0} {1}", desc.Id, desc.JsonString, e.Message);
				}
			});

			Descriptor.ForEach((desc) => Debug.Log("{0}", desc.JsonString));
		}

		private static void Main(string[] args)
		{
			DebugImpl.Instance = new ConsoleDebug();

			var program = new Program();
			program.Run();

			Console.ReadKey();
		}
	}
}
