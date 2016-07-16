using UnityEngine;

public static class ResourcesTool
{
	public static Sprite LoadIconByName(string spriteName)
	{
		var multiSpriteName = string.Empty;
		var slashLast = spriteName.LastIndexOf('/');
		if (slashLast >= 0)
		{
			multiSpriteName = spriteName.Substring(0, slashLast);
			spriteName = spriteName.Substring(slashLast + 1);
		}

		if (string.IsNullOrEmpty(multiSpriteName))
		{
			return Resources.Load<Sprite>(Constants.Paths.Views.ICONS + spriteName);
		}
		else
		{
			var all = Resources.LoadAll<Sprite>(Constants.Paths.Views.ICONS + multiSpriteName);
			for (int i = 0; i < all.Length; i++)
			{
				var sprite = all[i];
				if (sprite.name == spriteName)
				{
					return sprite;
				}
			}
		}

		return null;
	}
}
