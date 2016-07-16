using UnityEngine;
using Game;
using System.Collections.Generic;

/// <summary>
/// Игровая камера.
/// Имеет разные режимы управления.
/// </summary>
public class GameCamera : MonoBehaviour
{
	public enum Modes
	{
		/// <summary>
		/// Свободное передвижение по карте.
		/// </summary>
		FreeMove,
		/// <summary>
		/// Следует за главным персонажем, если он жив.
		/// Иначе свободное работает передвижение.
		/// </summary>
		FollowMainCharacter,
	}

	/// <summary>
	/// Информация о различных текстурах для отображения курсора в игре.
	/// </summary>
	[System.Serializable]
	public class CursorInfo
	{
		public string name;
		public Texture2D texture;
	}

	private const float LERP_SPEED = 0.1f;
	private const float CAMERA_OFFSET = 7f;

	[SerializeField]
	private CursorInfo[] cursorsTextures;

	private Modes mode = Modes.FollowMainCharacter;
	private Vector3 startPosition;
	private float zoom = 0f;
	private float zoomMinMax = 15f;
	private GameController gameController;
	private Dictionary<string, CursorInfo> cursors = new Dictionary<string, CursorInfo>();

	public Modes Mode
	{
		get
		{
			return mode;
		}
		set
		{
			mode = value;
		}
	}

	/// <summary>
	/// Сменить игровой курсор.
	/// </summary>
	/// <param name="cursorName">уникальное имя курсора.</param>
	public void ChangeCursor(string cursorName)
	{
		var cursor = default(CursorInfo);
		if (string.IsNullOrEmpty(cursorName) || !cursors.TryGetValue(cursorName, out cursor))
		{
			Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
		}
		else
		{
			Cursor.SetCursor(cursor.texture, Vector2.zero, CursorMode.ForceSoftware);
		}
	}

	private void Awake()
	{
		startPosition = transform.position;

		foreach (var i in cursorsTextures)
		{
			cursors.Add(i.name, i);
		}
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
			var mousePosition = Input.mousePosition;
			var screenMoveLimit = Application.isEditor ? -1f : 10f;

			if (Input.GetKey(KeyCode.A) || mousePosition.x < screenMoveLimit)
			{
				direction += Vector3.left;
			}
			if (Input.GetKey(KeyCode.D) || mousePosition.x > Screen.width - screenMoveLimit)
			{
				direction += Vector3.right;
			}
			if (Input.GetKey(KeyCode.W) || mousePosition.y > Screen.height - screenMoveLimit)
			{
				direction += Vector3.forward;
			}
			if (Input.GetKey(KeyCode.S) || mousePosition.y < screenMoveLimit)
			{
				direction += Vector3.back;
			}

			direction.Normalize();

			position += direction * 200f * Time.deltaTime;
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
