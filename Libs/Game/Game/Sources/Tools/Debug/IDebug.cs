using System;

public interface IDebug
{
	void Log(string format, params object[] args);
	void LogWarning(string format, params object[] args);
	void LogError(string format, params object[] args);
	void LogException(Exception e);
}
