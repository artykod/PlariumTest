﻿using UnityEngine;
using Game;

public class GameCamera : MonoBehaviour
{
	public enum Modes
	{
		FreeMove,
		FollowMainCharacter,
	}

	private const float LERP_SPEED = 0.1f;
	private const float CAMERA_OFFSET = 5f;

	private Modes mode = Modes.FreeMove;
	private Vector3 startPosition;
	private float zoom = 0f;
	private float zoomMinMax = 10f;
	private GameController gameController;

	private void Awake()
	{
		startPosition = transform.position;
	}

	private void Start()
	{
		gameController = Core.Instance.GameController;
	}

	private void Update()
	{
		var mainCharacter = gameController.Map.Fountain.Hero;
		if (mode == Modes.FreeMove || mainCharacter == null)
		{
			var position = transform.position;
			var direction = Vector3.zero;

			if (Input.GetKey(KeyCode.A))
			{
				direction += Vector3.left;
			}
			if (Input.GetKey(KeyCode.D))
			{
				direction += Vector3.right;
			}
			if (Input.GetKey(KeyCode.W))
			{
				direction += Vector3.forward;
			}
			if (Input.GetKey(KeyCode.S))
			{
				direction += Vector3.back;
			}

			direction.Normalize();

			position += direction * 100f * Time.deltaTime;
			position.x = Mathf.Max(0f, Mathf.Min(gameController.Map.Descriptor.Width, position.x));
			position.y = startPosition.y;
			position.z = Mathf.Max(-CAMERA_OFFSET, Mathf.Min(gameController.Map.Descriptor.Height - CAMERA_OFFSET, position.z));
			transform.position = Vector3.Lerp(transform.position, position, LERP_SPEED);
		} else if (mode == Modes.FollowMainCharacter)
		{
			if (mainCharacter != null)
			{
				var characterPosition = mainCharacter.Position;
				transform.position = Vector3.Lerp(transform.position, new Vector3(characterPosition.x, startPosition.y, characterPosition.y - CAMERA_OFFSET), LERP_SPEED);
			}
		}

		zoom = 0f;
		if (Input.GetKey(KeyCode.Q))
		{
			zoom -= 1f;
		}
		if (Input.GetKey(KeyCode.E))
		{
			zoom += 1f;
		}

		if (Mathf.Abs(zoom) > 0.0001f)
		{
			var position = transform.position;
			position.y = Mathf.Lerp(position.y, startPosition.y + zoomMinMax * zoom, LERP_SPEED);
			transform.position = position;
		}
	}
}