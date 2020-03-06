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
            Trace.WriteLine(message);

            Console.ForegroundColor = orgColor;
        }

        public static void Assert(bool condition, string message = "",
                                [System.Runtime.CompilerServices.CallerLineNumber] int line = 0,
                                [System.Runtime.CompilerServices.CallerFilePath] string fileName = "")
        {
            if (condition == true)
            {
                return;
            }

            message = $"ASSERT FALSE : {fileName} - line:{line}, msg:{message}";

            Trace.Assert(condition, message);
        }
    }
}