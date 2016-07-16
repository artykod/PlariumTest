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

	private void Awake()
	{
		Core.Instance.GameController.GameProgress.OnGoldChanged += OnGoldChanged;
	}

	private void OnGoldChanged(int newGold)
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
			transform.position = UIGame.UnitWorldToScreen(Logic, 2.5f);
		}
	}

	private void OnUpgradeClicked()
	{
		if (Logic != null)
		{
			var isMaxLevel = Logic.Level == Logic.Descriptor.Levels.Length - 1;
			if (!isMaxLevel)
			{
				var lvlUpCost = Logic.Descriptor.Levels[Logic.Level + 1].CostOfObtain;
				if (lvlUpCost > 0 && Core.Instance.GameController.GameProgress.Buy(lvlUpCost))
				{
					Logic.Level++;
				}
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
			var isMaxLevel = Logic.Level == Logic.Descriptor.Levels.Length - 1;
			var costOfObtain = isMaxLevel ? -1 : Logic.Descriptor.Levels[Logic.Level + 1].CostOfObtain;

			level = "Level " + (Logic.Level + 1);
			isButtonEnabled = !isMaxLevel && Core.Instance.GameController.GameProgress.Gold >= costOfObtain;
			nextLevelCost = isMaxLevel ? "Maximum level" : costOfObtain + " gold to lvl up";
		}

		levelText.text = level;
		nextLevelGoldText.text = nextLevelCost;
		upgradeButton.interactable = isButtonEnabled;
	}
}
