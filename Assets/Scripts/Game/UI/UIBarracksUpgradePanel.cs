using UnityEngine;
using UnityEngine.UI;
using Game.Logics.Buildings;

public class UIBarracksUpgradePanel : MonoBehaviour
{
	[SerializeField]
	private Text levelText;
	[SerializeField]
	private UIButton upgradeButton;
	[SerializeField]
	private Text nextLevelGoldText;

	private Barracks logic;

	public Barracks Logic
	{
		get
		{
			return logic;
		}
		set
		{
			logic = value;
			Refresh();
			logic.OnLevelChanged += OnLevelChanged;
		}
	}

	private void OnLevelChanged(int prevLevel, int newLevel)
	{
		Refresh();
	}

	private void Start()
	{
		upgradeButton.OnClick += OnUpgradeClicked;
	}

	private void Update()
	{
		if (Logic != null)
		{
			transform.position = UIGame.UnitWorldToScreen(Logic, 1.5f);
		}
	}

	private void OnUpgradeClicked()
	{
		if (Logic != null)
		{
			var isMaxLevel = Logic.Level == Logic.Descriptor.Levels.Length - 1;
			if (!isMaxLevel)
			{
				// TODO check coins and decrease after lvl up
				//var nextLevelCost = Logic.Descriptor.Levels[Logic.Level + 1];

				Logic.Level++;
			}
		}
	}

	private void Refresh()
	{
		var level = "";
		var nextLevelCost = "";
		var isButtonEnabled = false;

		if (Logic != null)
		{
			level = "Level " + (Logic.Level + 1);
			var isMaxLevel = Logic.Level == Logic.Descriptor.Levels.Length - 1;
			isButtonEnabled = !isMaxLevel;
			nextLevelCost = isMaxLevel ? "Maximum level" : Logic.Descriptor.Levels[Logic.Level + 1].CostOfObtain + " gold for next lvl";
		}

		levelText.text = level;
		nextLevelGoldText.text = nextLevelCost;
		upgradeButton.interactable = isButtonEnabled;
	}
}
