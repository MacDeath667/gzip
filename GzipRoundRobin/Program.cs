using GzipRoundRobin.Factory;
using GzipRoundRobin.Primitives;
using GzipRoundRobin.Validators;
using Serilog;

namespace GzipRoundRobin
{
	static class Program
	{
		static void Main(string[] args)
		{
			InitLogger();

			Log.Information("App start");

			if (!CliParser.TryParse(out var parsedArgs, args)
			    || !parsedArgs.Validate())
			{
				ExitHelper.ExitWithCode(1);
			}

			var threadingPreferences = AutoThreadingPreferences.Create();

			new GzipCombineFabric(parsedArgs, threadingPreferences)
				.MakeProcessChain()
				.StartProcessing();

			ExitHelper.ExitWithCode(0);
		}

		private static void InitLogger()
		{
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Information()
				.WriteTo.Console()
				.CreateLogger();
		}
	}
}