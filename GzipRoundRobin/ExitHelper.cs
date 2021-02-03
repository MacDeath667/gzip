using System;

namespace GzipRoundRobin
{
	public static class ExitHelper
	{
		internal static void ExitWithCode(int exitCode)
		{
			Console.WriteLine("Application will be closed");
			Environment.Exit(exitCode);
		}
		internal static void ExitWithCode(string exitMessage, int exitCode)
		{
			Console.WriteLine(exitMessage);
			Environment.Exit(exitCode);
		}
	}
}