using LogicCore.Utility.ThirdPartyLog;
using System;
using System.Diagnostics;

namespace LogicCore.Debug
{
    public static class Dev
    {
        [Conditional("DEBUG")]
        public static void DebugString(string message, ConsoleColor foregroundColor = ConsoleColor.White)
        {
            var orgColor = Console.ForegroundColor;

            Console.ForegroundColor = foregroundColor;

            Console.WriteLine(message);
            Log4Net.WriteLog(message, Log4Net.Level.DEBUG, 0, "");

            Console.ForegroundColor = orgColor;
        }

        [Conditional("DEBUG")]
        public static void Assert(bool condition, string message = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int line = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string fileName = "")
        {
            if (condition == true)
                return;

            message = $"DEV ASSERT FALSE : {fileName} - line:{line}, msg:{message}";
            DebugString(message, ConsoleColor.Red);
            Log4Net.WriteLog(message, Log4Net.Level.FATAL, 0, "");

            throw new Exception(message);
        }
    }
}