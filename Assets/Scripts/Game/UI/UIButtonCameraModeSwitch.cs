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

		var camera = Core.Instance.Camera;
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
			var camera = Core.Instance.Camera;
			if (camera != null)
			{
				Text = "Camera: " + camera.Mode.ToString();
			}
		}
	}
}
