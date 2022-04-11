using System;
using UnityEngine;

public class CustomDebugger
{
    public static bool CanLog = true;

    public static void Log(string str) { if (CanLog) Debug.Log(str); }
    public static void LogWarming(string str) { if (CanLog) Debug.LogWarning(str); }
    public static void LogError(string str) { if (CanLog) Debug.LogError(str); }
    public static void ThrowException(string str) { if (CanLog) throw new Exception(str); }
}
