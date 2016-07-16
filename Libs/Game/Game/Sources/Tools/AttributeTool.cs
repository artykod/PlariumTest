using System;

public static class AttributeTool
{
	/// <summary>
	/// Поиск аттрибута по типу.
	/// </summary>
	/// <typeparam name="T">тип искомого аттрибута.</typeparam>
	/// <param name="type">тип, у которого этот аттрибут ищется.</param>
	/// <returns>найденный аттрибут.</returns>
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
