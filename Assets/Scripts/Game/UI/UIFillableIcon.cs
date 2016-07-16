using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class UIFillableIcon : MonoBehaviour, IPointerClickHandler
{
	[SerializeField]
	private Image iconImage;
	[SerializeField]
	private Image iconTonerImage;

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
}
