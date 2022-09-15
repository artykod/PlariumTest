using System;
using Game;

/// <summary>
/// Имя Debug переопределит класс Debug в юнити, что позволит удобно отключать все отладочные логи в случае необходимости.
/// </summary>
internal class Debug
{
	public static void Log(string format, params object[] args)
	{
		DebugImpl.Instance.Log(format, args);
	}
	public static void LogWarning(string format, params object[] args)
	{
		DebugImpl.Instance.LogWarning(format, args);
	}
	public static void LogError(string format, params object[] args)
	{
		DebugImpl.Instance.LogError(format, args);
	}
	public static void LogException(Exception e)
	{
		DebugImpl.Instance.LogException(e);
	}
}

namespace Game
{
	public class DebugImpl
	{
		public static IDebug Instance
		{
			get;
			set;
		}
	}
}