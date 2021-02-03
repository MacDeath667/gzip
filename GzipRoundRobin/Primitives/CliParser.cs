using System;

namespace GzipRoundRobin.Primitives
{
	internal class CliParser
	{
		public bool TryCliParse(out CliParserResult parseResult, string[] args)
		{
			var result = new CliParserResult();
			if (args.Length != 3)
			{
				Console.WriteLine("Count of arguments must be equal 3. Actual format: [compress/decompress] [input filepath] [output filepath]");
				parseResult = result;
				return false;
			}
			
			var currentArg = args[0];

			switch (currentArg.ToLower())
			{
				case "compress":
					result.GzipActionType = GzipActionType.Compress;
					break;
				case "decompress":
					result.GzipActionType = GzipActionType.Decompress;
					break;
				default:
					result.GzipActionType = GzipActionType.Unknown;
					break;
			}

			currentArg = args[1];
			result.FilePath = currentArg;

			currentArg = args[2];
			result.OutPath = currentArg;
			
			parseResult = result;
			return true;
		}
	}
}