using System;

namespace BookStore.BLL
{
    /// <summary>
    /// Logging for the Client side;
    /// </summary>
    public static class Logger
    {
        public static void Log(string message) => Logic.Instance.Log.LogClient(message);
        public static void Warning(string message) => Logic.Instance.Log.WarningClient(message);
        public static void Fetal(string message) => Logic.Instance.Log.FetalClient(message);
        public static void Error(string message) => Logic.Instance.Log.ErrorClient(message);
        public static void Exception(Exception ex, string additonalMessage) => Logic.Instance.Log.ExceptionClient(ex, additonalMessage);
        public static void Exception(Exception ex) => Logic.Instance.Log.ExceptionClient(ex);
    }
}
