using System;
using System.IO;
using GzipRoundRobin.Primitives;

namespace GzipRoundRobin.Validators
{
	internal static class ArgumentsValidator
	{
		internal static bool Validate(this CliParserResult parsedArgs)
		{
			return IsInputFileExists(parsedArgs.FilePath)
			       && IsValidOutpath(parsedArgs.OutPath)
			       && IsValidGzipActionType(parsedArgs.GzipActionType);
		}

		private static bool IsInputFileExists(string filepath)
		{
			if (filepath is null)
			{
				Console.WriteLine("Input filepath was null or unsupported format. Check arguments");
				return false;
			}
			var fileInfo = new FileInfo(filepath);
			Console.WriteLine(fileInfo.Exists
				? $"Found file with size {fileInfo.Length}"
				: $"File not found on the filepath: {filepath}. Check input filepath an arguments");
			return fileInfo.Exists;
		}

		private static bool IsValidOutpath(string filepath)
		{
			if (filepath is null)
			{
				Console.WriteLine("Output filepath was null or unsupported format. Check arguments");
				return false;
			}
			try
			{
				File.Create(filepath).Close();
				File.Delete(filepath);
				return true;
			}
			catch (Exception e)
			{
				Console.WriteLine($"Output file can not be write on the filepath: \"{filepath}\" by reason: {e.Message}" +
				                  $"{Environment.NewLine}Check output filepath an arguments");
				return false;
			}
		}

		private static bool IsValidGzipActionType(GzipActionType parsedArgsGzipActionType)
		{
			var isValidType = parsedArgsGzipActionType != GzipActionType.Unknown;
			Console.WriteLine(isValidType
				? $"Selected file will be processed with {parsedArgsGzipActionType.ToString()} option."
				: "Check action option in CLI arguments. Must be \"Compress\" or \"Decompress\" option.");
			return isValidType;
		}
	}
}