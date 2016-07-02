using System;
using UnityEngine;

public static class EventTool {
	public static void SafeInvoke(this Delegate del)
	{
		SafeInvokeInternalUntyped(del);
	}

	public static void SafeInvoke<T>(this Action<T> action, T arg)
	{
		SafeInvokeInternalUntyped(action, arg);
	}

	public static void SafeInvoke<T1, T2>(this Action<T1, T2> action, T1 arg1, T2 arg2)
	{
		SafeInvokeInternalUntyped(action, arg1, arg2);
	}

	public static void SafeInvoke<T1, T2, T3>(this Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3)
	{
		SafeInvokeInternalUntyped(action, arg1, arg2, arg3);
	}

	public static void SafeInvokeInternalUntyped(Delegate del, params object[] args)
	{
		if (del != null)
		{
			var invocationList = del.GetInvocationList();
			for (int i = 0, l = invocationList.Length; i < l; i++)
			{
				try
				{
					var invocation = invocationList[i];
					if (invocation != null)
					{
						invocation.DynamicInvoke(args);
					}
				}
				catch (Exception e)
				{
					Debug.LogException(e);
				}
			}
		}
	}
}
