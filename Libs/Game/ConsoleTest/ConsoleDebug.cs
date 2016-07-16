using System;

public class ConsoleDebug : IDebug
{
	void IDebug.LogError(string format, params object[] args)
	{
		Console.WriteLine("ERROR | " + string.Format(format, args));
	}

	void IDebug.LogException(Exception e)
	{
		Console.WriteLine("EXCEPTION | " + e);
	}

	void IDebug.Log(string format, params object[] args)
	{
		Console.WriteLine(string.Format(format, args));
	}

	void IDebug.LogWarning(string format, params object[] args)
	{
		Console.WriteLine("WARNING | " + string.Format(format, args));
	}
}
