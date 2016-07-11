using System;

public static class TypeTool
{
	private static readonly string assemblyName = typeof(TypeTool).AssemblyQualifiedName.Replace(typeof(TypeTool).Name + ", ", "");
	private static readonly string classNameWithAssemblyTemplate = "{0}, " + assemblyName;

	public static Type GetTypeByNameFromThisAssembly(string className)
	{
		var typeName = string.Format(classNameWithAssemblyTemplate, className);
		var type = Type.GetType(typeName);
		return type;
	}
}
