using UnityEngine;

public class UIButtonCameraModeSwitch : UIButton
{
	protected override void Start()
	{
		base.Start();

		Refresh();
	}

	protected override void OnClickHandler()
	{
		base.OnClickHandler();

		var camera = Core.Instance.GameCamera;
		if (camera != null)
		{
			if (camera.Mode == GameCamera.Modes.FollowMainCharacter)
			{
				camera.Mode = GameCamera.Modes.FreeMove;
			}
			else
			{
				camera.Mode = GameCamera.Modes.FollowMainCharacter;
			}
		}

		Refresh();
	}

	private void Refresh()
	{
		if (Core.Instance != null)
		{
			var camera = Core.Instance.GameCamera;
			if (camera != null)
			{
				Text = "Camera: " + camera.Mode.ToString() + "\nHotkey: SPACE";
			}
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			OnClickHandler();
		}
	}
}
