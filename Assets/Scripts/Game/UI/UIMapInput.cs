using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Logics;
using Game.Logics.Characters;

/// <summary>
/// Класс, управляющий выделением и назначением целей юнитам.
/// </summary>
public class UIMapInput : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	[SerializeField]
	private Image selectionPanel;

	private GameController gameController;
	private Canvas canvas;
	private Vector2 leftTopOfSelection;
	private Vector2 rightBottomOfSelection;

	private void Awake()
	{
		canvas = GetComponentInParent<Canvas>();
		selectionPanel.enabled = false;

		gameController = Core.Instance.GameController;
	}

	void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
	{
		if (!gameController.IsBattleStarted || Core.Instance.UIGame.HeroHUD.IsAbilitySelected)
		{
			return;
		}

		if (eventData.button == PointerEventData.InputButton.Left)
		{
			selectionPanel.enabled = true;
			leftTopOfSelection = eventData.position;
		}
	}

	void IDragHandler.OnDrag(PointerEventData eventData)
	{
		if (!gameController.IsBattleStarted || Core.Instance.UIGame.HeroHUD.IsAbilitySelected)
		{
			return;
		}

		if (eventData.button == PointerEventData.InputButton.Left)
		{
			rightBottomOfSelection = eventData.position;
			var r = GetDrawingRect(leftTopOfSelection / canvas.scaleFactor, rightBottomOfSelection / canvas.scaleFactor);
			var selectionRT = selectionPanel.rectTransform;
			selectionRT.anchoredPosition = new Vector2(r.x, Screen.height - r.y);
			selectionRT.sizeDelta = new Vector2(r.width, r.height);
		}
	}

	void IEndDragHandler.OnEndDrag(PointerEventData eventData)
	{
		if (!gameController.IsBattleStarted || Core.Instance.UIGame.HeroHUD.IsAbilitySelected)
		{
			return;
		}

		if (eventData.button == PointerEventData.InputButton.Left)
		{
			rightBottomOfSelection = eventData.position;
			SelectUnitsInCurrentSelection();
		}
	}

	void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
	{
		if (!gameController.IsBattleStarted || (eventData.dragging && eventData.button == PointerEventData.InputButton.Left))
		{
			return;
		}

		var clickArea = new Vector2(30f, 40f);
		leftTopOfSelection = eventData.position - clickArea;
		rightBottomOfSelection = eventData.position + clickArea;

		if (eventData.button == PointerEventData.InputButton.Left)
		{
			if (Core.Instance.UIGame.HeroHUD.IsAbilitySelected)
			{
				var groundPosition = UIGame.ScreenToGroundPosition(Input.mousePosition);
				var units = FindUnitsInCurrentSelection<Unit>();
				Core.Instance.UIGame.HeroHUD.ClickOnGround(groundPosition, units);
			}
			else
			{
				SelectUnitsInCurrentSelection();
			}
		}
		else
		{
			if (Core.Instance.UIGame.HeroHUD.IsAbilitySelected)
			{
				Core.Instance.UIGame.HeroHUD.CancelAbility();
			}
			else
			{
				UpdateTargetOfUnits();
			}
		}
	}

	private T[] FindUnitsInCurrentSelection<T>(string teamFilter = null) where T : Unit
	{
		var result = new LinkedList<T>();
		var rect = GetDrawingRect(leftTopOfSelection, rightBottomOfSelection);
		gameController.ForEachLogic<T>(unit =>
		{
			if (teamFilter != null && unit.Team != teamFilter)
			{
				return false;
			}

			if (IsUnitInRect(unit, rect))
			{
				result.AddLast(unit);
			}

			return false;
		});
		return result.ToArray();
	}

	private void SelectUnitsInCurrentSelection()
	{
		var units = new LinkedList<Unit>();
		var unitsInSelection = FindUnitsInCurrentSelection<Character>(gameController.Map.Sofa.Team);
		foreach (var i in unitsInSelection)
		{
			units.AddLast(i);
		}
		gameController.SelectUnits(unitsInSelection);

		leftTopOfSelection = rightBottomOfSelection = Vector2.zero;
		selectionPanel.rectTransform.sizeDelta = Vector2.zero;
		selectionPanel.enabled = false;
	}

	private void UpdateTargetOfUnits()
	{
		var selectedUnits = Core.Instance.GameController.SelectedUnits;
		if (selectedUnits.Length > 0)
		{
			Character character = null;
			var rect = GetDrawingRect(leftTopOfSelection, rightBottomOfSelection);

			gameController.ForEachLogic<Character>(unit =>
			{
				if (unit.Team == gameController.Map.Sofa.Team)
				{
					return false;
				}

				if (IsUnitInRect(unit, rect))
				{
					character = unit;
					return true;
				}

				return false;
			});

			foreach (var i in selectedUnits)
			{
				var unitAsCharacter = i as Character;
				if (unitAsCharacter != null)
				{
					if (character != null)
					{
						unitAsCharacter.SetTargetUnit(character);
					}
					else if (i is Hero)
					{
						var groundPosition = UIGame.ScreenToGroundPosition(Input.mousePosition);
						(i as Hero).MoveTo(new Vec2(groundPosition.x, groundPosition.z));
						
						var marker = PrefabTool.CreateInstance<Marker>();
						marker.Show(groundPosition);
					}
				}
			}
		}
	}

	private Rect GetDrawingRect(Vector2 start, Vector2 end)
	{
		var screenHeight = Screen.height;
		var width = (int)(end.x - start.x);
		var height = (int)((screenHeight - end.y) - (screenHeight - start.y));
		var sx = width >= 0f ? start.x : end.x;
		var sy = height >= 0f ? start.y : end.y;
		return new Rect(sx, screenHeight - sy, Mathf.Abs(width), Mathf.Abs(height));
	}

	private bool IsUnitInRect(Unit unit, Rect rect)
	{
		var worldPos = new Vector3(unit.Position.x, 1f, unit.Position.y);
		var screenPos = Camera.main.WorldToViewportPoint(worldPos);
		screenPos.y = 1f - screenPos.y;
		screenPos = new Vector3(
			screenPos.x * Screen.width,
			screenPos.y * Screen.height,
			0f);

		return rect.Contains(screenPos);
	}
}
