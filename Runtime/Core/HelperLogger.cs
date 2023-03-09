using System.Text;
using UnityEngine;

namespace CollieMollie.Core
{
    public static partial class Helper
    {
        public const string Broccollie = "Broccollie";

        public static void Log(string msg, string prefix = null, Object obj = null)
        {
            Debug.Log(Combine(msg, prefix), obj);
        }

        public static void LogWarning(string msg, string prefix = null, Object obj = null)
        {
            Debug.LogWarning(Combine(msg, prefix), obj);
        }

        public static void LogError(string msg, string prefix = null, Object obj = null)
        {
            Debug.LogError(Combine(msg, prefix), obj);
        }

        private static string Combine(string msg, string prefix = null)
        {
            StringBuilder builder = new StringBuilder();
            if (prefix != null)
                builder.Append($"[ {prefix} ] ");
            builder.Append(msg);
            return builder.ToString();
        }
    }
}
