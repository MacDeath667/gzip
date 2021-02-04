using System;
using System.IO;
using GzipRoundRobin.Primitives;
using Serilog;

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
				Log.Error("Input filepath was null or unsupported format. Check arguments");
				return false;
			}

			FileInfo fileInfo = default;
			try
			{
				fileInfo = new FileInfo(filepath);
			}
			catch (Exception e)
			{
				Log.Error($"Input file can't be found by reason: {e.Message}");
				ExitHelper.ExitWithCode(1);
			}

			var fileExists = fileInfo != null && fileInfo.Exists;
			if (fileExists)
			{
				Log.Information($"Found file with size {fileInfo.Length} bytes");
			}
			else
			{
				Log.Error($"File not found on the filepath: {filepath}. Check input filepath an arguments");
			}

			return fileExists;
		}

		private static bool IsValidOutpath(string filepath)
		{
			if (filepath is null)
			{
				Log.Error("Output filepath was null or unsupported format. Check arguments");
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
				Log.Error(
					$"Output file can not be write on the filepath: \"{filepath}\" by reason: {e.Message}" +
					 $"{Environment.NewLine}Check output filepath an arguments");
				return false;
			}
		}

		private static bool IsValidGzipActionType(GzipActionType parsedArgsGzipActionType)
		{
			var isValidType = parsedArgsGzipActionType != GzipActionType.Unknown;
			
			if (isValidType)
			{
				Log.Information(
					$"Selected file will be processed with {parsedArgsGzipActionType.ToString()} option.");
			}
			else
			{
				Log.Error(
					"Check action option in CLI arguments. Must be \"Compress\" or \"Decompress\" option.");
			}

			return isValidType;
		}
	}
}