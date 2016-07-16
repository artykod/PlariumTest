using System;

public static class TypeTool
{
	private static readonly string assemblyName = typeof(TypeTool).AssemblyQualifiedName.Replace(typeof(TypeTool).Name + ", ", "");
	private static readonly string classNameWithAssemblyTemplate = "{0}, " + assemblyName;

	/// <summary>
	/// Определение типа по его имени.
	/// Тип берется из текущей assembly.
	/// </summary>
	/// <param name="className">имя искомого типа.</param>
	/// <returns>найденный тип.</returns>
	public static Type GetTypeByNameFromThisAssembly(string className)
	{
		var typeName = string.Format(classNameWithAssemblyTemplate, className);
		var type = Type.GetType(typeName);
		return type;
	}
}
