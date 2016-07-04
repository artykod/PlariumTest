using System;

public static class AttributeTool
{
	public static T GetAttribute<T>(this Type type) where T : Attribute
	{
		var allAttributes = type.GetCustomAttributes(typeof(T), true);
		for (int i = 0, l = allAttributes.Length; i < l; i++)
		{
			var attribute = allAttributes[i] as T;
			if (attribute != null)
			{
				return attribute;
			}
		}
		return null;
	}
}
