using UnityEngine;
using UnityEngine.UI;

public class UIDialogText : UIDialogGeneric<UIDialogText>
{
	[SerializeField]
	private Text titleText;
	[SerializeField]
	private Text messageText;
	[SerializeField]
	private LayoutGroup buttonsRoot;
	[SerializeField]
	private UIButton buttonTemplate;

	public UIDialogText Build(string title, string message)
	{
		titleText.text = title;
		messageText.text = message;
		buttonTemplate.gameObject.SetActive(false);

		return this;
	}

	public UIDialogText AddButton(string text, System.Action onClick = null)
	{
		var newButton = Instantiate(buttonTemplate);
		newButton.Text = text;
		newButton.OnClick += Close;
		newButton.OnClick += onClick;
		TransformTool.DropTo(newButton, buttonsRoot);
		newButton.gameObject.SetActive(true);

		return this;
	}
}
