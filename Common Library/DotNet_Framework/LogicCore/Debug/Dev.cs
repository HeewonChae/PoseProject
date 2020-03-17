using LogicCore.Utility.ThirdPartyLog;
using System;
using System.Diagnostics;

namespace LogicCore.Debug
{
    public static class Dev
    {
        [Conditional("DEBUG")]
        public static void DebugString(string message, ConsoleColor foregroundColor = ConsoleColor.White,
            [System.Runtime.CompilerServices.CallerLineNumber] int line = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string fileName = "")
        {
            var orgColor = Console.ForegroundColor;

            Console.ForegroundColor = foregroundColor;

            Console.WriteLine(message);

            Console.ForegroundColor = orgColor;
        }

        public static void Assert(bool condition, string message = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int line = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string fileName = "")
        {
            if (condition == true)
                return;

            Log4Net.WriteLog(message, Log4Net.Level.FATAL, line, fileName);

            message = $"DEV ASSERT FALSE : {fileName} - line:{line}, msg:{message}";
            throw new Exception(message);
        }
    }
}