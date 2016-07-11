using UnityDebug = UnityEngine.Debug;

public class DebugUnity : IDebug
{
	void IDebug.Log(string format, params object[] args)
	{
		UnityDebug.LogFormat(format, args);
	}

	void IDebug.LogError(string format, params object[] args)
	{
		UnityDebug.LogErrorFormat(format, args);
	}

	void IDebug.LogException(System.Exception e)
	{
		UnityDebug.LogException(e);
	}

	void IDebug.LogWarning(string format, params object[] args)
	{
		UnityDebug.LogWarningFormat(format, args);
	}
}
