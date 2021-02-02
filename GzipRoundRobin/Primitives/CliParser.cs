using System;

namespace GzipRoundRobin.Primitives
{
	public class CliParser
	{
		public CliParserResult CliParse(string[] args)
		{
			if (args.Length != 3)
			{
				throw new ArgumentException("Bad CLI args count");
			}

			var result = new CliParserResult();
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
			return result;
		}
	}
}