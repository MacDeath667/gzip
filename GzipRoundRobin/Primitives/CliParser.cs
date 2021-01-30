using System;
using System.IO;

namespace GzipRoundRobin.Primitives
{
	public class CliParser : ICliParser<CliParserResult>
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
				case "uncompress":
					result.GzipActionType = GzipActionType.Uncompress;
					break;
				default: throw new ArgumentException("Unknown gzip action in argument");
			}

			currentArg = args[1];
			if (!File.Exists(currentArg))
			{
				throw new ArgumentException("File does not exist.");
			}

			result.FilePath = currentArg;

			currentArg = args[2];
			var fileInfo = new FileInfo(currentArg);
			result.OutPath = currentArg;
			return result;
		}
	}

	public interface ICliParser<T>
	{
		T CliParse(string[] args);
	}

	public class CliParserResult
	{
		public GzipActionType GzipActionType { get; set; }
		public string FilePath { get; set; }
		public string OutPath { get; set; }
	}

	public enum GzipActionType
	{
		Compress,
		Uncompress
	}
}