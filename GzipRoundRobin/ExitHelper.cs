using System;
using Serilog;

namespace GzipRoundRobin
{
	public static class ExitHelper
	{
		internal static void ExitWithCode(int exitCode)
		{
			Log.Information("Application will be closed");
			Environment.Exit(exitCode);
		}
	}
}