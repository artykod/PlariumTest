using UnityEngine;
using UnityEngine.UI;

public abstract class UIDialogTextBase<T> : UIDialogGeneric<T> where T : UIDialogTextBase<T>
{
	[SerializeField]
	private Text titleText;
	[SerializeField]
	private Text messageText;
	[SerializeField]
	private LayoutGroup buttonsRoot;
	[SerializeField]
	private UIButton buttonTemplate;

	public T Build(string title, string message)
	{
		titleText.text = title;
		messageText.text = message;
		buttonTemplate.gameObject.SetActive(false);

		return this as T;
	}

	public T AddButton(string text, System.Action onClick = null)
	{
		var newButton = Instantiate(buttonTemplate);
		newButton.Text = text;
		newButton.OnClick += Close;
		newButton.OnClick += onClick;
		TransformTool.DropTo(newButton, buttonsRoot);
		newButton.gameObject.SetActive(true);

		return this as T;
	}
}

[PathInResources("Text")]
public class UIDialogText : UIDialogTextBase<UIDialogText>
{

}
