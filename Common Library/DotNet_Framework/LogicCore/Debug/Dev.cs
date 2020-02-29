using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicCore.Debug
{
	public static class Dev
	{
		//[Conditional("DEBUG")]
		public static void DebugString(string message, ConsoleColor foregroundColor = ConsoleColor.White)
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
			{
				return;
			}

			message = $"ASSERT FALSE : {fileName} - line:{line}, msg:{message}";

			Console.WriteLine(message);
			Trace.Assert(condition, message);
		}
	}
}
