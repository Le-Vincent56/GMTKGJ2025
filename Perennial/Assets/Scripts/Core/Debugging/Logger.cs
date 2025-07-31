using System;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Perennial.Core.Debugging
{
    public static class Debugger
    {
        /// <summary>
        /// Logs a message with the specified header and log type.
        /// </summary>
        /// <param name="message">The content or body of the log message.</param>
        /// <param name="type">The log type, which determines the level of the log (Info, Warning, Error).</param>
        /// <param name="filePath">The path of the class file calling the function.</param>
        public static void Log(string message, LogType type, [CallerFilePath] string filePath = "")
        {
            string className = Path.GetFileNameWithoutExtension(filePath);
            string header = $"[{className}]";
            
            // Log the message based on the log type
            switch (type)
            {
                case LogType.Info:
                    Debug.Log($"{header} {message}");
                    break;
                
                case LogType.Warning:
                    Debug.LogWarning($"{header} {message}");
                    break;
                
                case LogType.Error:
                    Debug.LogError($"{header} {message}");
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
