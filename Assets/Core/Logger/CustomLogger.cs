using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.Logger
{
    /// <summary>
    /// Custom logger solution for logs.
    /// <remarks>
    /// Logs in this class are dependant on <see langword="DEBUG"/> and <see langword="ENABLE_RELEASE_LOGS"/>. 
    /// If <see langword="ENABLE_RELEASE_LOGS"/> defined logs will displayed in Release Build.
    /// Otherwise only Editor and Developer Build will display logs.
    /// For defining preprocessor open CoreManager or write down in PlayerSettings in field "Scripting Define Symbols".
    /// </remarks>
    /// <seealso cref="Core.Managers.CoreManager"/>
    /// <seealso href="https://docs.unity3d.com/Manual/PlatformDependentCompilation.html">Scripting Define Symbols</seealso>
    /// </summary>
    public static class CustomLogger
    {
        public static void Log(string message)
        {
            #if DEBUG || ENABLE_RELEASE_LOGS
            Debug.Log(message);
            #endif
        }

        public static void Log(string message, Object context)
        {
            #if DEBUG || ENABLE_RELEASE_LOGS
            Debug.Log(message, context);
            #endif
        }
        
        public static void LogError(string message)
        {
            #if DEBUG || ENABLE_RELEASE_LOGS
            Debug.LogError(message);
            #endif
        }

        public static void LogError(string message, Object context)
        {
            #if DEBUG || ENABLE_RELEASE_LOGS
            Debug.LogError(message, context);
            #endif
        }
        
        public static void LogException(Exception exception)
        {
            #if DEBUG || ENABLE_RELEASE_LOGS
            Debug.LogException(exception);
            #endif
        }

        public static void LogException(Exception exception, Object context)
        {
            #if DEBUG || ENABLE_RELEASE_LOGS
            Debug.LogException(exception, context);
            #endif
        }
    }
}
