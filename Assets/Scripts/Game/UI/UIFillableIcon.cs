using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class UIFillableIcon : MonoBehaviour, IPointerClickHandler
{
	[SerializeField]
	private Image iconImage;
	[SerializeField]
	private Image iconTonerImage;
	[SerializeField]
	private Text lvlText;

	protected abstract float FillAmount
	{
		get;
	}

	void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
	{
		OnClick();
	}

	protected virtual void OnClick()
	{
	}

	protected virtual void Update()
	{
		iconTonerImage.fillAmount = FillAmount;
	}

	protected void SetIcon(Sprite sprite)
	{
		iconImage.sprite = iconTonerImage.sprite = sprite;
	}

	protected void SetLevel(int level)
	{
		lvlText.text = "lvl " + (level + 1);
	}

	protected virtual void Awake()
	{
	}
}
