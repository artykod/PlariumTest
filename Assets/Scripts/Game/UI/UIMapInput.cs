using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Logics;
using Game.Logics.Characters;

public class UIMapInput : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	[SerializeField]
	private Image selectionPanel;

	private GameController gameController;
	private Canvas canvas;
	private Vector2 startPos;
	private Vector2 endPos;
	private LinkedList<Character> units = new LinkedList<Character>();

	private void Awake()
	{
		canvas = GetComponentInParent<Canvas>();
		selectionPanel.enabled = false;

		gameController = Core.Instance.GameController;
	}

	void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			selectionPanel.enabled = true;
			startPos = eventData.position;
		}
	}

	void IDragHandler.OnDrag(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			endPos = eventData.position;
			var r = GetDrawingRect(startPos / canvas.scaleFactor, endPos / canvas.scaleFactor);
			var selectionRT = selectionPanel.rectTransform;
			selectionRT.anchoredPosition = new Vector2(r.x, Screen.height - r.y);
			selectionRT.sizeDelta = new Vector2(r.width, r.height);
		}
	}

	void IEndDragHandler.OnEndDrag(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			units.Clear();
			var rect = GetDrawingRect(startPos, endPos);
			gameController.ForEachLogic<Character>(unit =>
			{
				if (unit.Team != gameController.Map.Sofa.Team)
				{
					return false;
				}

				if (IsUnitInRect(unit, rect))
				{
					units.AddLast(unit);
				}

				return false;
			});

			gameController.SelectUnits(units.ToArray());

			startPos = endPos = Vector2.zero;
			selectionPanel.rectTransform.sizeDelta = Vector2.zero;
			selectionPanel.enabled = false;
		}
	}

	void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
	{
		Character character = null;
		var clickPoint = eventData.position;
		var clickArea = new Vector2(30f, 35f);
		var rect = GetDrawingRect(clickPoint - clickArea, clickPoint + clickArea);

		if (eventData.button == PointerEventData.InputButton.Left)
		{
			units.Clear();

			gameController.ForEachLogic<Character>(unit =>
			{
				if (unit.Team != gameController.Map.Sofa.Team)
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

			gameController.SelectUnits(new Unit[] { character });

			units.AddLast(character);
		}
		else
		{
			if (units.Count > 0)
			{
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

				if (units.Count < 2 && character == null)
				{
					if (units.First.Value is Hero)
					{
						var clickPointScreen = Camera.main.ScreenToViewportPoint(clickPoint);
						var rayToWorld = Camera.main.ViewportPointToRay(clickPointScreen);
						var groundPlane = new Plane(Vector3.up, Vector3.zero);
						var rayDistance = 0f;
						if (groundPlane.Raycast(rayToWorld, out rayDistance))
						{
							var worldPoint = rayToWorld.GetPoint(rayDistance);
							var hero = units.First.Value as Hero;
							hero.MoveTo(new Vec2(worldPoint.x, worldPoint.z));
						}
					}
				}
				else
				{
					if (character != null)
					{
						foreach (var i in units)
						{
							i.SetTargetUnit(character);
						}
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
		var worldPos = new Vector3(unit.Position.x, 0f, unit.Position.y);
		var screenPos = Camera.main.WorldToViewportPoint(worldPos);
		screenPos.y = 1f - screenPos.y;
		screenPos = new Vector3(
			screenPos.x * Screen.width,
			screenPos.y * Screen.height,
			0f);

		return rect.Contains(screenPos);
	}
}
