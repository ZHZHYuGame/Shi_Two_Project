using System;
using UnityEngine;

namespace Unity.Services.Apis.Sample
{
    static class AppLogger
    {
        public static bool Enabled { get; set; } = true;
        public static bool Rethrow { get; set; }

        public static void Log(string log)
        {
            if (Enabled)
            {
                Debug.Log(log);
            }
        }

        public static void LogAssertion(string log)
        {
            if (Enabled)
            {
                Debug.LogAssertion(log);
            }
        }

        public static void LogError(string log)
        {
            if (Enabled)
            {
                Debug.LogError(log);
            }
        }

        public static void LogWarning(string log)
        {
            if (Enabled)
            {
                Debug.LogWarning(log);
            }
        }

        public static void LogException(Exception e)
        {
            if (Enabled)
            {
                Debug.LogException(e);
            }

            if (Rethrow)
            {
                throw e;
            }
        }
    }
}
