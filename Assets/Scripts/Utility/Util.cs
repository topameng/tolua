using UnityEngine;
using System.Collections;

public class Util 
{
	public static void LogError(string log)
	{
		Debug.Log(log);
	}

	public static void Log(string log)
	{
		Debug.LogError(log);
	}

	public static void LogWarning(string log)
	{
		Debug.LogWarning(log);
	}

}
