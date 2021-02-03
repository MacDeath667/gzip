using System;
using GzipRoundRobin.Factory;
using GzipRoundRobin.Primitives;
using GzipRoundRobin.Validators;
using NLog;

namespace GzipRoundRobin
{
	static class Program
	{
		static void Main(string[] args)
		{
			var parsedArgs = new CliParser().CliParse(args);
			ValidateArguments(parsedArgs);

			var threadingPreferences = AutoThreadingPreferences.Create();

			new GzipCombineFabric(parsedArgs, threadingPreferences)
				.MakeProcessChain()
				.StartProcessing();

			Environment.Exit(0);
		}

		private static void ValidateArguments(CliParserResult parsedArgs)
		{
			if (!ArgumentsValidateHelper.IsInputFileExists(parsedArgs.FilePath)
			    || !ArgumentsValidateHelper.IsValidOutpath(parsedArgs.OutPath)
			    || !ArgumentsValidateHelper.IsValidGzipActionType(parsedArgs.GzipActionType))
			{
				Environment.Exit(1);
			}
		}
	}
}